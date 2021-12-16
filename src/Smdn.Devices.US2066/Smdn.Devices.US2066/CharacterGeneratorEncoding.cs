// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smdn.Devices.US2066;

public abstract class CharacterGeneratorEncoding : Encoding {
  public static readonly CharacterGeneratorEncoding CGRomA = new CharacterGeneratorRomAEncoding();
  public static readonly CharacterGeneratorEncoding CGRomB = new CharacterGeneratorRomBEncoding();
  public static readonly CharacterGeneratorEncoding CGRomC = new CharacterGeneratorRomCEncoding();

  // ROM-X encodings with extra collation
  public static readonly CharacterGeneratorEncoding CGRomBRussian = new CharacterGeneratorRomBRussianEncoding();
  public static readonly CharacterGeneratorEncoding CGRomCJapanese = new CharacterGeneratorRomCJapaneseEncoding();

  public static bool IsUnmappedCharacter(char ch) => CGRomCharacters.IsUnmapped(ch);
  public static bool IsUndefinedCharacter(char ch) => CGRomCharacters.IsUndefined(ch);

  internal CGRom CGRom { get; }
  private ICGRam cgram = null;

  private readonly (char ch, byte by)[] characterMap;

  private protected CharacterGeneratorEncoding(
    string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString,
    bool enableCollation = true
  )
    : this(encoderFallback: new CharacterGeneratorEncoderFallback(defaultReplacementString: defaultReplacementString, enableCollation: enableCollation))
  {
  }

  private protected CharacterGeneratorEncoding(EncoderFallback encoderFallback)
    : base(
      codePage: 0,
      encoderFallback: encoderFallback ?? CharacterGeneratorEncoderFallback.Default,
      decoderFallback: null
    )
  {
    (this.CGRom, this.characterMap) = this switch {
      CharacterGeneratorRomAEncoding => (CGRom.A, CGRomCharacters.CharacterMapRomA),
      CharacterGeneratorRomBEncoding => (CGRom.B, CGRomCharacters.CharacterMapRomB),
      CharacterGeneratorRomCEncoding => (CGRom.C, CGRomCharacters.CharacterMapRomC),
      _ => throw new NotSupportedException("character map not defined"),
    };
  }

  public override Encoding Clone()
    => Clone(cgram: null);

  internal CharacterGeneratorEncoding Clone(ICGRam cgram)
  {
    var clone = (CharacterGeneratorEncoding)base.Clone();

    clone.cgram = cgram;

    return clone;
  }

  internal void DetachCGRam()
    => cgram = null;

  /*
   * byte[] -> char[]: NotSupportedException
   */
  public override int GetMaxCharCount(int byteCount)
    => throw new NotSupportedException();

  public override int GetCharCount(byte[] bytes, int index, int count)
    => throw new NotSupportedException();

  public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    => throw new NotSupportedException();

  public IEnumerable<Rune> GetRunesForByte(byte byt)
  {
    var mapEntry = characterMap.First(entry => entry.by == byt);

    yield return new Rune(mapEntry.ch);

    foreach (var collation in CGRomCharacters.CollationMap.Where(entry => entry.to[0] == mapEntry.ch)) {
      yield return new Rune(collation.from);
    }
  }

  /*
   * char[] -> byte[]
   */
  public override int GetMaxByteCount(int charCount)
    => charCount < 0 ? throw new ArgumentOutOfRangeException(nameof(charCount), charCount, "must be zero or positive") : charCount;

  public override int GetByteCount(char[] chars, int index, int count)
    => GetByteCount((chars ?? throw new ArgumentNullException(nameof(chars))).AsSpan(index, count));

  public override int GetByteCount(string s)
    => GetByteCount((s ?? throw new ArgumentNullException(nameof(s))).AsSpan());

