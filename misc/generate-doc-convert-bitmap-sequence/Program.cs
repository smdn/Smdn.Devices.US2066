// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Smdn.Devices.US2066;

const string pathToCharacterBitmapDirectory = "./misc/characterbitmaps/";

Console.Write("CGROM (a, b, c, b-ru, c-ja)? ");
var cgromString = Console.ReadLine();

var (cgromName, cgrom) = cgromString.ToLowerInvariant() switch {
  "a"     => ("CGROM-A", CharacterGeneratorEncoding.CGRomA),
  "b"     => ("CGROM-B", CharacterGeneratorEncoding.CGRomB),
  "b-ru"  => ("CGROM-B", CharacterGeneratorEncoding.CGRomBRussian),
  "c"     => ("CGROM-C", CharacterGeneratorEncoding.CGRomC),
  "c-ja"  => ("CGROM-C", CharacterGeneratorEncoding.CGRomCJapanese),
  _ => throw new InvalidOperationException($"invalid CGROM"),
};

Console.Write("string? ");
var inputString = Console.ReadLine();

var charIndex = 0;
foreach (var by in cgrom.GetBytes(inputString)) {
  Console.Write($"![{inputString[charIndex]}]({pathToCharacterBitmapDirectory}/{cgromName}/{by & 0xF0:X2}/{by:X2}.svg)");
  charIndex++;
}

Console.WriteLine();