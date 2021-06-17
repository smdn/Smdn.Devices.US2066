// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;
using System.Threading;

using Iot.Device.CharacterLcd;

namespace Smdn.Devices.US2066 {
  [CLSCompliant(false)]
  public abstract partial class US2066 : LcdInterface, ICGRam {
    private enum FundamentalCommandSet : byte {
      ClearDisplay          = 0b_0000_0001,
      ReturnHome            = 0b_0000_0010,
      DisplayOnOffControl   = 0b_0000_1000,
      FunctionSet           = 0b_0010_0000,
      SetCGRamAddress       = 0b_0100_0000,
      SetDDRamAddress       = 0b_1000_0000,
    }

    [Flags]
    private enum DisplayControl : byte {
      Empty   = 0b_0000_0000,
      Blink   = 0b_0000_0001,
      Cursor  = 0b_0000_0010,
      Display = 0b_0000_0100,
    }

    private enum ExtendedCommandSet : byte {
      FunctionSelectionB    = 0b_0111_0010,
      OLEDCharacterization  = 0b_0111_1000,
    }

    private enum OLEDCommandSet : byte {
      SetFadeOutAndBlinking                         = 0b_0010_0011,
      SetContrastControl                            = 0b_1000_0001,
      SetDisplayClockDivideRatioOscillatorFrequency = 0b_1101_0101,
    }

    public int CursorPosition { get; private set; } = 0;
    public int CursorLine { get; private set; } = 0;

    public DisplayLineNumber NumberOfLines { get; private set; } = DisplayLineNumber.Undefined;
    public DisplayDotFormat DotFormat { get; private set; } = DisplayDotFormat.Undefined;
    private byte functionSetNBit = default;

    public virtual int Address { get; private protected set; } = default;
    public virtual int PartID { get; private protected set; } = default;
    public virtual bool IsBusy => ReadBusyFlagAndAddressPartID().isBusy;

    private ReadOnlyMemory<(int offset, int length)> ddramAddressRanges = default;
    internal int DDRamAddressWidth { get; private set; } = default;

    public int NumberOfUserDefinedCharactersSupported {
      get => cgramUsage switch {
        CGRamUsage.OPR_00b => 8,
        CGRamUsage.OPR_01b => 8,
        CGRamUsage.OPR_10b => 6,
        CGRamUsage.OPR_11b => 0,
        _ => default,
      };
    }

    internal const int MaxNumberOfCGRamCharactersSupported = 8;

    private const int firstCGRamCharacterAlternativeCodePoint = 0xE660; // U+E660~E+E668 (Private Use Area)
    private static readonly (Rune min, Rune max) cgramCharacterAlternativeCodePointRange = (
      new Rune(firstCGRamCharacterAlternativeCodePoint),
      new Rune(firstCGRamCharacterAlternativeCodePoint + MaxNumberOfCGRamCharactersSupported)
    );

    private readonly Rune[] cgramCharacterCodePoints = new Rune[MaxNumberOfCGRamCharactersSupported];

    private CharacterGeneratorEncoding characterGenerator;

    public CharacterGeneratorEncoding CharacterGenerator {
      get => characterGenerator;
      set {
        if (value == characterGenerator)
          return; // do nothing
        if (value is null)
          throw new ArgumentNullException(nameof(CharacterGenerator), "must be non-null value");

        characterGenerator?.DetachCGRam();
        characterGenerator = value.Clone(this);

        SendFunctionSelectionBSequence(cgramUsage, characterGenerator.CGRom);
        Clear();
      }
    }

    private CGRamUsage cgramUsage = CGRamUsage.NoUserDefinedCharacters;

    public CGRamUsage CGRamUsage {
      get => cgramUsage;
      set {
        if (value == cgramUsage)
          return; // do nothing
        if (!(CGRamUsage.OPR_00b <= value && value <= CGRamUsage.OPR_11b))
          throw new ArgumentException($"invalid value ({value})", nameof(CGRamUsage));

        cgramUsage = value;

        SendFunctionSelectionBSequence(cgramUsage, characterGenerator.CGRom);
        Clear();
      }
    }

    private DisplayControl displayControl = default;

    private static DisplayControl SetDisplayControlFlag(ref DisplayControl value, DisplayControl bit, bool trueForOnOtherwiseOff)
      => value = trueForOnOtherwiseOff ? (value | bit) : (value & ~bit);

