// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Linq;
using System.Threading;

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

display.BlinkingCursorVisible = false;
display.UnderlineCursorVisible = false;
display.CharacterGenerator = CharacterGeneratorEncoding.CGRomA;

var contrastValues = Enumerable.Range(0x00, 0x100).Concat(Enumerable.Range(0x00, 0x100).Reverse());

display.Home();
display.Write("contrast=");

var initialCursorPosition = display.GetCursorPosition();

foreach (var contrast in contrastValues) {
  display.Contrast = contrast;

  display.SetCursorPosition(initialCursorPosition);
  display.WriteLine($"0x{contrast:X2}");

  WriteBar(display, contrast, 0x00, 0x100);

  Thread.Sleep(10);
}

static void WriteBar(SOXXXXA display, int value, int min, int max)
{
  Span<char> bar = stackalloc char[display.NumberOfCharsPerLine];

  bar.Fill(' ');

  var totalDots = 5 * bar.Length;
  var dots = totalDots * (value - min) / (max - min);

  bar.Slice(0, dots / 5).Fill('█');

  if (value < max) {
    bar[dots / 5] = ((dots % 5) switch {
      4 => '▉',
      3 => '▋',
      2 => '▍',
      1 => '▏',
      _ => ' ',
    });
  }

  display.WriteLine(bar);
}
