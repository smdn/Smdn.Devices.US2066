![S](./misc/characterbitmaps//CGROM-C/50/53.svg)![m](./misc/characterbitmaps//CGROM-C/60/6D.svg)![d](./misc/characterbitmaps//CGROM-C/60/64.svg)![n](./misc/characterbitmaps//CGROM-C/60/6E.svg)![.](./misc/characterbitmaps//CGROM-C/20/2E.svg)![D](./misc/characterbitmaps//CGROM-C/40/44.svg)![e](./misc/characterbitmaps//CGROM-C/60/65.svg)![v](./misc/characterbitmaps//CGROM-C/70/76.svg)![i](./misc/characterbitmaps//CGROM-C/60/69.svg)![c](./misc/characterbitmaps//CGROM-C/60/63.svg)![e](./misc/characterbitmaps//CGROM-C/60/65.svg)![s](./misc/characterbitmaps//CGROM-C/70/73.svg)![.](./misc/characterbitmaps//CGROM-C/20/2E.svg)![U](./misc/characterbitmaps//CGROM-C/50/55.svg)![S](./misc/characterbitmaps//CGROM-C/50/53.svg)![2](./misc/characterbitmaps//CGROM-C/30/32.svg)![0](./misc/characterbitmaps//CGROM-C/30/30.svg)![6](./misc/characterbitmaps//CGROM-C/30/36.svg)![6](./misc/characterbitmaps//CGROM-C/30/36.svg)

[![GitHub license](https://img.shields.io/github/license/smdn/Smdn.Devices.US2066)](https://github.com/smdn/Smdn.Devices.US2066/blob/main/LICENSE.txt)
[![tests/main](https://img.shields.io/github/actions/workflow/status/smdn/Smdn.Devices.US2066/test.yml?branch=main&label=tests%2Fmain)](https://github.com/smdn/Smdn.Devices.US2066/actions/workflows/test.yml)
[![CodeQL](https://github.com/smdn/Smdn.Devices.US2066/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/smdn/Smdn.Devices.US2066/actions/workflows/codeql-analysis.yml)
[![NuGet Smdn.Devices.US2066](https://img.shields.io/nuget/v/Smdn.Devices.US2066.svg)](https://www.nuget.org/packages/Smdn.Devices.US2066/)

# Smdn.Devices.US2066
[Smdn.Devices.US2066](src/Smdn.Devices.US2066/) is a .NET library for controlling [WiseChip US2066](https://www.wisechip.com.tw/) OLED Driver with Controller.

This library enables you to control OLED character display modules which has US2066 controller chip.

The supported display modules are listed in [tested and supported display modules](#tested-and-supported-oled-display-modules).



## Hardware supports
`Smdn.Devices.US2066` is based on [Iot.Device.Bindings](https://www.nuget.org/packages/Iot.Device.Bindings/). This library enables you to control OLED character displays connected to the board like Raspberry Pi which `Iot.Device.Bindings` supports.

Also combined with the library [Smdn.Devices.Mcp2221A](https://github.com/smdn/Smdn.Devices.Mcp2221A), you can control the OLED displays via USB connection even on generic PCs without using the specific board like Arduino. See [MCP2221A example](examples/MCP2221A/).

## Library API features
### `string` to `byte[]` conversion
`Smdn.Devices.US2066` can output `string`s directly. Symbols and non-ASCII characters are automatically converted to the appropriate `byte`s, and are displayed on the OLED display.

As an example, the `string` `"ÄÁΩ⏫￫"` will be displayed like ![Ä](./misc/characterbitmaps/CGROM-A/50/5B.svg) ![Á](./misc/characterbitmaps/CGROM-A/E0/E2.svg) ![Ω](./misc/characterbitmaps/CGROM-A/B0/B5.svg) ![⏫](./misc/characterbitmaps/CGROM-A/10/12.svg) ![￫](./misc/characterbitmaps/CGROM-A/D0/DF.svg).

For more information about character mapping, see [this document](./doc/characters/).

(⚠The [character mapping of CGROM-B](./doc/characters/CGROM-B.md) and some other characters, especially alphabets with diacritical marks, are incomplete. Contributions are welcome! See [issue #2](https://github.com/smdn/Smdn.Devices.US2066/issues/2))

### Japanese and Russian conversion
`Smdn.Devices.US2066` also supports Japanese full-width Hiragana/Katakana to half-width Katakana conversion(全角かな/全角カナ→半角カナ変換), Russian lower case to upper case conversion.

As an example, the following `string`s will be displayed like below.

|`string`|Characters on display|
|--------|---------------------|
|`"こんにちは、せかい！"`|![こ](./misc/characterbitmaps//CGROM-C/B0/BA.svg)![ん](./misc/characterbitmaps//CGROM-C/D0/DD.svg)![に](./misc/characterbitmaps//CGROM-C/C0/C6.svg)![ち](./misc/characterbitmaps//CGROM-C/C0/C1.svg)![は](./misc/characterbitmaps//CGROM-C/C0/CA.svg)![、](./misc/characterbitmaps//CGROM-C/A0/A4.svg)![せ](./misc/characterbitmaps//CGROM-C/B0/BE.svg)![か](./misc/characterbitmaps//CGROM-C/B0/B6.svg)![い](./misc/characterbitmaps//CGROM-C/B0/B2.svg)![！](./misc/characterbitmaps//CGROM-C/20/21.svg)|
|`"Привет, мир!"`|![П](./misc/characterbitmaps//CGROM-B/80/8F.svg)![р](./misc/characterbitmaps//CGROM-B/90/90.svg)![и](./misc/characterbitmaps//CGROM-B/80/88.svg)![в](./misc/characterbitmaps//CGROM-B/80/82.svg)![е](./misc/characterbitmaps//CGROM-B/80/85.svg)![т](./misc/characterbitmaps//CGROM-B/90/92.svg)![,](./misc/characterbitmaps//CGROM-B/20/2C.svg)![ ](./misc/characterbitmaps//CGROM-B/20/20.svg)![м](./misc/characterbitmaps//CGROM-B/80/8C.svg)![и](./misc/characterbitmaps//CGROM-B/80/88.svg)![р](./misc/characterbitmaps//CGROM-B/90/90.svg)![!](./misc/characterbitmaps//CGROM-B/20/21.svg)|

See example of [helloworld-ja](examples/helloworld-ja/) and [helloworld-ru](examples/helloworld-ru/).

### Custom characters
`US2066` supports registering custom characters. `Smdn.Devices.US2066` can map custom characters to any characters including emojis on registration.

```cs
using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

display.CGRamUsage = CGRamUsage.UserDefined6Characters;

// define and register a custom character, and map to the emoji
display.CreateCustomCharacter(
  CGRamCharacter.Character0,
  "🙂", // emoji for this custom character
  new byte[8] {
    0b_00000, // 🟪🟪🟪🟪🟪
    0b_01010, // 🟪🟨🟪🟨🟪
    0b_01010, // 🟪🟨🟪🟨🟪
    0b_00000, // 🟪🟪🟪🟪🟪
    0b_10001, // 🟨🟪🟪🟪🟨
    0b_01110, // 🟪🟨🟨🟨🟪
    0b_00000, // 🟪🟪🟪🟪🟪
    0b_00000, // 🟪🟪🟪🟪🟪
  }
);

// display the registered custom character by specifying mapped emoji
display.Write("🙂");
```

See example of [customcharacters](examples/customcharacters) for detail.



## Supported US2066 features/commands
- Interface
  - [ ] 4/b bit 6800/8080
  - [ ] SPI
  - [x] I<sup>2</sup>C
- Fundamental Command Set
  - [x] Clear Display
  - [x] Return Home
  - [ ] Entry Mode Set
  - [x] Display ON/OFF Control
  - [x] Extended Function Set
  - [ ] Cursor or Display Shift
  - [ ] Double Height / Display dot shift
  - [ ] Shift Enable
  - [ ] Scroll Enable
  - [x] Function Set
  - [x] Set CGRAM address
  - [x] Set DDRAM address
  - [ ] Set Scroll Quantity
  - [x] Read Busy Flag and Address/Part ID
    - not tested
  - [x] Write data
  - [x] Read data
    - not tested
- Extended Command Set
  - [ ] Function Selection A
  - [x] Function Selection B
  - [x] OLED Characterization
- OLED Command Set
  - [x] Set Contrast Control
  - [x] Set Display Clock Divide Ratio / Oscillator Frequency
  - [ ] Set Phase Length
  - [ ] Set SEG Pins Hardware Configuration
  - [ ] Set V<sub>COMH</sub> Deselect Level
  - [ ] Function Selection C
  - [x] Set Fade Out and Blinking

## Tested and supported OLED display modules
|Series                                                                                             |Model No.|Size |Image                                      |
|---------------------------------------------------------------------------------------------------|---------|-----|:-----------------------------------------:|
|[SUNLIKE DISPLAY SO Series](https://www.lcd-modules.com.tw/page/about/index.aspx?kind=27) (SOXXXXA)|SO1602A  |16×2|![SO1602A](doc/display-modules/SO1602A.jpg)|
|                                                                                                   |SO2002A  |20×2|![SO2002A](doc/display-modules/SO2002A.jpg)|

By deriving from the class [`US2066DisplayModuleBase`](src/Smdn.Devices.US2066/Smdn.Devices.US2066/US2066DisplayModuleBase.cs), other display modules can also be worked.

# Getting started and examples
Firstly, add package [Smdn.Devices.US2066](https://www.nuget.org/packages/Smdn.Devices.US2066/) to your project.

```
dotnet add package Smdn.Devices.US2066
```

Nextly, write your codes. The simpliest `Hello, world!` code is like below. This code uses the display module `SO1602A`.

```cs
using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

display.Write("Hello, world!");
```

For detailed instructions, including wiring of the devices and parts, see [examples/helloworld](examples/helloworld/README.md).

More examples can be found in [examples](examples/) directory.