    public bool DisplayOn {
      get => displayControl.HasFlag(DisplayControl.Display);
      set => SendDisplayOnOffControlCommand(
        SetDisplayControlFlag(ref displayControl, DisplayControl.Display, value)
      );
    }

    public bool UnderlineCursorVisible {
      get => displayControl.HasFlag(DisplayControl.Cursor);
      set => SendDisplayOnOffControlCommand(
        SetDisplayControlFlag(ref displayControl, DisplayControl.Cursor, value)
      );
    }

    public bool BlinkingCursorVisible {
      get => displayControl.HasFlag(DisplayControl.Blink);
      set => SendDisplayOnOffControlCommand(
        SetDisplayControlFlag(ref displayControl, DisplayControl.Blink, value)
      );
    }

    public override bool EightBitMode => true;

    public override bool BacklightOn {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    /*
     * OLED commands
     */
    public const int MinContrast = 0x00;
    public const int MaxContrast = 0xFF;
    private byte contrast;

    public int Contrast {
      get => contrast;
      set {
        if (contrast == value)
          return; // do nothing
        contrast = (MinContrast <= value && value <= MaxContrast
          ? (byte)value
          : throw new ArgumentOutOfRangeException(nameof(Contrast), value, $"must be in range of {MinContrast}~{MaxContrast}")
        );

        SendOLEDCommandSequence(OLEDCommandSet.SetContrastControl, contrast);
      }
    }

    private byte fadeOutMode;

    public FadeOutMode FadeOutMode {
      get => (FadeOutMode)fadeOutMode;
      set {
        if (fadeOutMode == (byte)value)
          return; // do nothing

        fadeOutMode = Enum.IsDefined(value)
          ? (byte)value
          : throw new ArgumentException($"invalid value ({value})", nameof(FadeOutMode));

        SendOLEDCommandSequence(
          OLEDCommandSet.SetFadeOutAndBlinking,
          (byte)((fadeOutMode << 4) | fadeOutInterval)
        );
      }
    }

    private byte fadeOutInterval;

    public FadeOutInterval FadeOutInterval {
      get => (FadeOutInterval)fadeOutInterval;
      set {
        if (fadeOutInterval == (byte)value)
          return; // do nothing

        fadeOutInterval = Enum.IsDefined(value)
          ? (byte)value
          : throw new ArgumentException($"invalid value ({value})", nameof(FadeOutInterval));

        SendOLEDCommandSequence(OLEDCommandSet.SetFadeOutAndBlinking, SetFadeOutAndBlinkingSecondByte);
      }
    }

    private byte SetFadeOutAndBlinkingSecondByte => (byte)((fadeOutMode << 4) | fadeOutInterval);

    private byte clockDivideRatio;

    public ClockDivideRatio ClockDivideRatio {
      get => (ClockDivideRatio)clockDivideRatio;
      set {
        if (clockDivideRatio == (byte)value)
          return; // do nothing

        clockDivideRatio = Enum.IsDefined(value)
          ? (byte)value
          : throw new ArgumentException($"invalid value ({value})", nameof(ClockDivideRatio));

        SendOLEDCommandSequence(
          OLEDCommandSet.SetDisplayClockDivideRatioOscillatorFrequency,
          SetDisplayClockDivideRatioOscillatorFrequencySecondByte
        );
      }
    }

    private byte internalOscillatorFrequency;

    public InternalOscillatorFrequency InternalOscillatorFrequency {
      get => (InternalOscillatorFrequency)internalOscillatorFrequency;
      set {
        if (internalOscillatorFrequency == (byte)value)
          return; // do nothing

        internalOscillatorFrequency = Enum.IsDefined(value)
          ? (byte)value
          : throw new ArgumentException($"invalid value ({value})", nameof(InternalOscillatorFrequency));

        SendOLEDCommandSequence(
          OLEDCommandSet.SetDisplayClockDivideRatioOscillatorFrequency,
          SetDisplayClockDivideRatioOscillatorFrequencySecondByte
        );
      }
    }

    private byte SetDisplayClockDivideRatioOscillatorFrequencySecondByte => (byte)((internalOscillatorFrequency << 4) | clockDivideRatio);

    public virtual void Initialize(
      DisplayLineNumber numberOfLines,
      DisplayDotFormat dotFormat
    )
    {
      NumberOfLines = Enum.IsDefined(numberOfLines)
        ? numberOfLines
        : throw new ArgumentException($"invalid value ({numberOfLines})", nameof(numberOfLines));
      DotFormat = Enum.IsDefined(dotFormat)
        ? dotFormat
        : throw new ArgumentException($"invalid value ({dotFormat})", nameof(dotFormat));

      functionSetNBit = NumberOfLines switch {
        DisplayLineNumber.Lines1 or DisplayLineNumber.Lines3 => 0b_0000,
        DisplayLineNumber.Lines2 or DisplayLineNumber.Lines4 => 0b_1000,
        _ => default,
      };

      (DDRamAddressWidth, ddramAddressRanges) = NumberOfLines switch {
        DisplayLineNumber.Lines1 => (0x50, new[] { (0x00, 0x50) }),
        DisplayLineNumber.Lines2 => (0x28, new[] { (0x00, 0x28), (0x40, 0x28) }),
        DisplayLineNumber.Lines3 => (0x28, new[] { (0x00, 0x14), (0x20, 0x14), (0x40, 0x14) }),
        DisplayLineNumber.Lines4 => (0x14, new[] { (0x00, 0x14), (0x20, 0x14), (0x40, 0x14), (0x60, 0x14) }),
        _ => default,
      };

      // set display off
      SendDisplayOnOffControlCommand(DisplayControl.Empty);

      CGRamUsage = CGRamUsage.OPR_11b;

      // clear CGRAM data
      Span<byte> emptyCharacterData = stackalloc byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      for (var c = CGRamCharacter.Min; c <= CGRamCharacter.Max; c++) {
        WriteCGRamCharacter(c, emptyCharacterData);
      }

      CharacterGenerator = CharacterGeneratorEncoding.CGRomC;
      //Clear(); this will be called by CharacterGenerator.Setter

      Home();

      contrast = 0xff;
      SendOLEDCommandSequence(
        OLEDCommandSet.SetContrastControl,
        contrast
      );

      fadeOutMode = (byte)FadeOutMode.Disabled;
      fadeOutInterval = (byte)FadeOutInterval.Min;
      SendOLEDCommandSequence(
        OLEDCommandSet.SetFadeOutAndBlinking,
        SetFadeOutAndBlinkingSecondByte
      );

      clockDivideRatio = (byte)ClockDivideRatio.Default;
      internalOscillatorFrequency = (byte)InternalOscillatorFrequency.Default;
      SendOLEDCommandSequence(
        OLEDCommandSet.SetDisplayClockDivideRatioOscillatorFrequency,
        SetDisplayClockDivideRatioOscillatorFrequencySecondByte
      );

      SetDisplayControlFlag(ref displayControl, DisplayControl.Display, true);
      SetDisplayControlFlag(ref displayControl, DisplayControl.Cursor, true);
      SetDisplayControlFlag(ref displayControl, DisplayControl.Blink, true);

      Thread.Sleep(TimeSpan.FromMilliseconds(100));

      SendDisplayOnOffControlCommand(displayControl);

      Clear();

#if false
      (_, Address, PartID) = ReadBusyFlagAndAddressPartID();
#endif
    }

    protected abstract void SendByteSequence(byte controlByte, ReadOnlySpan<byte> byteSequence);

    public override void SendData(ReadOnlySpan<char> values)
    {
      if (values.IsEmpty)
        return;

      var maxLengthToSend = DDRamAddressWidth - CursorPosition;

      if (maxLengthToSend == 0)
        return; // reached to the end of current line

      Span<byte> dataSequence = stackalloc byte[maxLengthToSend];

      var len = characterGenerator.GetBytes(values, dataSequence);

      SendDataAwaitExecutionTime(dataSequence.Slice(0, len));

      CursorPosition += len;
    }

    public override void SendData(byte value)
      => SendData(stackalloc byte[1] { value });

    public override void SendData(ReadOnlySpan<byte> values)
    {
      if (values.IsEmpty)
        return;

      var maxLengthToSend = DDRamAddressWidth - CursorPosition;

      if (maxLengthToSend == 0)
        return; // reached to the end of current line

      var lengthToSend = Math.Min(values.Length, maxLengthToSend);

      SendDataAwaitExecutionTime(values.Slice(0, lengthToSend));

      CursorPosition += lengthToSend;
    }

    private static readonly TimeSpan executionTime_1_52ms = TimeSpan.FromMilliseconds(1.52);
    private static readonly TimeSpan executionTime_37us = TimeSpan.FromMilliseconds(0.037);

    private void SendDataAwaitExecutionTime(ReadOnlySpan<byte> dataSequence)
    {
      const byte controlByte =
        0b_0_0000000 | // Continuation bit (0: the transmission of the following information will contain data bytes only)
        0b__1_000000;  // Data / Command Selection bit (1: defines the following data byte as a data)

      SendByteSequence(controlByte, dataSequence);

      Thread.Sleep(executionTime_37us);
    }

    public override void SendCommands(ReadOnlySpan<byte> values)
    {
      const byte controlByte =
        0b_0_0000000 | // Continuation bit (0: the transmission of the following information will contain data bytes only)
        0b__0_000000;  // Data / Command Selection bit (0: defines the following data byte as a command)

      SendByteSequence(controlByte, values);
    }

    public void Clear()
    {
      SendCommandAwaitExcecutionTime((byte)FundamentalCommandSet.ClearDisplay, executionTime_1_52ms);

      CursorPosition = 0;
      CursorLine = 0;
    }

    public void Home()
    {
      SendCommandAwaitExcecutionTime((byte)FundamentalCommandSet.ReturnHome, executionTime_1_52ms);

      CursorPosition = 0;
      CursorLine = 0;
    }

    public void SetCursorPosition(int line, int position)
    {
      int ThrowIfCursorLineOutOfRange(int line, string paramName)
        => (0 <= line && line < ddramAddressRanges.Length)
          ? line
          : throw new ArgumentOutOfRangeException(paramName, line, $"must be in range between 0 and {ddramAddressRanges.Length}");
      int ThrowIfCursorPositionOutOfRange(int line, int position, string paramName)
        => (0 <= position && position < ddramAddressRanges.Span[line].length)
          ? position
          : throw new ArgumentOutOfRangeException(paramName, position, $"must be in range between 0 and {ddramAddressRanges.Span[line].length}");

      SendSetDDRamAddressCommand(
        (byte)(
          ddramAddressRanges.Span[ThrowIfCursorLineOutOfRange(line, nameof(line))].offset + ThrowIfCursorPositionOutOfRange(line, position, nameof(position))
        )
      );

      CursorLine = line;
      CursorPosition = position;
    }

    public void ResetFadeOutStep()
    {
      // disable fade out and blinking temporarily to reset fade out step counter
      SendOLEDCommandSequence(OLEDCommandSet.SetFadeOutAndBlinking, 0b_00_0000_00);

      // then revert fade out mode and interval
      SendOLEDCommandSequence(OLEDCommandSet.SetFadeOutAndBlinking, SetFadeOutAndBlinkingSecondByte);
    }

    private void SendOLEDCommandSequence(OLEDCommandSet command, byte? secondByte = default)
    {
      const byte extensionRegisterRE = 0b_0000_0010;
      const byte extensionRegisterIS = 0b_0000_0001;

      // RE=1
      SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.FunctionSet | functionSetNBit | extensionRegisterRE), executionTime_37us);

