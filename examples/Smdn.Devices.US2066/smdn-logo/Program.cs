// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Microsoft.Extensions.DependencyInjection;

using Smdn.Devices.Mcp2221A;
using Smdn.Devices.US2066;
using Smdn.IO.UsbHid.DependencyInjection;

static SO1602A CreateDisplay(IServiceProvider serviceProvider)
{
  try {
    var mcp2221a = Mcp2221A.Create(serviceProvider);

    return SO1602A.Create(
      mcp2221a.I2c.CreateDevice(SO1602A.DefaultI2CAddress, shouldDisposeMcp2221A: true).WithFastMode()
    );
  }
  catch {
    try {
      return SO1602A.Create(SO1602A.DefaultI2CAddress);
    }
    catch {
      throw;
    }
  }
}

var services = new ServiceCollection();

services.AddHidSharpUsbHid();

using var serviceProvider = services.BuildServiceProvider();

using var display = CreateDisplay(serviceProvider);

display.BlinkingCursorVisible = false;
display.UnderlineCursorVisible = false;
display.CGRamUsage = CGRamUsage.UserDefined8Characters;

var char_smdnlogo = display.CreateCustomCharacter(
  CGRamCharacter.Character0,
  new byte[8] {
    0b_00000,
    0b_00000,
    ('s' & 0x1f),
    ('m' & 0x1f),
    ('d' & 0x1f),
    ('n' & 0x1f),
    0b_00000,
    0b_00000,
  }
);

var char_s = display.CreateCustomCharacter(
  CGRamCharacter.Character1,
  new byte[8] {
    0b_00000,
    0b_00111,
    0b_01000,
    0b_00110,
    0b_00001,
    0b_01110,
    0b_00000,
    0b_00000,
  }
);

var char_m = display.CreateCustomCharacter(
  CGRamCharacter.Character2,
  new byte[8] {
    0b_00000,
    0b_11110,
    0b_10101,
    0b_10101,
    0b_10101,
    0b_10101,
    0b_00000,
    0b_00000,
  }
);

var char_d = display.CreateCustomCharacter(
  CGRamCharacter.Character3,
  new byte[8] {
    0b_00001,
    0b_00001,
    0b_01101,
    0b_10011,
    0b_10011,
    0b_01101,
    0b_00000,
    0b_00000,
  }
);

var char_n = display.CreateCustomCharacter(
  CGRamCharacter.Character4,
  new byte[8] {
    0b_00000,
    0b_10110,
    0b_11001,
    0b_10001,
    0b_10001,
    0b_10001,
    0b_00000,
    0b_00000,
  }
);

var char_dot_j = display.CreateCustomCharacter(
  CGRamCharacter.Character5,
  new byte[8] {
    0b_00001,
    0b_00000,
    0b_00001,
    0b_00001,
    0b_00001,
    0b_10001,
    0b_00110,
    0b_00000,
  }
);

var char_p = display.CreateCustomCharacter(
  CGRamCharacter.Character6,
  new byte[8] {
    0b_00000,
    0b_11100,
    0b_10010,
    0b_10010,
    0b_11100,
    0b_10000,
    0b_10000,
    0b_00000,
  }
);

foreach (var line in new[] {
  new[] { char_smdnlogo, char_s, char_m, char_d, char_n },
  new[] { char_s, char_m, char_d, char_n, char_dot_j, char_p },
}) {
  display.CursorLeft = (display.NumberOfCharsPerLine - line.Length) / 2;
  display.WriteLine(line);
}
