// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Devices.US2066 {
  public enum ClockDivideRatio : byte {
    Default = 0b0000,

    Ratio1  = 0b0000,
    Ratio2  = 0b0001,
    Ratio3  = 0b0010,
    Ratio4  = 0b0011,
    Ratio5  = 0b0100,
    Ratio6  = 0b0101,
    Ratio7  = 0b0110,
    Ratio8  = 0b0111,
    Ratio9  = 0b1000,
    Ratio10 = 0b1001,
    Ratio11 = 0b1010,
    Ratio12 = 0b1011,
    Ratio13 = 0b1100,
    Ratio14 = 0b1101,
    Ratio15 = 0b1110,
    Ratio16 = 0b1111,
  }
}
