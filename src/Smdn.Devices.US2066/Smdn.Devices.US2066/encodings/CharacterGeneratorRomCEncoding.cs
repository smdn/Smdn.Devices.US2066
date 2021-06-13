// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  public class CharacterGeneratorRomCEncoding : CharacterGeneratorEncoding {
    public override string EncodingName => "US2066 CGROM-C";

    public CharacterGeneratorRomCEncoding(
      string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString,
      bool enableCollation = true
    )

      : base(defaultReplacementString: defaultReplacementString, enableCollation: enableCollation)
    {
    }

    public CharacterGeneratorRomCEncoding(EncoderFallback encoderFallback)
      : base(encoderFallback: encoderFallback)
    {
    }
  }
}