      try {
        // IS=1
        SendCommandAwaitExcecutionTime((byte)((byte)ExtendedCommandSet.OLEDCharacterization | extensionRegisterIS), executionTime_37us);

        try {
          SendCommandAwaitExcecutionTime((byte)command, executionTime_37us);

          if (secondByte.HasValue)
            SendCommandAwaitExcecutionTime(secondByte.Value, executionTime_37us);
        }
        finally {
          // IS=0
          SendCommandAwaitExcecutionTime((byte)ExtendedCommandSet.OLEDCharacterization, executionTime_37us);
        }
      }
      finally {
        // RE=0
        SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.FunctionSet | functionSetNBit), executionTime_37us);
      }
    }

    private void SendFunctionSelectionBSequence(CGRamUsage opr, CGRom rom)
    {
      const byte extensionRegisterRE = 0b_0000_0010;

      // RE=1
      SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.FunctionSet | functionSetNBit | extensionRegisterRE), executionTime_37us);

      try {
        SendCommandAwaitExcecutionTime((byte)ExtendedCommandSet.FunctionSelectionB, executionTime_37us);

        SendDataAwaitExecutionTime(stackalloc byte[1] {
          (byte)(
            (Enum.IsDefined(opr) ? (byte)opr : (byte)0b_00 /*as default*/) |
            (Enum.IsDefined(rom) ? (byte)rom : (byte)CGRom.Invalid)
          )
        });
      }
      finally {
        // RE=0
        SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.FunctionSet | functionSetNBit), executionTime_37us);
      }
    }

    internal void SendSetDDRamAddressCommand(byte address)
      => SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.SetDDRamAddress | address), executionTime_37us);

    /// <returns>Returns alternative <see cref="System.Char"><c>char</c></see> value for registered character.</returns>
    public char RegisterCGRamCharacter(CGRamCharacter character, Rune characterCodePoint, ReadOnlySpan<byte> characterData)
    {
      if (!(CGRamCharacter.Character0 <= character && character <= CGRamCharacter.Character7))
        throw new ArgumentOutOfRangeException(nameof(character), character, $"must be in range of {CGRamCharacter.Character0}~{CGRamCharacter.Character7}");

      if (characterCodePoint != default(Rune))
        cgramCharacterCodePoints[(int)character] = characterCodePoint;

      WriteCGRamCharacter(character, characterData);

      // reset DDRAM address (required to refresh display?)
      SetCursorPosition(CursorLine, CursorPosition);

      return (char)(cgramCharacterAlternativeCodePointRange.min.Value + (int)character);
    }

    private void WriteCGRamCharacter(CGRamCharacter character, ReadOnlySpan<byte> characterData)
    {
      if (characterData.Length != 8)
        throw new ArgumentException($"length of {nameof(characterData)} must be 8 bytes exactly", nameof(characterData));

      var cgramAddress = (byte)((int)character << 3);

      SendSetCGRamAddressCommand(cgramAddress);
      SendDataAwaitExecutionTime(characterData);
    }

    internal byte GetUserDefinedCharacterByte(int index)
    {
      const byte fallback = 0x20; // SPACE

      if (NumberOfUserDefinedCharactersSupported <= index)
        return fallback;
      else
        return (byte)(0x00 + index);
    }

    bool ICGRam.GetByte(Rune codePoint, out byte by)
    {
      by = default;

      if (cgramCharacterAlternativeCodePointRange.min <= codePoint && codePoint <= cgramCharacterAlternativeCodePointRange.max) {
        by = GetUserDefinedCharacterByte(codePoint.Value - cgramCharacterAlternativeCodePointRange.min.Value);
        return true;
      }

      for (var i = 0; i < cgramCharacterCodePoints.Length; i++) {
        if (cgramCharacterCodePoints[i] == codePoint) {
          by = GetUserDefinedCharacterByte(i);
          return true;
        }
      }

      return false;
    }

    private void SendSetCGRamAddressCommand(byte address)
      => SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.SetCGRamAddress | address), executionTime_37us);

    private void SendDisplayOnOffControlCommand(DisplayControl displayControl)
      => SendCommandAwaitExcecutionTime((byte)((byte)FundamentalCommandSet.DisplayOnOffControl | (byte)displayControl), executionTime_37us);

    private void SendCommandAwaitExcecutionTime(byte command, TimeSpan expectedExecutionTime)
    {
      SendCommands((ReadOnlySpan<byte>)stackalloc byte[1] { command });

      Thread.Sleep(expectedExecutionTime);
    }

    public override void SendCommand(byte command)
      => SendCommands((ReadOnlySpan<byte>)stackalloc byte[1] { command });


    /*
     * read operations
     */
    protected abstract byte ReceiveByte(byte controlByte);

    private (bool isBusy, int address, int partId) ReadBusyFlagAndAddressPartID()
    {
      const byte controlByte =
        0b_0_0000000 | // Continuation bit (0: the transmission of the following information will contain data bytes only)
        0b__0_000000;  // Data / Command Selection bit (0: defines the following data byte as a command)

      Thread.Sleep(executionTime_37us);

      var first  = ReceiveByte(controlByte); // first time: address counter
      var second = ReceiveByte(controlByte); // second time: part ID

      return (
        isBusy:   (second & 0b_1_0000000) != 0,
        address:   first  & 0b_0_1111111,
        partId:    second & 0b_0_1111111
      );
    }
  }
}
