// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn.Devices.US2066;

public class CharacterGeneratorEncoderFallbackBuffer : EncoderFallbackBuffer {
  private readonly CharacterGeneratorEncoderFallback fallback;
  private string replacement = null;
  private int fallbackOffset = -1;
  private int fallbackRemaining = -1;

  public override int Remaining => 0 <= fallbackRemaining ? fallbackRemaining : 0;

  public CharacterGeneratorEncoderFallbackBuffer(CharacterGeneratorEncoderFallback fallback)
  {
    this.fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
  }

  protected string DefaultReplacementString => fallback.ReplacementString;

  protected virtual string GetReplacement(char charUnknown)
  {
    if (fallback.EnableCollation) {
      var index = Array.BinarySearch(
          CGRomCharacters.CollationMap,
          (charUnknown, default(string)),
          CGRomCharacters.CollationMapEntryComparer
        );

      if (0 <= index)
        return CGRomCharacters.CollationMap[index].to;
    }

    return fallback.ReplacementString;
  }

  protected virtual string GetReplacement(char charUnknownHigh, char charUnknownLow)
    => fallback.ReplacementString;

  public override bool Fallback(char charUnknown, int index)
  {
    replacement = GetReplacement(charUnknown);

    if (replacement is null) {
      return false;
    }
    else {
      fallbackRemaining = replacement.Length;
      return true;
    }
  }

  public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
  {
    replacement = GetReplacement(charUnknownHigh, charUnknownLow);

    if (replacement is null) {
      return false;
    }
    else {
      fallbackRemaining = replacement.Length;
      return true;
    }
  }

  public override char GetNextChar()
  {
    fallbackRemaining--;
    fallbackOffset++;

    if (0 <= fallbackRemaining)
      return replacement[fallbackOffset];
    else
      return char.MinValue;
  }

  public override bool MovePrevious()
    => throw new NotSupportedException();

  public override void Reset()
  {
    replacement = null;
    fallbackOffset = -1;
    fallbackRemaining = -1;
  }
}
