using Iot.Device.Pwm;
using Iot.Device.ServoMotor;
using System.Device.I2c;

namespace Verdure.Kame.Core
{
    public class Quadruped : IQuadruped, IDisposable
    {
        // Some SG90s can do 180 angle range but some other will be oscillating on the edge values
        // Max angle which doesn't cause any issues found experimentally was as below.
        // The ones which can do 180 will have the minimum pulse width at around 520uS.
        private const int AngleRange = 173;
        private const int MinPulseWidthMicroseconds = 600;
        private const int MaxPulseWidthMicroseconds = 2590;

        private const int ServoMotorCount = 16;

        int busId = 1;
        int selectedI2cAddress = 0b000000; // A5 A4 A3 A2 A1 A0

        I2cConnectionSettings settings;
        I2cDevice device;
        Pca9685 pca9685;

        List<ServoMotor> servoMotors = new();
        public Quadruped()
        {
            settings = new(busId, Pca9685.I2cAddressBase + selectedI2cAddress);

            device = I2cDevice.Create(settings);

            pca9685 = new(device);

            CreateServo(pca9685);

        }

        public Task HomePosAsync()
        {
            servoMotors[4].WriteAngle(40);//FL HIP
            servoMotors[5].WriteAngle(110);   //FL FOOT
            servoMotors[6].WriteAngle(130);   //FR HIP
            servoMotors[7].WriteAngle(70);   //FR FOOT
            servoMotors[8].WriteAngle(130);   //BL HIP
            servoMotors[9].WriteAngle(70);    //BL FOOT
            servoMotors[10].WriteAngle(40);    //BR HIP
            servoMotors[11].WriteAngle(110);  //BR FOOT
            return Task.CompletedTask;
        }

