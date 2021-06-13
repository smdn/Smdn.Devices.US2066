// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Globalization;
using System.Threading;

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

display.BlinkingCursorVisible = false;
display.UnderlineCursorVisible = false;

CreateCustomCharacters();

var locale_ja = new CultureInfo("ja");

display.WriteLine(DateTime.Now.ToString("yyyy年MM月dd日(ddd)", locale_ja));

while (true) {
  display.SetCursorPosition(0, 1);
  display.WriteLine(DateTime.Now.ToString("T", locale_ja));

  Thread.Sleep(1000);
}

void CreateCustomCharacters()
{
  display.CGRamUsage = CGRamUsage.UserDefined8Characters;

  display.CreateCustomCharacter(
    CGRamCharacter.Character0,
    '年', // (n-th) year
    new byte[8] {
      0b_01000,
      0b_01111,
      0b_10010,
      0b_01111,
      0b_01010,
      0b_11111,
      0b_00010,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character1,
    '日', // sun(-day) or (n-th) day
    new byte[8] {
      0b_11111,
      0b_10001,
      0b_10001,
      0b_11111,
      0b_10001,
      0b_10001,
      0b_11111,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character2,
    '月', // mon(-day) or (n-th) month
    new byte[8] {
      0b_01111,
      0b_01001,
      0b_01111,
      0b_01001,
      0b_01111,
      0b_01001,
      0b_10001,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character3,
    '火', // tues(-day)
    new byte[8] {
      0b_00100,
      0b_10101,
      0b_10101,
      0b_00100,
      0b_00100,
      0b_01010,
      0b_10001,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character4,
    '水', // wednes(-day)
    new byte[8] {
      0b_00100,
      0b_00101,
      0b_11110,
      0b_01110,
      0b_01101,
      0b_10100,
      0b_00100,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character5,
    '木', // thurs(-day)
    new byte[8] {
      0b_00100,
      0b_00100,
      0b_11111,
      0b_00100,
      0b_01110,
      0b_10101,
      0b_00100,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character6,
    '金', // fri(-day)
    new byte[8] {
      0b_00100,
      0b_01010,
      0b_10001,
      0b_01110,
      0b_10101,
      0b_01110,
      0b_11111,
      0b_00000,
    }
  );

  display.CreateCustomCharacter(
    CGRamCharacter.Character7,
    '土', // satur(-day)
    new byte[8] {
      0b_00100,
      0b_00100,
      0b_01110,
      0b_00100,
      0b_00100,
      0b_00100,
      0b_11111,
      0b_00000,
    }
  );
}