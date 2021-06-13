// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Smdn.Devices.MCP2221;
using Smdn.Devices.MCP2221.GpioAdapter;
using Smdn.Devices.US2066;

using var mcp2221 = MCP2221.Open();
using var display = SO1602A.Create(
  new MCP2221I2cDevice(mcp2221.I2C, SO1602A.DefaultI2CAddress) {
    BusSpeed = I2CBusSpeed.FastMode
  }
);

display.Write("Hello, world!");
