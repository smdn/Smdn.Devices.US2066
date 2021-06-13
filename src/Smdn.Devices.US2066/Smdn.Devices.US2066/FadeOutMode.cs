// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Devices.US2066 {
  public enum FadeOutMode : byte {
    Disabled = 0b00,
    FadeOut = 0b10,
    Blinking = 0b11,
  }
}