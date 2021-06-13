// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  public class CharacterGeneratorEncoderFallback : EncoderFallback {
    public const string DefaultReplacementString = " "; // 0x20 SPACE
    public static readonly CharacterGeneratorEncoderFallback Default = new();

    public string ReplacementString { get; }
    internal bool EnableCollation { get; }

    public override int MaxCharCount => ReplacementString.Length;

    public CharacterGeneratorEncoderFallback(
      string defaultReplacementString = DefaultReplacementString,
      bool enableCollation = true
    )
    {
      // TODO: reject surrogates
      this.ReplacementString = defaultReplacementString ?? throw new ArgumentNullException(nameof(defaultReplacementString));
      this.EnableCollation = enableCollation;
    }

    public override EncoderFallbackBuffer CreateFallbackBuffer()
      => new CharacterGeneratorEncoderFallbackBuffer(this);
  }
}
