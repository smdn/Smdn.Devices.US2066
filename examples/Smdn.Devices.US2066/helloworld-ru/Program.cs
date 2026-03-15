// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

// CGRomBRussian supports cyrillic small letters and some extension letters.
// The small letters will be displayed as capitalized characters.
display.CharacterGenerator = CharacterGeneratorEncoding.CGRomBRussian;

display.WriteLine("Привет, мир!");
display.WriteLine("Здравствуйте!");
