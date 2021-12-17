// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Devices.US2066;

public class CharacterGeneratorRomBRussianEncoding : CharacterGeneratorRomBEncoding {
  public override string EncodingName => "US2066 CGROM-B Russian";

  public CharacterGeneratorRomBRussianEncoding(
    string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString
  )
    : base(encoderFallback: new CharacterGeneratorRomBRussianEncoderFallback(defaultReplacementString))
  {
  }
}
