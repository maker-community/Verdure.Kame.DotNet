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

        List<ServoMotor> servoMotors = new ();
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
            Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(150);
            servoMotors[8].WriteAngle(110);
            servoMotors[4].WriteAngle(60);
            servoMotors[10].WriteAngle(20);
            Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(130);
            servoMotors[11].WriteAngle(130);
            Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(110);
            servoMotors[8].WriteAngle(150);
            servoMotors[4].WriteAngle(20);
            servoMotors[10].WriteAngle(60);
            Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task WalkBackwardAsync()
        {
            servoMotors[7].WriteAngle(90);
            servoMotors[9].WriteAngle(90);
            Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(110);
            servoMotors[8].WriteAngle(150);
            servoMotors[4].WriteAngle(20);
            servoMotors[10].WriteAngle(60);
            Task.Delay(200); //200毫秒

            servoMotors[7].WriteAngle(70);
            servoMotors[9].WriteAngle(70);
            Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(130);
            servoMotors[11].WriteAngle(130);
            Task.Delay(200); //200毫秒

            servoMotors[6].WriteAngle(150);
            servoMotors[8].WriteAngle(110);
            servoMotors[4].WriteAngle(60);
            servoMotors[10].WriteAngle(20);
            Task.Delay(200); //200毫秒

            servoMotors[5].WriteAngle(110);
            servoMotors[11].WriteAngle(110);
            Task.Delay(200); //200毫秒

            return Task.CompletedTask;
        }

        public Task TurnLeftAsync()
        {
            throw new NotImplementedException();
        }

        public Task TurnRightAsync()
        {
            throw new NotImplementedException();
        }

        public Task BowAsync()
        {
            throw new NotImplementedException();
        }

        public Task BendBackAsync()
        {
            throw new NotImplementedException();
        }
        public Task PushUpAsync()
        {
            throw new NotImplementedException();
        }

        public Task JumpUpAsync()
        {
            throw new NotImplementedException();
        }

        public Task JumpBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task DanceAsync()
        {
            throw new NotImplementedException();
        }
        public Task SwerveAsync()
        {
            throw new NotImplementedException();
        }
        public Task DemoAsync()
        {
            throw new NotImplementedException();
        }

        public Task SayHiAsync()
        {
            throw new NotImplementedException();
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
