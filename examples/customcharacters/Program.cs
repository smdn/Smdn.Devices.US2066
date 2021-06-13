// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Text;
using System.Threading;

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

// use 6 custom characters (max. 8 characters can be defined)
display.CGRamUsage = CGRamUsage.UserDefined6Characters;

// create custom character #0 (0x00) and assign its `char` value (code point) as '⭐'(U+2B50)
display.CreateCustomCharacter(
  CGRamCharacter.Character0,
  '⭐',
  new byte[8] {
    0b_00000,
    0b_00100,
    0b_00100,
    0b_11111,
    0b_01110,
    0b_01010,
    0b_10001,
    0b_00000,
  }
);

display.Clear();

// write custom character #0 by specifying assigned `char` value
display.Write("Star:⭐");

// write custom character #0 by specifying CGRamCharacter enum value
display.Write(CGRamCharacter.Character0);

// also code point U+E660~E+E667 is asigned for each custom characters by default
display.Write("\uE660");

// or byte code 0x00~0x07 can be used
display.Write((byte)0x00);

display.WriteLine();



display.CreateCustomCharacter(
  CGRamCharacter.Character1,
  "🙂", // a surrogate pair (including emojis) can be used
  new byte[8] {
    0b_00000,
    0b_01010,
    0b_01010,
    0b_00000,
    0b_10001,
    0b_01110,
    0b_00000,
    0b_00000,
  }
);
display.CreateCustomCharacter(
  CGRamCharacter.Character2,
  "🙁",
  new byte[8] {
    0b_00000,
    0b_01010,
    0b_01010,
    0b_00000,
    0b_01110,
    0b_10001,
    0b_00000,
    0b_00000,
  }
);
display.CreateCustomCharacter(
  CGRamCharacter.Character3,
  "🔋", // U+1F50B 'BATTERY'
  new byte[8] {
    0b_01110,
    0b_11111,
    0b_11111,
    0b_11111,
    0b_11111,
    0b_11111,
    0b_11111,
    0b_00000,
  }
);
display.CreateCustomCharacter(
  CGRamCharacter.Character4,
  new Rune(0x1FAAB), // U+1FAAB 'LOW BATTERY' (Unicode 14.0)
  new byte[8] {
    0b_01110,
    0b_11111,
    0b_10001,
    0b_10001,
    0b_10001,
    0b_10001,
    0b_11111,
    0b_00000,
  }
);

display.Write("🙂🙁\uD83D\uDD0B\uD83E\uDEAB");


// animate random character
var rand = new Random();
var characterData = new byte[8];

display.Write("\uE665");

while (true) {
  rand.NextBytes(characterData);

  display.CreateCustomCharacter(
    CGRamCharacter.Character5,
    characterData
  );

  Thread.Sleep(50);
}