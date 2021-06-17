// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066 {
  internal static class EncoderFallbackExtensions {
    public static bool Fallback(this EncoderFallbackBuffer fallbackBuffer, Rune runeUnknown, int index)
    {
      Span<char> chars = stackalloc char[runeUnknown.Utf16SequenceLength];

      return runeUnknown.EncodeToUtf16(chars) switch {
        1 => fallbackBuffer.Fallback(charUnknown: chars[0], index),
        2 => fallbackBuffer.Fallback(charUnknownHigh: chars[0], charUnknownLow: chars[1], index),
        _ => throw new NotImplementedException("unexpected case"),
      };
    }
  }
}
