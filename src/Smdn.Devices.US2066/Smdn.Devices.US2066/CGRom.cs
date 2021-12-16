// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Devices.US2066;

internal enum CGRom : byte {
  A       = 0b_0000,
  B       = 0b_0100,
  C       = 0b_1000,
  Invalid = 0b_1100,
}
