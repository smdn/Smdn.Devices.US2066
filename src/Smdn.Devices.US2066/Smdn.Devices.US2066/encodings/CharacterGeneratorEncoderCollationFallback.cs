// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;

namespace Smdn.Devices.US2066;

public abstract class CharacterGeneratorEncoderCollationFallback : CharacterGeneratorEncoderFallback {
  private protected CharacterGeneratorEncoderCollationFallback(
    string defaultReplacementString
  )
    : base(defaultReplacementString: defaultReplacementString, enableCollation: true)
  {
  }

  public abstract override EncoderFallbackBuffer CreateFallbackBuffer();
}
