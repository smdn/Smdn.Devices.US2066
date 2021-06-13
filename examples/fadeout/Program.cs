// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Threading;

using Smdn.Devices.US2066;

using var display = SO1602A.Create(SO1602A.DefaultI2CAddress);

display.BlinkingCursorVisible = true;
display.UnderlineCursorVisible = true;

display.WriteLine("Fosc:Oscillator Frequency");
display.WriteLine("I:Fade-out interval");

Thread.Sleep(TimeSpan.FromSeconds(3.0));

while (true) {
  foreach (var oscFreq in new[] {
    InternalOscillatorFrequency.Frequency16,
    InternalOscillatorFrequency.Frequency8,
    InternalOscillatorFrequency.Frequency1,
  }) {
    display.InternalOscillatorFrequency = oscFreq;

    foreach (var (mode, interval) in new[] {
      (FadeOutMode.Blinking, FadeOutInterval.Step8Frames),
      (FadeOutMode.Blinking, FadeOutInterval.Step32Frames),
      (FadeOutMode.FadeOut, FadeOutInterval.Step8Frames),
      (FadeOutMode.FadeOut, FadeOutInterval.Step32Frames),
      (FadeOutMode.Disabled, (FadeOutInterval)default)
    }) {
      display.Clear();
      display.WriteLine($"mode: {mode}");
      display.WriteLine($"Fosc=0x{(int)oscFreq:X1} I=0x{(int)interval:X1}");

      display.FadeOutMode = mode;
      display.FadeOutInterval = interval;
      display.ResetFadeOutStep();

      Thread.Sleep(TimeSpan.FromSeconds(10.0));
    }
  }
}