        public Task WalkForwardAsync()
        {
            servoMotors[7].WriteAngle(50);
            servoMotors[9].WriteAngle(50);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(150);
            servoMotors[8].WriteAngle(110);
            servoMotors[4].WriteAngle(60);
            servoMotors[10].WriteAngle(20);
            await Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(130);
            servoMotors[11].WriteAngle(130);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(110);
            servoMotors[8].WriteAngle(150);
            servoMotors[4].WriteAngle(20);
            servoMotors[10].WriteAngle(60);
            await Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task WalkBackwardAsync()
        {
            servoMotors[7].WriteAngle(90);
            servoMotors[9].WriteAngle(90);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(110);
            servoMotors[8].WriteAngle(150);
            servoMotors[4].WriteAngle(20);
            servoMotors[10].WriteAngle(60);
            await Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(70);
            servoMotors[9].WriteAngle(70);
            await Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(130);
            servoMotors[11].WriteAngle(130);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(150);
            servoMotors[8].WriteAngle(110);
            servoMotors[4].WriteAngle(60);
            servoMotors[10].WriteAngle(20);
            await Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task TurnLeftAsync()
        {
            servoMotors[5].WriteAngle(130);
            servoMotors[11].WriteAngle(130);
            await Task.Delay(200); //200毫秒

            servoMotors[4].WriteAngle(10);
            servoMotors[10].WriteAngle(10);
            await Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(200); //200毫秒

            servoMotors[4].WriteAngle(40);
            servoMotors[10].WriteAngle(40);
            await Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(50);
            servoMotors[9].WriteAngle(50);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(80);
            servoMotors[8].WriteAngle(80);
            await Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(70);
            servoMotors[9].WriteAngle(70);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(130);
            servoMotors[8].WriteAngle(130);
            await Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task TurnRightAsync()
        {
            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(200); //200毫秒

            servoMotors[4].WriteAngle(40);
            servoMotors[10].WriteAngle(40);
            await Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(50);
            servoMotors[9].WriteAngle(50);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(180);
            servoMotors[8].WriteAngle(180);
            await Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(70);
            servoMotors[9].WriteAngle(70);
            await Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(130);
            servoMotors[8].WriteAngle(130);
            await Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task BowAsync()
        {
            servoMotors[5].WriteAngle(140);
            servoMotors[7].WriteAngle(15);
            servoMotors[9].WriteAngle(130);
            servoMotors[11].WriteAngle(30);
            await Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task BendBackAsync()
        {
            servoMotors[5].WriteAngle(30);
            servoMotors[7].WriteAngle(130);
            servoMotors[9].WriteAngle(15);
            servoMotors[11].WriteAngle(140);
            Tawait ask.Delay(200); //200毫秒

            return Task.CompletedTask;
        }
        public Task PushUpAsync()
        {
            servoMotors[4].WriteAngle(130);
            servoMotors[5].WriteAngle(70);
            servoMotors[6].WriteAngle(60);
            servoMotors[7].WriteAngle(130);

            servoMotors[8].WriteAngle(170);
            servoMotors[9].WriteAngle(50);
            servoMotors[10].WriteAngle(10);
            servoMotors[11].WriteAngle(130);
            await Task.Delay(1000); //1秒

            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 80; k++)
                {
                    servoMotors[5].WriteAngle(70 + k);
                    servoMotors[7].WriteAngle(130 - k);
                    await Task.Delay(1); //1毫秒
                }
                for (int k = 0; k < 80; k++)
                {
                    servoMotors[5].WriteAngle(110 - k);
                    servoMotors[7].WriteAngle(90 + k);
                    await Task.Delay(1); //1毫秒
                }
            }
            return Task.CompletedTask;
        }

        public Task JumpUpAsync()
        {
            servoMotors[4].WriteAngle(10);
            servoMotors[5].WriteAngle(20);
            servoMotors[6].WriteAngle(170);
            servoMotors[7].WriteAngle(120);
            servoMotors[8].WriteAngle(0);
            servoMotors[9].WriteAngle(70);
            servoMotors[10].WriteAngle(160);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(3000); //3秒

            servoMotors[9].WriteAngle(140);
            servoMotors[11].WriteAngle(40);
            await Task.Delay(500); //500毫秒

            return Task.CompletedTask;
        }

        public Task JumpBackAsync()
        {
            servoMotors[4].WriteAngle(10);
            servoMotors[5].WriteAngle(20);
            servoMotors[6].WriteAngle(170);
            servoMotors[7].WriteAngle(120);
            servoMotors[8].WriteAngle(0);
            servoMotors[9].WriteAngle(70);
            servoMotors[10].WriteAngle(160);
            servoMotors[11].WriteAngle(110);
            await Task.Delay(3000); //3秒

            servoMotors[9].WriteAngle(140);
            servoMotors[11].WriteAngle(40);
            await Task.Delay(1500); //1.5秒

            servoMotors[4].WriteAngle(40);
            servoMotors[5].WriteAngle(110);
            servoMotors[6].WriteAngle(130);
            servoMotors[7].WriteAngle(70);
            servoMotors[8].WriteAngle(130);
            servoMotors[9].WriteAngle(70);
            servoMotors[10].WriteAngle(40);
            servoMotors[11].WriteAngle(110);

            return Task.CompletedTask;
        }

        public async Task DanceAsync()
        {
            servoMotors[4].WriteAngle(0);
            servoMotors[6].WriteAngle(180);
            servoMotors[8].WriteAngle(180);
            servoMotors[10].WriteAngle(0);

            for (int i = 0; i < 5; i++)
            {
                servoMotors[7].WriteAngle(50);
                servoMotors[9].WriteAngle(60);
                servoMotors[5].WriteAngle(110);
                servoMotors[11].WriteAngle(110);
                await Task.Delay(200); //200毫秒

                servoMotors[7].WriteAngle(20);
                servoMotors[9].WriteAngle(120);
                await Task.Delay(200); //200毫秒

                servoMotors[7].WriteAngle(70);
                servoMotors[9].WriteAngle(70);
                servoMotors[5].WriteAngle(150);
                servoMotors[11].WriteAngle(50);
                await Task.Delay(200); //200毫秒
            }

            return Task.CompletedTask;
        }
        public Task SwerveAsync()
        {
            servoMotors[4].WriteAngle(40);
            servoMotors[5].WriteAngle(110);
            servoMotors[6].WriteAngle(130);
            servoMotors[7].WriteAngle(70);
            servoMotors[8].WriteAngle(130);
            servoMotors[9].WriteAngle(70);
            servoMotors[10].WriteAngle(40);
            servoMotors[11].WriteAngle(110);

            for (int i = 0; i < 2; i++)
            {
                servoMotors[4].WriteAngle(170);
                servoMotors[6].WriteAngle(180);
                servoMotors[8].WriteAngle(180);
                servoMotors[10].WriteAngle(140);
                Task.Delay(800); //800毫秒
                servoMotors[4].WriteAngle(0);
                servoMotors[6].WriteAngle(0);
                servoMotors[8].WriteAngle(0);
                servoMotors[10].WriteAngle(0);
                Task.Delay(800); //800毫秒
            }

            return Task.CompletedTask;
        }
        public async Task DemoAsync()
        {
            await HomePosAsync();
            await WalkBackwardAsync();
            await Task.Delay(1000); //1秒

            await WalkBackwardAsync();
            await Task.Delay(1000);

            await TurnLeftAsync();
            await Task.Delay(1000);

            await TurnRightAsync();
            await Task.Delay(1000);

            await BowAsync();
            await Task.Delay(1000);

            await BendBackAsync();
            await Task.Delay(1000);

            await PushUpAsync();
            await Task.Delay(1000);

            await JumpBackAsync();
            await Task.Delay(1000);

            return Task.CompletedTask;
        }

        public async Task SayHiAsync()
        {
            await HomePosAsync();
            servoMotors[7].WriteAngle(0);
            for (int i = 0; i < 5; i++)
            {
                servoMotors[6].WriteAngle(80);
                await Task.Delay(300); //300毫秒

                servoMotors[6].WriteAngle(80);
                await Task.Delay(300); //300毫秒

            }
            return Task.CompletedTask;
        }

        private void CreateServo(Pca9685 pca9685)
        {
            for (int i = 0; i < ServoMotorCount; i++)
            {
                var servoMotor = new ServoMotor(pca9685.CreatePwmChannel(i),
                    AngleRange, MinPulseWidthMicroseconds, MaxPulseWidthMicroseconds);

                servoMotors.Add(servoMotor);
            }
        }

        public void Dispose()
        {
            servoMotors.Clear();
            device.Dispose();
            pca9685.Dispose();
        }
    }
}
