// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Microsoft.Extensions.DependencyInjection;

using Smdn.Devices.Mcp2221A;
using Smdn.Devices.US2066;
using Smdn.IO.UsbHid.DependencyInjection;

var services = new ServiceCollection();

services.AddHidSharpUsbHid();

using var serviceProvider = services.BuildServiceProvider();
using var mcp2221a = Mcp2221A.Create(serviceProvider);
using var display = SO1602A.Create(
  mcp2221a.I2c.CreateDevice(SO1602A.DefaultI2CAddress).WithFastMode()
);

display.Write("Hello, world!");
