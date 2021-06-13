// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Management;

#if MCP2221
using Smdn.Devices.MCP2221;
using Smdn.Devices.MCP2221.GpioAdapter;
#endif
using Smdn.Devices.US2066;

#if MCP2221
using var mcp2221 = MCP2221.Open();

using var display = SO1602A.Create(
  new MCP2221I2cDevice(mcp2221.I2C, SO1602A.DefaultI2CAddress) {
    BusSpeed = I2CBusSpeed.FastMode
  }
);
#else
using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);
#endif

display.CGRamUsage = CGRamUsage.UserDefined6Characters;
display.CreateCustomCharacter(
  CGRamCharacter.Character0,
  '℃',
  new byte[8] {
    0b_11000,
    0b_11000,
    0b_00111,
    0b_01000,
    0b_01000,
    0b_01000,
    0b_00111,
    0b_00000,
  }
);

display.BlinkingCursorVisible = false;
display.UnderlineCursorVisible = false;

Func<(double, double)> retrieveCPUStats =
  RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ? WindowsCPUStats.Retrieve
    : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
      ? LinuxCPUStats.Retrieve
      : throw new PlatformNotSupportedException();

var updateInterval = TimeSpan.FromSeconds(0.5);

for (;;) {
  var (temperature, usage) = retrieveCPUStats();

  display.Clear();
  display.WriteLine($"CPU: {temperature:F1}℃ {usage:P1}");

  await Task.Delay(updateInterval);
}

static class WindowsCPUStats {
  static readonly PerformanceCounter pcTotalProcessorTime = new("Processor", "% Processor Time", "_Total");

  public static (double, double) Retrieve()
  {
    var searcher = new ManagementObjectSearcher(
      @"root\WMI",
      "SELECT * FROM MSAcpi_ThermalZoneTemperature"
    );

    double temp = double.NaN;

    try {
      foreach (ManagementBaseObject obj in searcher.Get()) {
        temp = (double)obj["CurrentTemperature"];
        temp = (temp - 2732) / 10.0;
        break;
      }
    }
    catch (ManagementException ex) {
      Console.Error.WriteLine($"{ex.GetType().Name}: {ex.Message} ({ex.ErrorCode})");
    }

    var usage = pcTotalProcessorTime.NextValue() / 100.0;

    return (temp, usage);
  }
}

static class LinuxCPUStats {
  static double prevIdle = default;
  static double prevNonIdle = default;
  static double prevTotal = default;

  public static (double, double) Retrieve()
  {
    var temp = double.Parse(File.ReadAllText("/sys/class/thermal/thermal_zone0/temp")) / 1000.0;

    double usage = default;

    var cpuStat = File.ReadLines("/proc/stat").First().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    var user    = int.Parse(cpuStat[1]);
    var nice    = int.Parse(cpuStat[2]);
    var system  = int.Parse(cpuStat[3]);
    var idle    = int.Parse(cpuStat[4]);
    var iowait  = int.Parse(cpuStat[5]);
    var irq     = int.Parse(cpuStat[6]);
    var softirq = int.Parse(cpuStat[7]);
    var steal   = int.Parse(cpuStat[8]);

    var currIdle = idle + iowait;
    var currNonIdle = user + nice + system + irq + softirq + steal;
    var currTotal = currIdle + currNonIdle;

    if (prevTotal != default) {
      var diffTotal = currTotal - prevTotal;
      var diffIdle = currIdle - prevIdle;

      usage = (diffTotal - diffIdle) / (double)diffTotal;
    }

    prevIdle    = currIdle;
    prevNonIdle = currNonIdle;
    prevTotal   = currTotal;

    return (temp, usage);
  }
}
