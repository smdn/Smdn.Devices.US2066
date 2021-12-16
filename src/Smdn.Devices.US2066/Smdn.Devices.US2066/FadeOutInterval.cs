// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Devices.US2066;

public enum FadeOutInterval : byte {
  Step8Frames     = 0b0000,
  Step16Frames    = 0b0001,
  Step24Frames    = 0b0010,
  Step32Frames    = 0b0011,

  Step40Frames    = 0b0100,
  Step48Frames    = 0b0101,
  Step56Frames    = 0b0110,
  Step64Frames    = 0b0111,

  Step72Frames    = 0b1000,
  Step80Frames    = 0b1001,
  Step88Frames    = 0b1010,
  Step96Frames    = 0b1011,

  Step104Frames   = 0b1100,
  Step112Frames   = 0b1101,
  Step120Frames   = 0b1110,
  Step128Frames   = 0b1111,

  Min = Step8Frames,
  Max = Step128Frames,
}
