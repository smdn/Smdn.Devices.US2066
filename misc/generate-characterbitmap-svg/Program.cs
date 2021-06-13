// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

using Smdn.Devices.US2066;

const string outputBaseDirectory = "../characterbitmaps/";

var nsSvg = (XNamespace)"http://www.w3.org/2000/svg";

foreach (var cgrom in new[] {
  (name: "CGROM-A", encoding: CharacterGeneratorEncoding.CGRomA, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomA),
  (name: "CGROM-B", encoding: CharacterGeneratorEncoding.CGRomB, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomB),
  (name: "CGROM-C", encoding: CharacterGeneratorEncoding.CGRomC, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomC),
}) {
  for (var byte_hi = 0x00; byte_hi <= 0x0F; byte_hi++) {
    for (var byte_lo = 0x00; byte_lo <= 0x0F; byte_lo++) {
      var characterByte = (byte)((byte_hi << 4) | byte_lo);

      const int characterBitmapDotWidth = 8;
      const int characterBitmapDotHeight = 8;
      const int characterBitmapDotPadding = 1;
      const int characterBitmapMargin = 2;
      const int characterBitmapWidth  = characterBitmapMargin * 2 + characterBitmapDotWidth  * 5 + characterBitmapDotPadding * (5 - 1);
      const int characterBitmapHeight = characterBitmapMargin * 2 + characterBitmapDotHeight * 8 + characterBitmapDotPadding * (8 - 1);
      const float svgWidthInEm = 0.9f;

      var rectCharacterBitmapBackground = new XElement(
        nsSvg + "rect",
        new XAttribute("x", 0),
        new XAttribute("y", 0),
        new XAttribute("width", characterBitmapWidth),
        new XAttribute("height", characterBitmapHeight),
        new XAttribute("fill", "black")
      );
      var svgCharacterBitmap = new XElement(
        nsSvg + "svg",
        new XAttribute("version", "1.2"),
        new XAttribute("class", $"cgromcharacter-bitmap cgromcharacter-bitmap-{characterByte:x2}"),
        new XAttribute("width", $"{svgWidthInEm}em"),
        new XAttribute("height", $"{svgWidthInEm * characterBitmapHeight / characterBitmapWidth}em"),
        new XAttribute("viewBox", $"0 0 {characterBitmapWidth} {characterBitmapHeight}"),
        rectCharacterBitmapBackground
      );
      var doc = new XDocument(
        new XDeclaration("1.0", "utf-8", "yes"),
        new XComment(
@"
SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
SPDX-License-Identifier: MIT
"
        ),
        svgCharacterBitmap
      );

      var characterBitmap = cgrom.getBitmap(characterByte);

      for (var dotY = 0; dotY < 8; dotY++) {
        var groupBitmapLine = new XElement(nsSvg + "g");
        var dotPositionY = characterBitmapMargin + (characterBitmapDotHeight + characterBitmapDotPadding) * dotY;

        svgCharacterBitmap.Add(groupBitmapLine);

        for (var dotX = 4; 0 <= dotX; dotX--) {
          var dotPositionX = characterBitmapMargin + (characterBitmapDotWidth  + characterBitmapDotPadding) * ((5 - 1) - dotX);
          var dot = (characterBitmap[dotY] & (0b00001 << dotX)) != 0;

          groupBitmapLine.Add(
            new XElement(
              nsSvg + "rect",
              new XAttribute("x", dotPositionX),
              new XAttribute("y", dotPositionY),
              new XAttribute("width", characterBitmapDotWidth),
              new XAttribute("height", characterBitmapDotHeight),
              new XAttribute("fill", dot ? "white" : "transparent"),
              new XAttribute("stroke", "white"),
              new XAttribute("stroke-width", "0.25")
            )
          );
        }

        groupBitmapLine.Add(
          new XElement(
            nsSvg + "text",
            new XAttribute("fill", "transparent"),
            new XText($"0b{Convert.ToString(characterBitmap[dotY], 2).PadLeft(5, '0')}")
          )
        );
      }

      var output = $"{outputBaseDirectory}/{cgrom.name}/{byte_hi << 4:X2}/{characterByte:X2}.svg";

      Directory.CreateDirectory(Path.GetDirectoryName(output));

      doc.Save(output);

      Console.WriteLine($"generated '{output}'");
    }
  }
}