// Smdn.Devices.US2066.dll (Smdn.Devices.US2066-0.9.1 (net5.0))
//   Name: Smdn.Devices.US2066
//   AssemblyVersion: 0.9.1.0
//   InformationalVersion: 0.9.1 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;
using Iot.Device.CharacterLcd;
using SixLabors.ImageSharp;
using Smdn.Devices.US2066;

namespace Smdn.Devices.US2066 {
  public enum CGRamCharacter : int {
    Character0 = 0,
    Character1 = 1,
    Character2 = 2,
    Character3 = 3,
    Character4 = 4,
    Character5 = 5,
    Character6 = 6,
    Character7 = 7,
    Max = 7,
    Min = 0,
  }

  public enum CGRamUsage : byte {
    NoUserDefinedCharacters = 3,
    OPR_00b = 0,
    OPR_01b = 1,
    OPR_10b = 2,
    OPR_11b = 3,
    UserDefined6Characters = 2,
    UserDefined8Characters = 1,
  }

  public enum ClockDivideRatio : byte {
    Default = 0,
    Ratio1 = 0,
    Ratio10 = 9,
    Ratio11 = 10,
    Ratio12 = 11,
    Ratio13 = 12,
    Ratio14 = 13,
    Ratio15 = 14,
    Ratio16 = 15,
    Ratio2 = 1,
    Ratio3 = 2,
    Ratio4 = 3,
    Ratio5 = 4,
    Ratio6 = 5,
    Ratio7 = 6,
    Ratio8 = 7,
    Ratio9 = 8,
  }

  public enum DisplayDotFormat : int {
    Dots5x8 = 1,
    Dots6x8 = 2,
    Undefined = 0,
  }

  public enum DisplayLineNumber : int {
    Lines1 = 1,
    Lines2 = 2,
    Lines3 = 3,
    Lines4 = 4,
    Undefined = 0,
  }

  public enum FadeOutInterval : byte {
    Max = 15,
    Min = 0,
    Step104Frames = 12,
    Step112Frames = 13,
    Step120Frames = 14,
    Step128Frames = 15,
    Step16Frames = 1,
    Step24Frames = 2,
    Step32Frames = 3,
    Step40Frames = 4,
    Step48Frames = 5,
    Step56Frames = 6,
    Step64Frames = 7,
    Step72Frames = 8,
    Step80Frames = 9,
    Step88Frames = 10,
    Step8Frames = 0,
    Step96Frames = 11,
  }

  public enum FadeOutMode : byte {
    Blinking = 3,
    Disabled = 0,
    FadeOut = 2,
  }

  public enum InternalOscillatorFrequency : byte {
    Default = 7,
    Frequency1 = 0,
    Frequency10 = 9,
    Frequency11 = 10,
    Frequency12 = 11,
    Frequency13 = 12,
    Frequency14 = 13,
    Frequency15 = 14,
    Frequency16 = 15,
    Frequency2 = 1,
    Frequency3 = 2,
    Frequency4 = 3,
    Frequency5 = 4,
    Frequency6 = 5,
    Frequency7 = 6,
    Frequency8 = 7,
    Frequency9 = 8,
  }

  public abstract class CharacterGeneratorEncoderCollationFallback : CharacterGeneratorEncoderFallback {
    public abstract EncoderFallbackBuffer CreateFallbackBuffer();
  }

  public class CharacterGeneratorEncoderFallback : EncoderFallback {
    public static readonly CharacterGeneratorEncoderFallback Default; // = "Smdn.Devices.US2066.CharacterGeneratorEncoderFallback"
    public const string DefaultReplacementString = " ";

    public CharacterGeneratorEncoderFallback(string defaultReplacementString = " ", bool enableCollation = true) {}

    public override int MaxCharCount { get; }
    public string ReplacementString { get; }

    public override EncoderFallbackBuffer CreateFallbackBuffer() {}
  }

  public class CharacterGeneratorEncoderFallbackBuffer : EncoderFallbackBuffer {
    public CharacterGeneratorEncoderFallbackBuffer(CharacterGeneratorEncoderFallback fallback) {}

    protected string DefaultReplacementString { get; }
    public override int Remaining { get; }

