// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using NUnit.Framework;

namespace Smdn.Devices.US2066;

[TestFixture]
public class CGRomCJapaneseEncodingTests {
  [Test]
  public void GetByteCount_Hiragana()
  {
    Assert.AreEqual(
      5,
      CharacterGeneratorEncoding.CGRomCJapanese.GetByteCount("かがぱ")
    );
  }

  [Test]
  public void GetBytes_Hiragana()
  {
    Assert.AreEqual(
      new byte[] {0xB6, 0xB6, 0xDE, 0xCA, 0xDF},
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("かがぱ")
    );
  }

  [Test]
  public void GetByteCount_FullWidthKatakana()
  {
    Assert.AreEqual(
      5,
      CharacterGeneratorEncoding.CGRomCJapanese.GetByteCount("カガパ")
    );
  }

  [Test]
  public void GetBytes_FullWidthKatakana()
  {
    Assert.AreEqual(
      new byte[] {0xB6, 0xB6, 0xDE, 0xCA, 0xDF},
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("カガパ")
    );
  }

  [Test]
  public void GetBytes_KatakanaPhoneticExtensions()
  {
    Assert.AreEqual(
      new byte[] {0x20, 0x20, 0x20},
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("ㇰㇵㇻ")
    );
  }

  [Test]
  public void GetBytes_DefaultReplacementString()
  {
    var e = new CharacterGeneratorRomCJapaneseEncoding(defaultReplacementString: "!");

    Assert.AreEqual(
      new byte[] {0xB1, 0xB1, 0xB1, 0x21, 0x21},
      e.GetBytes("ｱアあ⭐😫")
    );
  }
}
