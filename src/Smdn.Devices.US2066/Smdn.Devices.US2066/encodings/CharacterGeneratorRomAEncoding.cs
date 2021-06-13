// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  public class CharacterGeneratorRomAEncoding : CharacterGeneratorEncoding {
    public override string EncodingName => "US2066 CGROM-A";

    public CharacterGeneratorRomAEncoding(
      string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString,
      bool enableCollation = true
    )
      : base(defaultReplacementString: defaultReplacementString, enableCollation: enableCollation)
    {
    }

    public CharacterGeneratorRomAEncoding(EncoderFallback encoderFallback)
      : base(encoderFallback: encoderFallback)
    {
    }
  }
}