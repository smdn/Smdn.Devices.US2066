// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Linq;
using System.Text;

using Iot.Device.CharacterLcd;

namespace Smdn.Devices.US2066;

public abstract class US2066DisplayModuleBase : ICharacterLcd {
  protected abstract DisplayLineNumber DisplayLineNumber { get; }
  protected abstract DisplayDotFormat DisplayDotFormat { get; }

  public abstract int NumberOfCharsPerLine { get; }
  public int NumberOfLines => DisplayLineNumber switch {
    DisplayLineNumber.Lines1 => 1,
    DisplayLineNumber.Lines2 => 2,
    DisplayLineNumber.Lines3 => 3,
    DisplayLineNumber.Lines4 => 4,
    _ => throw new NotSupportedException($"unsupported display line number (DisplayLineNumber)"),
  };

  private US2066 oledInterface;
  private US2066 OLEDInterface => oledInterface ?? throw new ObjectDisposedException(GetType().FullName);

  [CLSCompliant(false)]
  public US2066 Controller => OLEDInterface;

  [CLSCompliant(false)]
  protected US2066DisplayModuleBase(US2066 oledInterface)
  {
    this.oledInterface = oledInterface;

    this.oledInterface.Initialize(
      numberOfLines: DisplayLineNumber,
      dotFormat: DisplayDotFormat
    );
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;

    oledInterface?.Dispose();
    oledInterface = null;
  }

  public int Address => OLEDInterface.Address;
  public int PartID => OLEDInterface.PartID;
  public bool IsBusy => OLEDInterface.IsBusy;

  public int CursorLeft {
    get => OLEDInterface.CursorPosition;
    set => OLEDInterface.SetCursorPosition(OLEDInterface.CursorLine, value);
  }

  public int CursorTop {
    get => OLEDInterface.CursorLine;
    set => OLEDInterface.SetCursorPosition(value, OLEDInterface.CursorPosition);
  }

  private static bool ThrowBacklightNotSupportedException()
    => throw new NotSupportedException("Backlight is not supported with US2066");

  bool ICharacterLcd.BacklightOn {
    get => ThrowBacklightNotSupportedException();
    set => ThrowBacklightNotSupportedException();
  }

  public bool DisplayOn {
    get => OLEDInterface.DisplayOn;
    set => OLEDInterface.DisplayOn = value;
  }

  public bool UnderlineCursorVisible {
    get => OLEDInterface.UnderlineCursorVisible;
    set => OLEDInterface.UnderlineCursorVisible = value;
  }

  public bool BlinkingCursorVisible {
    get => OLEDInterface.BlinkingCursorVisible;
    set => OLEDInterface.BlinkingCursorVisible = value;
  }

  SixLabors.ImageSharp.Size ICharacterLcd.Size => new(NumberOfCharsPerLine, NumberOfLines);

  public int NumberOfCustomCharactersSupported => OLEDInterface.NumberOfUserDefinedCharactersSupported;

  public int Contrast {
    get => OLEDInterface.Contrast;
    set => OLEDInterface.Contrast = value;
  }

  public FadeOutMode FadeOutMode {
    get => OLEDInterface.FadeOutMode;
    set => OLEDInterface.FadeOutMode = value;
  }

  public FadeOutInterval FadeOutInterval {
    get => OLEDInterface.FadeOutInterval;
    set => OLEDInterface.FadeOutInterval = value;
  }

  public ClockDivideRatio ClockDivideRatio {
    get => OLEDInterface.ClockDivideRatio;
    set => OLEDInterface.ClockDivideRatio = value;
  }

  public InternalOscillatorFrequency InternalOscillatorFrequency {
    get => OLEDInterface.InternalOscillatorFrequency;
    set => OLEDInterface.InternalOscillatorFrequency = value;
  }

  public CharacterGeneratorEncoding CharacterGenerator {
    get => OLEDInterface.CharacterGenerator;
    set => OLEDInterface.CharacterGenerator = value;
  }

  public CGRamUsage CGRamUsage {
    get => OLEDInterface.CGRamUsage;
    set => OLEDInterface.CGRamUsage = value;
  }

