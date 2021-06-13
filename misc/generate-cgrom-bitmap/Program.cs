// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

foreach (var target in new[] {
  (name: "CGROM-A", input: "../cgrom-bitmap/CGROM-A.png", output: "../cgrom-bitmap/CGRomBitmap.A.cs"),
  (name: "CGROM-B", input: "../cgrom-bitmap/CGROM-B.png", output: "../cgrom-bitmap/CGRomBitmap.B.cs"),
  (name: "CGROM-C", input: "../cgrom-bitmap/CGROM-C.png", output: "../cgrom-bitmap/CGRomBitmap.C.cs")
}) {
  Console.Error.WriteLine($"generating {target.name} bitmap from {target.input} to {target.output}");

  using var bitmap = (Bitmap)Image.FromFile(target.input);
  using var output = new StreamWriter(target.output, append: false, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

  output.WriteLine(
$@"// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

/* DO NOT MODIFY - Automatically generated file */
namespace Smdn.Devices.US2066 {{
  partial class CGRomBitmap {{
    private static readonly byte[][] bitmap{target.name.Replace("-", "_")} = {{"
  );
  var indent = new string(' ', 6);

  BitmapData data = null;

  try {
    data = bitmap.LockBits(
      new Rectangle(0, 0, bitmap.Width, bitmap.Height),
      ImageLockMode.ReadOnly,
      PixelFormat.Format32bppArgb
    );

    //         |<-(2)->|
    //
    //      +--+-------+--..
    //      |\ |       |
    //      | \| 0b0000|
    //  -   +-(1)------+--..
    //  ^   |  |       |
    //  |   |  |(3)*** |
    //  |   |0 | ***** |
    //  |   |b | ***** |
    // (2)  |0 | ***** |
    //  |   |0 | ***** |
    //  |   |0 | ***** |
    //  |   |0 | ***** |
    //  |   |  | ***** |
    //  v   |  |       |
    //  -   +--+-------+--..
    //      |  |       |
    //      :  :       :
    var mapOffset = (x: 126, y: 84); // (1): offset from image origin (0, 0)
    var characterSize = (w: 2190 / 16.0, h: 3278 / 16.0); // (2)
    var characterDotsOffset = (x: 18, y: 19); // (3): offset from (1)
    var dotSize = (w: 19, h: 20); // size of each dot
    var dotCenter = (x: dotSize.w / 2, y: dotSize.h / 2);

    for (var char_hi = 0x00; char_hi <= 0x0F; char_hi++) {
      for (var char_lo = 0x00; char_lo <= 0x0F; char_lo++) {
        byte charByte = (byte)(char_hi << 4 | char_lo);

        var characterOffset = (
          x: mapOffset.x + char_hi * characterSize.w,
          y: mapOffset.y + char_lo * characterSize.h
        );
        var characterPosition = (
          x: characterOffset.x + characterDotsOffset.x,
          y: characterOffset.y + characterDotsOffset.y
        );

        output.Write(indent); output.WriteLine($"// 0x{charByte:X2} (0b_{Convert.ToString(char_hi, 2).PadLeft(4, '0')}_{Convert.ToString(char_lo, 2).PadLeft(4, '0')})");
        output.Write(indent); output.WriteLine("new byte[8] {");

        const int dotWidth = 5;
        const int dotHeight = 8;

        for (var dotY = 0; dotY < dotHeight; dotY++) {
          unsafe {
            var scanLine = new ReadOnlySpan<uint>(
              (data.Scan0 + data.Stride * (int)(characterPosition.y + dotSize.h * dotY + dotCenter.y)).ToPointer(),
              data.Stride
            );
            var line = 0b00000;
            var emoji = new string[dotWidth];

            for (var dotX = 0; dotX < dotWidth; dotX++) {
              var dot = scanLine[(int)(characterPosition.x + dotSize.w * dotX + dotCenter.x)];

              if (0x00808080 <= (dot & 0x00FFFFFF)) {
                line |= 0b1 << ((dotWidth - 1) - dotX);
                emoji[dotX] = "🟨";
              }
              else {
                emoji[dotX] = "🟪";
              }
            }

            output.Write(indent);
            output.WriteLine($"  0b_{Convert.ToString(line, 2).PadLeft(dotWidth, '0')}, // {string.Concat(emoji)}");
          }
        }

        output.Write(indent); output.WriteLine("},");
        output.WriteLine();
      }
    }

    output.WriteLine(
$@"
    }}; // end of field
  }} // end of class
}} // end of namespace"
    );
  }
  finally {
    if (data is not null)
      bitmap.UnlockBits(data);
  }
}
