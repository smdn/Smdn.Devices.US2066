// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Device.I2c;

namespace Smdn.Devices.US2066 {
  partial class US2066 {
    public static US2066 Create(int deviceAddress, int busId = 1)
      => Create(new I2cConnectionSettings(busId: busId, deviceAddress: deviceAddress));

    public static US2066 Create(I2cConnectionSettings connectionSettings)
      => Create(I2cDevice.Create(connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings))));

    public static US2066 Create(I2cDevice i2cDevice)
      => new US2066I2C(i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice)));
  }
}