    public override bool Fallback(char charUnknown, int index) {}
    public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index) {}
    public override char GetNextChar() {}
    protected virtual string GetReplacement(char charUnknown) {}
    protected virtual string GetReplacement(char charUnknownHigh, char charUnknownLow) {}
    public override bool MovePrevious() {}
    public override void Reset() {}
  }

  public abstract class CharacterGeneratorEncoding : Encoding {
    public static readonly CharacterGeneratorEncoding CGRomA; // = "Smdn.Devices.US2066.CharacterGeneratorRomAEncoding"
    public static readonly CharacterGeneratorEncoding CGRomB; // = "Smdn.Devices.US2066.CharacterGeneratorRomBEncoding"
    public static readonly CharacterGeneratorEncoding CGRomBRussian; // = "Smdn.Devices.US2066.CharacterGeneratorRomBRussianEncoding"
    public static readonly CharacterGeneratorEncoding CGRomC; // = "Smdn.Devices.US2066.CharacterGeneratorRomCEncoding"
    public static readonly CharacterGeneratorEncoding CGRomCJapanese; // = "Smdn.Devices.US2066.CharacterGeneratorRomCJapaneseEncoding"

    [PreserveBaseOverrides]
    public virtual Encoding Clone() {}
    public override int GetByteCount(ReadOnlySpan<char> chars) {}
    public override int GetByteCount(char[] chars, int index, int count) {}
    public override int GetByteCount(string s) {}
    public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) {}
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetCharCount(byte[] bytes, int index, int count) {}
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {}
    public override int GetMaxByteCount(int charCount) {}
    public override int GetMaxCharCount(int byteCount) {}
    public IEnumerable<Rune> GetRunesForByte(byte byt) {}
    public static bool IsUndefinedCharacter(char ch) {}
    public static bool IsUnmappedCharacter(char ch) {}
  }

  public class CharacterGeneratorRomAEncoding : CharacterGeneratorEncoding {
    public CharacterGeneratorRomAEncoding(EncoderFallback encoderFallback) {}
    public CharacterGeneratorRomAEncoding(string defaultReplacementString = " ", bool enableCollation = true) {}

    public override string EncodingName { get; }
  }

  public class CharacterGeneratorRomBEncoding : CharacterGeneratorEncoding {
    public CharacterGeneratorRomBEncoding(EncoderFallback encoderFallback) {}
    public CharacterGeneratorRomBEncoding(string defaultReplacementString = " ", bool enableCollation = true) {}

    public override string EncodingName { get; }
  }

  public class CharacterGeneratorRomBRussianEncoderFallback : CharacterGeneratorEncoderCollationFallback {
    public CharacterGeneratorRomBRussianEncoderFallback(string defaultReplacementString = " ") {}

    public override EncoderFallbackBuffer CreateFallbackBuffer() {}
  }

  public class CharacterGeneratorRomBRussianEncoderFallbackBuffer : CharacterGeneratorEncoderFallbackBuffer {
    public CharacterGeneratorRomBRussianEncoderFallbackBuffer(CharacterGeneratorRomBRussianEncoderFallback fallback) {}

    protected override string GetReplacement(char charUnknown) {}
  }

  public class CharacterGeneratorRomBRussianEncoding : CharacterGeneratorRomBEncoding {
    public CharacterGeneratorRomBRussianEncoding(string defaultReplacementString = " ") {}

    public override string EncodingName { get; }
  }

  public class CharacterGeneratorRomCEncoding : CharacterGeneratorEncoding {
    public CharacterGeneratorRomCEncoding(EncoderFallback encoderFallback) {}
    public CharacterGeneratorRomCEncoding(string defaultReplacementString = " ", bool enableCollation = true) {}

    public override string EncodingName { get; }
  }

  public class CharacterGeneratorRomCJapaneseEncoderFallback : CharacterGeneratorEncoderCollationFallback {
    public CharacterGeneratorRomCJapaneseEncoderFallback(string defaultReplacementString = " ") {}

    public override EncoderFallbackBuffer CreateFallbackBuffer() {}
  }

  public class CharacterGeneratorRomCJapaneseEncoding : CharacterGeneratorRomCEncoding {
    public CharacterGeneratorRomCJapaneseEncoding(string defaultReplacementString = " ") {}

    public override string EncodingName { get; }
  }

  public class SO1602A : SOXXXXA {
    public override int NumberOfCharsPerLine { get; }

    public static SO1602A Create(I2cConnectionSettings connectionSettings) {}
    public static SO1602A Create(I2cDevice i2cDevice) {}
    public static SO1602A Create(int deviceAddress, int busId = 1) {}
  }

  public class SO2002A : SOXXXXA {
    public override int NumberOfCharsPerLine { get; }

    public static SO2002A Create(I2cConnectionSettings connectionSettings) {}
    public static SO2002A Create(I2cDevice i2cDevice) {}
    public static SO2002A Create(int deviceAddress, int busId = 1) {}
  }

  public abstract class SOXXXXA : US2066DisplayModuleBase {
    public const byte DefaultI2CAddress = 60;
    public const byte SecondaryI2CAddress = 61;

    protected override DisplayDotFormat DisplayDotFormat { get; }
    protected override DisplayLineNumber DisplayLineNumber { get; }
  }

  public abstract class US2066 : LcdInterface {
    public const int MaxContrast = 255;
    public const int MinContrast = 0;

    protected US2066() {}

    public virtual int Address { get; }
    public override bool BacklightOn { get; set; }
    public bool BlinkingCursorVisible { get; set; }
    public CGRamUsage CGRamUsage { get; set; }
    public CharacterGeneratorEncoding CharacterGenerator { get; set; }
    public ClockDivideRatio ClockDivideRatio { get; set; }
    public int Contrast { get; set; }
    public int CursorLine { get; }
    public int CursorPosition { get; }
    public bool DisplayOn { get; set; }
    public DisplayDotFormat DotFormat { get; }
    public override bool EightBitMode { get; }
    public FadeOutInterval FadeOutInterval { get; set; }
    public FadeOutMode FadeOutMode { get; set; }
    public InternalOscillatorFrequency InternalOscillatorFrequency { get; set; }
    public virtual bool IsBusy { get; }
    public DisplayLineNumber NumberOfLines { get; }
    public int NumberOfUserDefinedCharactersSupported { get; }
    public virtual int PartID { get; }
    public bool UnderlineCursorVisible { get; set; }

    public void Clear() {}
    public static US2066 Create(I2cConnectionSettings connectionSettings) {}
    public static US2066 Create(I2cDevice i2cDevice) {}
    public static US2066 Create(int deviceAddress, int busId = 1) {}
    public void Home() {}
    public virtual void Initialize(DisplayLineNumber numberOfLines, DisplayDotFormat dotFormat) {}
    protected abstract byte ReceiveByte(byte controlByte);
    public char RegisterCGRamCharacter(CGRamCharacter character, Rune characterCodePoint, ReadOnlySpan<byte> characterData) {}
    public void ResetFadeOutStep() {}
    protected abstract void SendByteSequence(byte controlByte, ReadOnlySpan<byte> byteSequence);
    public override void SendCommand(byte command) {}
    public override void SendCommands(ReadOnlySpan<byte> values) {}
    public override void SendData(ReadOnlySpan<byte> values) {}
    public override void SendData(ReadOnlySpan<char> values) {}
    public override void SendData(byte @value) {}
    public void SetCursorPosition(int line, int position) {}
  }

  public abstract class US2066DisplayModuleBase : ICharacterLcd {
    protected US2066DisplayModuleBase(US2066 oledInterface) {}

    public int Address { get; }
    public bool BlinkingCursorVisible { get; set; }
    public CGRamUsage CGRamUsage { get; set; }
    public CharacterGeneratorEncoding CharacterGenerator { get; set; }
    public ClockDivideRatio ClockDivideRatio { get; set; }
    public int Contrast { get; set; }
    public US2066 Controller { get; }
    public int CursorLeft { get; set; }
    public int CursorTop { get; set; }
    protected abstract DisplayDotFormat DisplayDotFormat { get; }
    protected abstract DisplayLineNumber DisplayLineNumber { get; }
    public bool DisplayOn { get; set; }
    public FadeOutInterval FadeOutInterval { get; set; }
    public FadeOutMode FadeOutMode { get; set; }
    public InternalOscillatorFrequency InternalOscillatorFrequency { get; set; }
    bool ICharacterLcd.BacklightOn { get; set; }
    Size ICharacterLcd.Size { get; }
    public bool IsBusy { get; }
    public abstract int NumberOfCharsPerLine { get; }
    public int NumberOfCustomCharactersSupported { get; }
    public int NumberOfLines { get; }
    public int PartID { get; }
    public bool UnderlineCursorVisible { get; set; }

    public void Clear() {}
    public char CreateCustomCharacter(CGRamCharacter character, ReadOnlySpan<byte> characterData) {}
    public char CreateCustomCharacter(CGRamCharacter character, Rune characterCodePoint, ReadOnlySpan<byte> characterData) {}
    public char CreateCustomCharacter(CGRamCharacter character, char characterCodePoint, ReadOnlySpan<byte> characterData) {}
    public char CreateCustomCharacter(CGRamCharacter character, int characterCodePoint, ReadOnlySpan<byte> characterData) {}
    public char CreateCustomCharacter(CGRamCharacter character, string characterCodePointString, ReadOnlySpan<byte> characterData) {}
    public void CreateCustomCharacter(int location, ReadOnlySpan<byte> characterMap) {}
    protected virtual void Dispose(bool disposing) {}
    public void Dispose() {}
    public (int Left, int Top) GetCursorPosition() {}
    public void Home() {}
    public void ResetFadeOutStep() {}
    public void SetCursorPosition((int Left, int Top) position) {}
    public void SetCursorPosition(int left, int top) {}
    public void Write(CGRamCharacter character) {}
    public void Write(ReadOnlySpan<byte> text) {}
    public void Write(ReadOnlySpan<char> text) {}
    public void Write(byte character) {}
    public void Write(string text) {}
    public void WriteLine() {}
    public void WriteLine(CGRamCharacter character) {}
    public void WriteLine(ReadOnlySpan<byte> text) {}
    public void WriteLine(ReadOnlySpan<char> text) {}
    public void WriteLine(byte character) {}
    public void WriteLine(string text) {}
  }
}

