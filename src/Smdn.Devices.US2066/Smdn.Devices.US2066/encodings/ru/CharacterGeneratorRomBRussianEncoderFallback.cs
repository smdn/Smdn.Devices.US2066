// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  public class CharacterGeneratorRomBRussianEncoderFallback : CharacterGeneratorEncoderCollationFallback {
    public CharacterGeneratorRomBRussianEncoderFallback(
      string defaultReplacementString = CharacterGeneratorEncoderFallback.DefaultReplacementString
    )
      : base(defaultReplacementString: defaultReplacementString)
    {
    }

    public override EncoderFallbackBuffer CreateFallbackBuffer()
      => new CharacterGeneratorRomBRussianEncoderFallbackBuffer(this);
  }
}
