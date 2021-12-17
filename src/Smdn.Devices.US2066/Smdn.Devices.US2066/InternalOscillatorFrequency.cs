// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CA1069 // The enum member has the same constant value

namespace Smdn.Devices.US2066;

public enum InternalOscillatorFrequency : byte {
  Default     = 0b0111,

  Frequency1  = 0b0000,
  Frequency2  = 0b0001,
  Frequency3  = 0b0010,
  Frequency4  = 0b0011,
  Frequency5  = 0b0100,
  Frequency6  = 0b0101,
  Frequency7  = 0b0110,
  Frequency8  = 0b0111,
  Frequency9  = 0b1000,
  Frequency10 = 0b1001,
  Frequency11 = 0b1010,
  Frequency12 = 0b1011,
  Frequency13 = 0b1100,
  Frequency14 = 0b1101,
  Frequency15 = 0b1110,
  Frequency16 = 0b1111,
}
