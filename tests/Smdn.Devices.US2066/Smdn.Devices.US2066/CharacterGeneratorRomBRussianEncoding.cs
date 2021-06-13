// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using NUnit.Framework;

namespace Smdn.Devices.US2066 {
  [TestFixture]
  public class CharacterGeneratorRomBRussianEncodingTests {
    [Test]
    public void GetBytes_CyrillicSmallLetter()
    {
      Assert.AreEqual(
        new byte[] {0x87, 0x84, 0x90, 0x80, 0x82, 0x91, 0x92, 0x82, 0x93, 0x89, 0x92, 0x85, 0x21},
        CharacterGeneratorEncoding.CGRomBRussian.GetBytes("ЗДРАВСТВУЙТЕ!")
      );

      Assert.AreEqual(
        new byte[] {0x87, 0x84, 0x90, 0x80, 0x82, 0x91, 0x92, 0x82, 0x93, 0x89, 0x92, 0x85, 0x21},
        CharacterGeneratorEncoding.CGRomBRussian.GetBytes("Здравствуйте!")
      );
    }

    [Test]
    public void GetBytes_DefaultReplacementString()
    {
      var e = new CharacterGeneratorRomBRussianEncoding(defaultReplacementString: "!");

      Assert.AreEqual(
        new byte[] {0x80, 0x80, 0x21, 0x21},
        e.GetBytes("Аа⭐😫")
      );
    }
  }
}