// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Devices.US2066;

public abstract class SOXXXXA : US2066DisplayModuleBase {
  public const byte DefaultI2CAddress = 0x3C;
  public const byte SecondaryI2CAddress = 0x3D;

  protected override DisplayLineNumber DisplayLineNumber => DisplayLineNumber.Lines4; // Even SOXX02A (2-lines), it is defined as 4-lines (?)
  protected override DisplayDotFormat DisplayDotFormat => DisplayDotFormat.Dots5x8;

  private protected SOXXXXA(US2066 oledInterface)
    : base(oledInterface)
  {
  }
}
