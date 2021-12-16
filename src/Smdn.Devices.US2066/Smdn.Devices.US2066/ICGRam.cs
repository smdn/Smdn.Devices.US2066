// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;

namespace Smdn.Devices.US2066;

internal interface ICGRam {
  bool GetByte(Rune codePoint, out byte by);
}
