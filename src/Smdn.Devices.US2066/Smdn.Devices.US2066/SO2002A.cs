// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Device.I2c;

namespace Smdn.Devices.US2066 {
  public class SO2002A : SOXXXXA {
    public static SO2002A Create(int deviceAddress, int busId = 1)
      => new SO2002A(US2066.Create(deviceAddress: deviceAddress, busId: busId));

    [CLSCompliant(false)]
    public static SO2002A Create(I2cConnectionSettings connectionSettings)
      => new SO2002A(US2066.Create(connectionSettings: connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings))));

    [CLSCompliant(false)]
    public static SO2002A Create(I2cDevice i2cDevice)
      => new SO2002A(US2066.Create(i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice))));

    /*
     * instance members
     */
    public override int NumberOfCharsPerLine => 20;

    private SO2002A(US2066 controller)
      : base(controller)
    {
    }
  }
}