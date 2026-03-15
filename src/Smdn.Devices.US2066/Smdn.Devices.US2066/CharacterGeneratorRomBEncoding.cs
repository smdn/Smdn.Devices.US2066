// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;

namespace Smdn.Devices.US2066;

public class CharacterGeneratorRomBEncoding : CharacterGeneratorEncoding {
  public override string EncodingName => "US2066 CGROM-B";

  public CharacterGeneratorRomBEncoding(
    string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString,
    bool enableCollation = true
  )

    : base(defaultReplacementString: defaultReplacementString, enableCollation: enableCollation)
  {
  }

  public CharacterGeneratorRomBEncoding(EncoderFallback encoderFallback)
    : base(encoderFallback: encoderFallback)
  {
  }
}
