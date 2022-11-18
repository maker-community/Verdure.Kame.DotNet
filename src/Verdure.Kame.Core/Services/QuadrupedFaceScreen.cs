using Iot.Device.ST7789V3;
using System.Device.Pwm.Drivers;
using System.Device.Spi;

namespace Verdure.Kame.Core
{
    public class QuadrupedFaceScreen : IQuadrupedFaceScreen, IDisposable
    {
        int resetPin = 27;
        int dataCommandPin = 25;
        int backlightPin = 18;
        int blFreq = 1000;

        private bool isSending;

        // SPI0 CS0
        SpiConnectionSettings senderSettings = new(0, 0)
        {
            ClockFrequency = ST7789V3.SpiClockFrequency,
            Mode = ST7789V3.SpiMode
        };

        SpiDevice senderDevice;

        SoftwarePwmChannel pwmChannel;

        ST7789V3 lcd;

        public QuadrupedFaceScreen()
        {
            senderDevice = SpiDevice.Create(senderSettings);

            pwmChannel = new SoftwarePwmChannel(pinNumber: backlightPin, frequency: blFreq);

            lcd = new ST7789V3(dataCommandPin, senderDevice, resetPin, pwmChannel, shouldDispose: false);

            lcd.Init();

            lcd.SetWindows(0, 0, 172, 320);
        }
        public Task ShowImageAsync(byte[] data)
        {
            if (isSending == false)
            {
                isSending = true;

                lcd.SpiWrite(true, new ReadOnlySpan<byte>(data));
                //todo: send data;
            }
            isSending = false;
            return Task.CompletedTask;
        }

        public void ClearScreen()
        {
            lcd.Clear();
        }

        public void Dispose()
        {
            lcd.Dispose();
            pwmChannel.Dispose();
            senderDevice.Dispose();
        }
    }
}
