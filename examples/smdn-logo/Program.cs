// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Smdn.Devices.MCP2221;
using Smdn.Devices.MCP2221.GpioAdapter;
using Smdn.Devices.US2066;

static SO1602A CreateDisplay()
{
  try {
    var mcp2221 = MCP2221.Open();

    return SO1602A.Create(
      new MCP2221I2cDevice(mcp2221.I2C, SO1602A.DefaultI2CAddress) {
        BusSpeed = I2CBusSpeed.FastMode
      }
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

using var display = CreateDisplay();

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
