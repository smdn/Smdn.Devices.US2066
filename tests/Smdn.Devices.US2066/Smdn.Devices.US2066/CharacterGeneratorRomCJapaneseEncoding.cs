// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using NUnit.Framework;

namespace Smdn.Devices.US2066;

[TestFixture]
public class CGRomCJapaneseEncodingTests {
  [Test]
  public void GetByteCount_Hiragana()
  {
    Assert.That(
      CharacterGeneratorEncoding.CGRomCJapanese.GetByteCount("かがぱ"),
      Is.EqualTo(5)
    );
  }

  [Test]
  public void GetBytes_Hiragana()
  {
    Assert.That(
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("かがぱ"),
      Is.EqualTo(new byte[] { 0xB6, 0xB6, 0xDE, 0xCA, 0xDF }).AsCollection
    );
  }

  [Test]
  public void GetByteCount_FullWidthKatakana()
  {
    Assert.That(
      CharacterGeneratorEncoding.CGRomCJapanese.GetByteCount("カガパ"),
      Is.EqualTo(5)
    );
  }

  [Test]
  public void GetBytes_FullWidthKatakana()
  {
    Assert.That(
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("カガパ"),
      Is.EqualTo(new byte[] { 0xB6, 0xB6, 0xDE, 0xCA, 0xDF }).AsCollection
    );
  }

  [Test]
  public void GetBytes_KatakanaPhoneticExtensions()
  {
    Assert.That(
      CharacterGeneratorEncoding.CGRomCJapanese.GetBytes("ㇰㇵㇻ"),
      Is.EqualTo(new byte[] { 0x20, 0x20, 0x20 }).AsCollection
    );
  }

  [Test]
  public void GetBytes_DefaultReplacementString()
  {
    var e = new CharacterGeneratorRomCJapaneseEncoding(defaultReplacementString: "!");

    Assert.That(
      e.GetBytes("ｱアあ⭐😫"),
      Is.EqualTo(new byte[] { 0xB1, 0xB1, 0xB1, 0x21, 0x21 }).AsCollection
    );
  }
}
