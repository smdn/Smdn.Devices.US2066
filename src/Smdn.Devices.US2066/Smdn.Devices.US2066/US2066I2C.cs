// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Device.I2c;

namespace Smdn.Devices.US2066 {
  internal class US2066I2C : US2066 {
    private I2cDevice _i2cDevice;
    private I2cDevice I2CDevice => _i2cDevice ?? throw new ObjectDisposedException(GetType().FullName);

    private readonly bool isI2CDeviceMCP2221;

    public override int Address {
      get => throw new NotSupportedException();
      private protected set => throw new NotSupportedException();
    }
    public override int PartID {
      get => throw new NotSupportedException();
      private protected set => throw new NotSupportedException();
    }
    public override bool IsBusy => throw new NotSupportedException();

    public US2066I2C(I2cDevice i2cDevice)
    {
      this._i2cDevice = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));
      this.isI2CDeviceMCP2221 = i2cDevice.GetType().FullName.Equals("Smdn.Devices.MCP2221.GpioAdapter.MCP2221I2cDevice", StringComparison.Ordinal);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing) {
        _i2cDevice?.Dispose();
        _i2cDevice = null;
      }
    }

    protected override void SendByteSequence(byte controlByte, ReadOnlySpan<byte> byteSequence)
    {
      var chunkSize = isI2CDeviceMCP2221
        ? 12 // ???
        : byteSequence.Length;

      Span<byte> sequence = stackalloc byte[1 + chunkSize];

      while (!byteSequence.IsEmpty) {
        var lengthToTransfer = Math.Min(byteSequence.Length, chunkSize);

        sequence[0] = controlByte;
        byteSequence.Slice(0, lengthToTransfer).CopyTo(sequence.Slice(1));

        I2CDevice.Write(sequence.Slice(0, 1 + lengthToTransfer));

        byteSequence = byteSequence.Slice(lengthToTransfer);
      }
    }

    protected override byte ReceiveByte(byte controlByte)
    {
      I2CDevice.WriteByte(controlByte);

      return I2CDevice.ReadByte();
    }
  }
}