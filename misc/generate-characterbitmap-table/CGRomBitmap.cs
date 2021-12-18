// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections.Generic;

namespace Smdn.Devices.US2066;

internal partial class CGRomBitmap {
  public static IReadOnlyList<byte> GetBitmapCGRomA(byte by) => bitmapCGROM_A[by];
  public static IReadOnlyList<byte> GetBitmapCGRomB(byte by) => bitmapCGROM_B[by];
  public static IReadOnlyList<byte> GetBitmapCGRomC(byte by) => bitmapCGROM_C[by];
}