  public void Clear()
    => OLEDInterface.Clear();

  public void Home()
    => OLEDInterface.Home();

  public void CreateCustomCharacter(int location, ReadOnlySpan<byte> characterMap)
    => CreateCustomCharacter((CGRamCharacter)location, default(Rune), characterMap);

  /// <returns>Returns alternative <see cref="char"><c>char</c></see> value for registered character.</returns>
  public char CreateCustomCharacter(CGRamCharacter character, ReadOnlySpan<byte> characterData)
    => OLEDInterface.RegisterCGRamCharacter(character, default, characterData);

  /// <returns>Returns alternative <see cref="char"><c>char</c></see> value for registered character.</returns>
  public char CreateCustomCharacter(CGRamCharacter character, int characterCodePoint, ReadOnlySpan<byte> characterData)
    => OLEDInterface.RegisterCGRamCharacter(character, new Rune(characterCodePoint), characterData);

  /// <returns>Returns alternative <see cref="char"><c>char</c></see> value for registered character.</returns>
  public char CreateCustomCharacter(CGRamCharacter character, char characterCodePoint, ReadOnlySpan<byte> characterData)
    => OLEDInterface.RegisterCGRamCharacter(character, new Rune(characterCodePoint), characterData);

  /// <returns>Returns alternative <see cref="char"><c>char</c></see> value for registered character.</returns>
  public char CreateCustomCharacter(CGRamCharacter character, string characterCodePointString, ReadOnlySpan<byte> characterData)
    => OLEDInterface.RegisterCGRamCharacter(
      character,
      string.IsNullOrEmpty(characterCodePointString) ? throw new ArgumentException("must be non-null and non-empty string", nameof(characterCodePointString)) : characterCodePointString.EnumerateRunes().First(),
      characterData
    );

  /// <returns>Returns alternative <see cref="char"><c>char</c></see> value for registered character.</returns>
  public char CreateCustomCharacter(CGRamCharacter character, Rune characterCodePoint, ReadOnlySpan<byte> characterData)
    => OLEDInterface.RegisterCGRamCharacter(character, characterCodePoint, characterData);

  public void SetCursorPosition(int left, int top)
    => OLEDInterface.SetCursorPosition(top, left);

  public void SetCursorPosition((int left, int top) position)
    => OLEDInterface.SetCursorPosition(position.top, position.left);

  public (int left, int top) GetCursorPosition()
    => (OLEDInterface.CursorPosition, OLEDInterface.CursorLine);

  public void Write(string text)
    => Write((text ?? throw new ArgumentNullException(nameof(text))).AsSpan());

  public void WriteLine(string text)
  {
    Write(text);
    WriteLine();
  }

  public void Write(ReadOnlySpan<char> text)
    => OLEDInterface.SendData(text);

  public void WriteLine(ReadOnlySpan<char> text)
  {
    Write(text);
    WriteLine();
  }

  public void Write(ReadOnlySpan<byte> text)
    => OLEDInterface.SendData(text);

  public void WriteLine(ReadOnlySpan<byte> text)
  {
    Write(text);
    WriteLine();
  }

  public void Write(byte character)
    => OLEDInterface.SendData(character);

  public void WriteLine(byte character)
  {
    Write(character);
    WriteLine();
  }

  public void Write(CGRamCharacter character)
    => OLEDInterface.SendData(
      OLEDInterface.GetUserDefinedCharacterByte(
        character is >= CGRamCharacter.Min and <= CGRamCharacter.Max
          ? (int)character
          : throw new ArgumentException($"invalid value ({character})", nameof(character))
      )
    );

  public void WriteLine(CGRamCharacter character)
  {
    Write(character);
    WriteLine();
  }

  public void WriteLine()
  {
    if (OLEDInterface.CursorLine + 1 == NumberOfLines)
      SetCursorPosition(OLEDInterface.DDRamAddressWidth - 1, OLEDInterface.CursorLine);
    else
      SetCursorPosition(0, OLEDInterface.CursorLine + 1);
  }

  public void ResetFadeOutStep()
    => OLEDInterface.ResetFadeOutStep();
}
