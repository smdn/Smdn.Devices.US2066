// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Devices.US2066;

[TestFixture]
public class CharacterGeneratorEncodingTests {
  [Test] public void GetChars() => Assert.Throws<NotSupportedException>(() => CharacterGeneratorEncoding.CGRomA.GetChars(Array.Empty<byte>()));
  [Test] public void GetCharCount() => Assert.Throws<NotSupportedException>(() => CharacterGeneratorEncoding.CGRomA.GetCharCount(Array.Empty<byte>()));
  [Test] public void GetMaxCharCount() => Assert.Throws<NotSupportedException>(() => CharacterGeneratorEncoding.CGRomA.GetMaxCharCount(0));

  [Test]
  public void GetBytes_StringNull()
    => Assert.Throws<ArgumentNullException>(() => CharacterGeneratorEncoding.CGRomA.GetBytes(chars: null));

  [Test]
  public void GetByteCount_ASCII()
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        26,
        e.GetByteCount("ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetBytes_ASCII()
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        new byte[] {0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A},
        e.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
        e.EncodingName
      );
    }
  }

  [TestCase("😊", 1)]
  [TestCase("😘", 1)]
  [TestCase("😊😘", 2)]
  public void GetByteCount_SurrogatePair(string input, int expectedByteCount)
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        expectedByteCount,
        e.GetByteCount(input),
        e.EncodingName
      );
    }
  }

  [TestCase("😊", "20")]
  [TestCase("😘", "20")]
  [TestCase("😊😘", "20-20")]
  public void GetBytes_SurrogatePair(string input, string expectedByteSequence)
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        Array.ConvertAll(expectedByteSequence.Split('-'), by => Convert.ToByte(by, 16)),
        e.GetBytes(input),
        e.EncodingName
      );
    }
  }

  [TestCase("≤")]
  [TestCase("≦")]
  [TestCase("⩽")]
  [TestCase("≥")]
  [TestCase("≧")]
  [TestCase("⩾")]
  public void GetByteCount_Collation(string input)
  {
    Assert.AreEqual(
      1,
      CharacterGeneratorEncoding.CGRomC.GetByteCount(input)
    );
  }

  [TestCase("≥", 0xF9)]
  [TestCase("≧", 0xF9)]
  [TestCase("⩾", 0xF9)]
  [TestCase("≤", 0xFA)]
  [TestCase("≦", 0xFA)]
  [TestCase("⩽", 0xFA)]
  public void GetBytes_Collation(string input, byte expectedCharacter)
  {
    Assert.AreEqual(
      new byte[] {expectedCharacter},
      CharacterGeneratorEncoding.CGRomC.GetBytes(input)
    );
  }

  [TestCase("！", true, 0x21)]
  [TestCase("／", true, 0x2F)]
  [TestCase("Ａ", true, 0x41)]
  [TestCase("ａ", true, 0x61)]
  [TestCase("！", false, 0x20)]
  [TestCase("／", false, 0x20)]
  [TestCase("Ａ", false, 0x20)]
  [TestCase("ａ", false, 0x20)]
  public void GetBytes_Collation_FullWidthToHalfWidth(string input, bool enableCollation, byte expectedCharacter)
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(enableCollation: enableCollation),
      new CharacterGeneratorRomBEncoding(enableCollation: enableCollation),
      new CharacterGeneratorRomCEncoding(enableCollation: enableCollation),
    }) {
      Assert.AreEqual(
        new byte[] {expectedCharacter},
        e.GetBytes(input),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetByteCount_DefaultFallbackByte()
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        7,
        e.GetByteCount("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetBytes_DefaultFallbackByte()
  {
    foreach (var e in new[] {
      CharacterGeneratorEncoding.CGRomA,
      CharacterGeneratorEncoding.CGRomB,
      CharacterGeneratorEncoding.CGRomC
    }) {
      Assert.AreEqual(
        new byte[] { 0x41, 0x42, 0x43, 0x20, 0x20, 0x20, 0x20 },
        e.GetBytes("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetByteCount_DefaultReplacementString()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(defaultReplacementString: "!"),
      new CharacterGeneratorRomBEncoding(defaultReplacementString: "!"),
      new CharacterGeneratorRomCEncoding(defaultReplacementString: "!"),
    }) {
      Assert.AreEqual(
        7,
        e.GetByteCount("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetBytes_DefaultReplacementString()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(defaultReplacementString: "!"),
      new CharacterGeneratorRomBEncoding(defaultReplacementString: "!"),
      new CharacterGeneratorRomCEncoding(defaultReplacementString: "!"),
    }) {
      Assert.AreEqual(
        new byte[] { 0x41, 0x42, 0x43, 0x21, 0x21, 0x21, 0x21},
        e.GetBytes("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetByteCount_DefaultReplacementString_Empty()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(defaultReplacementString: string.Empty),
      new CharacterGeneratorRomBEncoding(defaultReplacementString: string.Empty),
      new CharacterGeneratorRomCEncoding(defaultReplacementString: string.Empty),
    }) {
      Assert.AreEqual(
        3,
        e.GetByteCount("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetBytes_DefaultReplacementString_Empty()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(defaultReplacementString: string.Empty),
      new CharacterGeneratorRomBEncoding(defaultReplacementString: string.Empty),
      new CharacterGeneratorRomCEncoding(defaultReplacementString: string.Empty),
    }) {
      Assert.AreEqual(
        new byte[] { 0x41, 0x42, 0x43},
        e.GetBytes("ABC日本語😫"),
        e.EncodingName
      );
    }
  }



  [Test]
  public void GetByteCount_EncoderExceptionFallback()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(new EncoderExceptionFallback()),
      new CharacterGeneratorRomBEncoding(new EncoderExceptionFallback()),
      new CharacterGeneratorRomCEncoding(new EncoderExceptionFallback()),
    }) {
      var ex = Assert.Throws<EncoderFallbackException>(
        () => e.GetByteCount("ABC日本語😫")
      );

      Assert.AreEqual('日', ex.CharUnknown);
    }
  }

  [Test]
  public void GetBytes_EncoderExceptionFallback()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(new EncoderExceptionFallback()),
      new CharacterGeneratorRomBEncoding(new EncoderExceptionFallback()),
      new CharacterGeneratorRomCEncoding(new EncoderExceptionFallback()),
    }) {
      var ex = Assert.Throws<EncoderFallbackException>(
        () => e.GetBytes("ABC日本語😫")
      );

      Assert.AreEqual('日', ex.CharUnknown);
    }
  }

  [Test]
  public void GetByteCount_EncoderReplacementFallback()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(new EncoderReplacementFallback("!")),
      new CharacterGeneratorRomBEncoding(new EncoderReplacementFallback("!")),
      new CharacterGeneratorRomCEncoding(new EncoderReplacementFallback("!")),
    }) {
      Assert.AreEqual(
        8, // EncoderReplacementFallback replaces "😫" -> "!!"
        e.GetByteCount("ABC日本語😫"),
        e.EncodingName
      );
    }
  }

  [Test]
  public void GetBytes_EncoderReplacementFallback()
  {
    foreach (var e in new CharacterGeneratorEncoding[] {
      new CharacterGeneratorRomAEncoding(new EncoderReplacementFallback("!")),
      new CharacterGeneratorRomBEncoding(new EncoderReplacementFallback("!")),
      new CharacterGeneratorRomCEncoding(new EncoderReplacementFallback("!")),
    }) {
      Assert.AreEqual(
        new byte[] { 0x41, 0x42, 0x43, 0x21, 0x21, 0x21, 0x21, 0x21}, // EncoderReplacementFallback replaces "😫" -> "!!"
        e.GetBytes("ABC日本語😫"),
        e.EncodingName
      );
    }
  }
}