  public override int GetByteCount(ReadOnlySpan<char> chars)
  {
    EncoderFallbackBuffer fallbackBuffer = null;
    var indexOfChars = 0;
    var byteCount = 0;

    for (; ; ) {
      if (chars.IsEmpty)
        return byteCount;

      var lengthOfCountedChars = 0;
      var (rune, isRune) =
        2 <= chars.Length && Char.IsSurrogatePair(chars[0], chars[1])
          ? (rune: new Rune(chars[0], chars[1]), isRune: true)
          : char.IsSurrogate(chars[0])
            ? (rune: default(Rune), isRune: false)
            : (rune: new Rune(chars[0]), isRune: true);

      // 1: CGRAM
      if (cgram is not null && isRune && cgram.GetByte(rune, out var _)) {
        lengthOfCountedChars = rune.Utf16SequenceLength;
        byteCount++;
        goto COUNTED;
      }

      // 2: CGROM
      if (
        isRune &&
        rune.IsBmp &&
        0 <= Array.BinarySearch<(char, byte)>(characterMap, ((char)rune.Value, default(byte)), CGRomCharacters.CharacterMapEntryComparer)
      ) {
        lengthOfCountedChars = 1; // rune.Utf16SequenceLength = 1
        byteCount++;
        goto COUNTED;
      }

      // 3: fallback
      if (fallbackBuffer is null)
        fallbackBuffer = EncoderFallback.CreateFallbackBuffer();
      else
        fallbackBuffer.Reset();

      if (isRune && fallbackBuffer.Fallback(rune, indexOfChars)) {
        var fallbackBufferLength = fallbackBuffer.Remaining;
        var fallbackChars = ArrayPool<char>.Shared.Rent(fallbackBufferLength);

        try {
          for (var f = 0; f < fallbackBufferLength; f++) {
            fallbackChars[f] = fallbackBuffer.GetNextChar();
          }

          byteCount += GetByteCount(fallbackChars.AsSpan(0, fallbackBufferLength));
          lengthOfCountedChars = rune.Utf16SequenceLength;
        }
        finally {
          ArrayPool<char>.Shared.Return(fallbackChars);
        }
      }
      else {
        lengthOfCountedChars = 1;
        byteCount += 1; // 0x20 SPACE
      }

    COUNTED:
      chars = chars.Slice(lengthOfCountedChars);
      indexOfChars += lengthOfCountedChars;
    }
  }

  public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    => GetBytes(
      (chars ?? throw new ArgumentNullException(nameof(chars))).AsSpan(charIndex, charCount),
      (bytes ?? throw new ArgumentNullException(nameof(bytes))).AsSpan(byteIndex)
    );

  public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
  {
    EncoderFallbackBuffer fallbackBuffer = null;
    var indexOfChars = 0;
    var byteCount = 0;

    while (true) {
      if (chars.IsEmpty)
        return byteCount;
      if (bytes.IsEmpty)
        return byteCount;

      var lengthOfConvertedChars = 0;
      var lengthOfConvertedBytes = 0;
      var (rune, isRune) =
        2 <= chars.Length && Char.IsSurrogatePair(chars[0], chars[1])
          ? (rune: new Rune(chars[0], chars[1]), isRune: true)
          : char.IsSurrogate(chars[0])
            ? (rune: default(Rune), isRune: false)
            : (rune: new Rune(chars[0]), isRune: true);

      // 1: CGRAM
      if (cgram is not null && isRune && cgram.GetByte(rune, out var cgramChararacterByte)) {
        lengthOfConvertedChars = rune.Utf16SequenceLength;
        lengthOfConvertedBytes = 1;
        bytes[0] = cgramChararacterByte;
        goto CONVERTED;
      }

      // 2: CGROM
      if (isRune && rune.IsBmp) {
        var indexOfCharacterMap = Array.BinarySearch<(char, byte)>(characterMap, ((char)rune.Value, default(byte)), CGRomCharacters.CharacterMapEntryComparer);

        if (0 <= indexOfCharacterMap) {
          lengthOfConvertedChars = 1; // rune.Utf16SequenceLength = 1
          lengthOfConvertedBytes = 1;
          bytes[0] = characterMap[indexOfCharacterMap].by;
          goto CONVERTED;
        }
      }

      // 3: fallback
      if (fallbackBuffer is null)
        fallbackBuffer = EncoderFallback.CreateFallbackBuffer();
      else
        fallbackBuffer.Reset();

      if (isRune && fallbackBuffer.Fallback(rune, indexOfChars)) {
        var fallbackBufferLength = fallbackBuffer.Remaining;
        var fallbackChars = ArrayPool<char>.Shared.Rent(fallbackBufferLength);

        try {
          for (var f = 0; f < fallbackBufferLength; f++) {
            fallbackChars[f] = fallbackBuffer.GetNextChar();
          }

          lengthOfConvertedBytes = GetBytes(fallbackChars.AsSpan(0, fallbackBufferLength), bytes);
          lengthOfConvertedChars = rune.Utf16SequenceLength;
        }
        finally {
          ArrayPool<char>.Shared.Return(fallbackChars);
        }
      }
      else {
        lengthOfConvertedChars = 1;
        lengthOfConvertedBytes = 1;
        bytes[0] = 0x20; // SPACE
      }

    CONVERTED:
      chars = chars.Slice(lengthOfConvertedChars);
      indexOfChars += lengthOfConvertedChars;

      bytes = bytes.Slice(lengthOfConvertedBytes);
      byteCount += lengthOfConvertedBytes;
    }
  }
}
