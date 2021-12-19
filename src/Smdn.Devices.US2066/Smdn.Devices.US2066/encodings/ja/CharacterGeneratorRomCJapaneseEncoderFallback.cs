// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System.Text;

namespace Smdn.Devices.US2066;

public class CharacterGeneratorRomCJapaneseEncoderFallback : CharacterGeneratorEncoderCollationFallback {
  public CharacterGeneratorRomCJapaneseEncoderFallback(
    string defaultReplacementString = DefaultReplacementString
  )
    : base(defaultReplacementString: defaultReplacementString)
  {
  }

  public override EncoderFallbackBuffer CreateFallbackBuffer()
    => new CharacterGeneratorRomCJapaneseEncoderFallbackBuffer(this);
}
