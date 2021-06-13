// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  public class CharacterGeneratorRomCJapaneseEncoding : CharacterGeneratorRomCEncoding {
    public override string EncodingName => "US2066 CGROM-C Japanese";

    public CharacterGeneratorRomCJapaneseEncoding(
      string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString
    )
      : base(encoderFallback: new CharacterGeneratorRomCJapaneseEncoderFallback(defaultReplacementString))
    {
    }
  }
}
