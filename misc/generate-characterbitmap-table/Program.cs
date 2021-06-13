// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Smdn.Devices.US2066;

var nsSvg = (XNamespace)"http://www.w3.org/2000/svg";
var nsXhtml = (XNamespace)"http://www.w3.org/1999/xhtml";

foreach (var cgrom in new[] {
  (name: "CGROM-A", encoding: CharacterGeneratorEncoding.CGRomA, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomA),
  (name: "CGROM-B", encoding: CharacterGeneratorEncoding.CGRomB, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomB),
  (name: "CGROM-C", encoding: CharacterGeneratorEncoding.CGRomC, getBitmap: (Func<byte, IReadOnlyList<byte>>)CGRomBitmap.GetBitmapCGRomC),
}) {
  const string classNamePrefix = "cgromtable-";

  var characterBoxBitmapBound = new Rectangle(
    x: 0,
    y: 0,
    width: /*5 * 10*/ 5 * 24,
    height: /*8 * 10*/ 8 * 24
  );
  var characterBoxCharBound = new Rectangle(
    x: characterBoxBitmapBound.X,
    y: characterBoxBitmapBound.Bottom,
    width: characterBoxBitmapBound.Width,
    height: 20
  );
  var characterBoxAltCharsBound = new Rectangle(
    x: characterBoxCharBound.X,
    y: characterBoxCharBound.Bottom,
    width: characterBoxCharBound.Width,
    height: 160
  );

  /*readonly*/ var characterBoxWidth = characterBoxBitmapBound.Width;
  /*readonly*/ var characterBoxHeight = characterBoxAltCharsBound.Bottom;

  const int leftHeaderWidth = 32;
  const int topHeaderHeight = 32;

  /*readonly*/ var tableWidth = leftHeaderWidth + characterBoxWidth * 16;
  /*readonly*/ var tableHeight = topHeaderHeight + characterBoxHeight * 16;

  var groupTableLines = new XElement(
    nsSvg + "g"
  );
  var groupTableContents = new XElement(
    nsSvg + "g"
  );
  var groupTable = new XElement(
    nsSvg + "g",
    groupTableLines,
    groupTableContents
  );
  var root = new XElement(
    nsSvg + "svg",
    new XAttribute("version", "1.2"),
    new XAttribute("viewBox", $"0 0 {tableWidth} {tableHeight}"),
    groupTable
  );
  var doc = new XDocument(root);

  groupTableLines.Add(
    new XElement(
      nsSvg + "rect",
      new XAttribute("class", classNamePrefix + "table"),
      new XAttribute("fill", "black"),
      new XAttribute("stroke", "white"),
      new XAttribute("stroke-width", "3"),
      new XAttribute("x", 0),
      new XAttribute("y", 0),
      new XAttribute("width", tableWidth),
      new XAttribute("height", tableHeight)
    )
  );
  groupTableLines.Add(
    new XElement(
      nsSvg + "line",
      new XAttribute("class", string.Join(" ", new[] {classNamePrefix + "headerline", classNamePrefix + "headerline-left"})),
      new XAttribute("stroke", "white"),
      new XAttribute("stroke-width", "3"),
      new XAttribute("x1", leftHeaderWidth),
      new XAttribute("y1", 0),
      new XAttribute("x2", leftHeaderWidth),
      new XAttribute("y2", tableHeight)
    )
  );
  groupTableLines.Add(
    new XElement(
      nsSvg + "line",
      new XAttribute("class", string.Join(" ", new[] {classNamePrefix + "headerline", classNamePrefix + "headerline-top"})),
      new XAttribute("stroke", "white"),
      new XAttribute("stroke-width", "3"),
      new XAttribute("x1", 0),
      new XAttribute("y1", topHeaderHeight),
      new XAttribute("x2", tableWidth),
      new XAttribute("y2", topHeaderHeight)
    )
  );

  int CalcCharacterBoxPositionY(int by_lo) => topHeaderHeight + characterBoxHeight * by_lo;
  int CalcCharacterBoxPositionX(int by_hi) => leftHeaderWidth + characterBoxWidth  * by_hi;

  for (var byte_hi = 0x00; byte_hi <= 0x0F; byte_hi++) {
    var y = CalcCharacterBoxPositionY(byte_hi);

    groupTableLines.Add(
      new XElement(
        nsSvg + "line",
        new XAttribute("class", string.Join(" ", new[] {classNamePrefix + "characterboxline", classNamePrefix + $"characterboxline-hi-{byte_hi:X2}"})),
        new XAttribute("stroke", "white"),
        new XAttribute("stroke-width", "1"),
        new XAttribute("x1", 0),
        new XAttribute("y1", y),
        new XAttribute("x2", tableWidth),
        new XAttribute("y2", y)
      )
    );
    // TODO: header title
  }

  for (var byte_lo = 0x00; byte_lo <= 0x0F; byte_lo++) {
    var x = CalcCharacterBoxPositionX(byte_lo);

    groupTableLines.Add(
      new XElement(
        nsSvg + "line",
        new XAttribute("class", string.Join(" ", new[] {classNamePrefix + "characterboxline", classNamePrefix + $"characterboxline-lo-{byte_lo:X2}"})),
        new XAttribute("stroke", "white"),
        new XAttribute("stroke-width", "1"),
        new XAttribute("x1", x),
        new XAttribute("y1", 0),
        new XAttribute("x2", x),
        new XAttribute("y2", tableHeight)
      )
    );
    // TODO: header title
  }

  for (var byte_hi = 0x00; byte_hi <= 0x0F; byte_hi++) {
    var characterBoxPositionX = CalcCharacterBoxPositionX(byte_hi);

    for (var byte_lo = 0x00; byte_lo <= 0x0F; byte_lo++) {
      var characterByte = (byte)((byte_hi << 4) | byte_lo);
      var characterBoxPosition = (x: characterBoxPositionX, y: CalcCharacterBoxPositionY(byte_lo));

      var groupCharacterBox = new XElement(
        nsSvg + "g",
        new XAttribute("class", classNamePrefix + $"character-box-{characterByte:x2}")
      );

      groupTableContents.Add(groupCharacterBox);

      /*
       * character bitmap
       */
      const int characterBitmapDotWidth = 8;
      const int characterBitmapDotHeight = 8;
      const int characterBitmapDotPadding = 1;
      const int characterBitmapMargin = 8;
      const int characterBitmapWidth  = characterBitmapMargin * 2 + characterBitmapDotWidth  * 5 + characterBitmapDotPadding * (5 - 1);
      const int characterBitmapHeight = characterBitmapMargin * 2 + characterBitmapDotHeight * 8 + characterBitmapDotPadding * (8 - 1);

      var characterBitmapContainer = new XElement(
        nsSvg + "svg",
        new XAttribute("version", "1.2"),
        new XAttribute("class", classNamePrefix + $"character-bitmap-{characterByte:x2}"),
        new XAttribute("x", characterBoxPosition.x + characterBoxBitmapBound.X),
        new XAttribute("y", characterBoxPosition.y + characterBoxBitmapBound.Y),
        new XAttribute("width", characterBoxBitmapBound.Width),
        new XAttribute("height", characterBoxBitmapBound.Height),
        new XAttribute("viewBox", $"0 0 {characterBitmapWidth} {characterBitmapHeight}")
      );

      groupCharacterBox.Add(characterBitmapContainer);

      var characterBitmap = cgrom.getBitmap(characterByte);

      for (var dotY = 0; dotY < 8; dotY++) {
        var bitmapLine = new XElement(nsSvg + "g");
        var dotPositionY = characterBitmapMargin + (characterBitmapDotHeight + characterBitmapDotPadding) * dotY;

        characterBitmapContainer.Add(bitmapLine);

        for (var dotX = 0; dotX < 5; dotX++) {
          var dotPositionX = characterBitmapMargin + (characterBitmapDotWidth  + characterBitmapDotPadding) * ((5 - 1) - dotX);
          var dot = (characterBitmap[dotY] & (0b00001 << dotX)) != 0;

          bitmapLine.Add(
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
      }

      var runes = cgrom.encoding.GetRunesForByte(characterByte).ToList();

      var textCharacterChar = new XElement(
        nsSvg + "text",
        new XAttribute("class", classNamePrefix + $"character-char-{characterByte:x2}"),
        new XAttribute("x", characterBoxPosition.x + characterBoxCharBound.X + characterBoxCharBound.Width / 2),
        new XAttribute("y", characterBoxPosition.y + characterBoxCharBound.Y + characterBoxCharBound.Height / 2),
        new XAttribute("width", characterBoxCharBound.Width),
        new XAttribute("height", characterBoxCharBound.Height),
        new XAttribute("fill", "white"),
        new XAttribute("font-size", (int)(characterBoxCharBound.Height * 0.96)),
        new XAttribute("font-weight", "bolder"),
        new XAttribute("dominant-baseline", "middle"),
        new XAttribute("text-anchor", "middle"),
        new XElement(
          nsSvg + "tspan",
          new XText(runes[0].ToString())
        ),
        new XElement(
          nsSvg + "tspan",
          new XText($"(U+{runes[0].Value:X4})")
        )
      );

      groupCharacterBox.Add(textCharacterChar);

      var textCharacterAltChars = new XElement(
        nsSvg + "foreignObject",
        new XAttribute("class", classNamePrefix + $"character-altchars-{characterByte:x2}"),
        new XAttribute("x", characterBoxPosition.x + characterBoxAltCharsBound.X),
        new XAttribute("y", characterBoxPosition.y + characterBoxAltCharsBound.Y),
        new XAttribute("width", characterBoxAltCharsBound.Width),
        new XAttribute("height", characterBoxAltCharsBound.Height),
        new XElement(
          nsXhtml + "div",
          new XAttribute("style", "text-align: center"),
          runes.Skip(1).Select(rune =>
            new XElement(
              nsXhtml + "span",
              new XAttribute("style", "color: white; white-space: nowrap;"),
              new XText($"{rune.ToString()} (U+{rune.Value:X4})"),
              new XText("\n")
            )
          )
        )
      );

      groupCharacterBox.Add(textCharacterAltChars);
    }
  }

  var output = $"table-{cgrom.name}.svg";

  doc.Save(output);

  Console.WriteLine($"generated '{output}'");
}