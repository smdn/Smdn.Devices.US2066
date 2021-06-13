// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Smdn.Devices.US2066;

const string outputBaseDirectory = "../../doc/characters/";
const string characterBitmapBaseDirectory = "../characterbitmaps/";

var pathToCharacterBitmapDirectory = Path.GetRelativePath(outputBaseDirectory, characterBitmapBaseDirectory);

foreach (var cgrom in new[] {
  (name: "CGROM-A", encoding: CharacterGeneratorEncoding.CGRomA, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomA),
  (name: "CGROM-B", encoding: CharacterGeneratorEncoding.CGRomB, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomB),
  (name: "CGROM-C", encoding: CharacterGeneratorEncoding.CGRomC, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomC),
}) {
  var output = $"{outputBaseDirectory}/{cgrom.name}.md";

  Directory.CreateDirectory(Path.GetDirectoryName(output));

  using var writer = new StreamWriter(output, append: false, encoding: new UTF8Encoding(false));

  var assm = cgrom.encoding.GetType().Assembly;

  writer.WriteLine("<!-- DO NOT MODIFY - Automatically generated file -->");
  writer.WriteLine();

  writer.WriteLine($"# {cgrom.name} character table");
  writer.WriteLine($"{assm.GetName().Name} version {assm.GetName().Version} ([{cgrom.encoding.GetType().FullName}](/src/Smdn.Devices.US2066/Smdn.Devices.US2066/encodings/))");
  writer.WriteLine();
  writer.WriteLine("|bitmap|`byte`<br>expression|`char`<br>expression|alternative `char` expression<br>(if collation enabled)|");
  writer.WriteLine("|------|--------------------|--------------------|-------------------------------------------------------|");

  for (var byte_hi = 0x00; byte_hi <= 0x0F; byte_hi++) {
    for (var byte_lo = 0x00; byte_lo <= 0x0F; byte_lo++) {
      var characterByte = (byte)((byte_hi << 4) | byte_lo);

      // bitmap
      var bitmapPath = Path.Combine(
        pathToCharacterBitmapDirectory,
        cgrom.name,
        $"{byte_hi << 4:X2}",
        $"{characterByte:X2}.svg"
      );

      writer.Write($"|![0x{characterByte:X2}]({bitmapPath})");

      // byte expression
      writer.Write($"|`0x{characterByte:X2}`");

      // char expression
      var runes = cgrom.encoding.GetRunesForByte(characterByte).ToList();
      var primaryRune = runes[0];

      var isUndefined = CharacterGeneratorEncoding.IsUndefinedCharacter((char)primaryRune.Value);
      var isUnmapped = CharacterGeneratorEncoding.IsUnmappedCharacter((char)primaryRune.Value);

      if (isUndefined || isUnmapped) {
        writer.Write($"|(`U+{primaryRune.Value:X4}`)");

        if (isUnmapped)
          writer.Write("<br><span style=\"font-size: smaller\">⚠have not mapped to certain character</span>");
      }
      else {
        var charExpression = (char)primaryRune.Value switch{
          '|' or '`' => $"&#x{primaryRune.Value:X};",
          _ => primaryRune.ToString()
        };

        writer.Write($"|`{charExpression}` (`U+{primaryRune.Value:X4}`)");
      }

      // alternative char expression
      writer.Write("|");
      writer.Write(
        string.Join(
          "<br>",
          runes.Skip(1).Select(rune => $"`{rune.ToString()}` (`U+{rune.Value:X4}`)")
        )
      );

      writer.WriteLine("|");
    }
  }

  Console.WriteLine($"generated '{output}'");
}