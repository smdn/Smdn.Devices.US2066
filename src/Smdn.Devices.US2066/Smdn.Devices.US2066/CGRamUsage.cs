// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Devices.US2066 {
  public enum CGRamUsage : byte {
    /// <summary>represents OPR[1:0] = 00b</summary>
    OPR_00b = 0b_00,

    /// <summary>represents OPR[1:0] = 01b</summary>
    OPR_01b = 0b_01,

    /// <summary>represents OPR[1:0] = 10b</summary>
    OPR_10b = 0b_10,

    /// <summary>represents OPR[1:0] = 11b</summary>
    OPR_11b = 0b_11,

    UserDefined8Characters = OPR_01b,
    UserDefined6Characters = OPR_10b,
    NoUserDefinedCharacters = OPR_11b,
  }
}
