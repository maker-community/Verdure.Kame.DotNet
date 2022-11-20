using Grpc.Core;
using Microsoft.Extensions.Logging;
using Verdure.Kame.Core;
using Verdure.Kame.DataTransmission;

namespace Verdure.Kame.IotGrpcService.Services
{
    public class DataTransmissionGrpcService : DataTransmissionGrpc.DataTransmissionGrpcBase
    {
        private readonly ILogger<DataTransmissionGrpcService> _logger;
        private readonly IQuadruped _quadruped;
        private readonly IQuadrupedFaceScreen _quadrupedFaceScreen;
        public DataTransmissionGrpcService(ILogger<DataTransmissionGrpcService> logger,
            IQuadruped quadruped,
            IQuadrupedFaceScreen quadrupedFaceScreen)
        {
            _logger = logger;
            _quadruped = quadruped;
            _quadrupedFaceScreen = quadrupedFaceScreen;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task<MsgReply> PlayImageOnFaceScreen(FaceScreenFrameRequest request, ServerCallContext context)
        {
            var imageData = request.FrameBuffer.ToByteArray();

            await _quadrupedFaceScreen.ShowImageAsync(imageData);

            return new MsgReply
            {
                StatusCode = 200,
                Message = "ok"
            };
        }

        public override async Task<MsgReply> PlayVideoOnFaceScreen(FaceScreenFrameListRequest request, ServerCallContext context)
        {

            if (request.FaceScreenFrames != null && request.FaceScreenFrames.Count > 0)
            {
                foreach (var face in request.FaceScreenFrames)
                {
                    var imageData = face.FrameBuffer.ToByteArray();

                    await _quadrupedFaceScreen.ShowImageAsync(imageData);

                }
            }
            return new MsgReply
            {
                StatusCode = 200,
                Message = "ok"
            };
        }

        public override async Task<MsgReply> ControlQuadrupedPosture(QuadrupedRequest request, ServerCallContext context)
        {
            if (request.ActionName != null)
            {
                switch (request.ActionName)
                {
                    case "HomePos":
                        await _quadruped.HomePosAsync();
                        break;
                    case "WalkForward":
                        await _quadruped.WalkForwardAsync();
                        break;
                    case "WalkBackward":
                        await _quadruped.WalkBackwardAsync();
                        break;
                    case "TurnLeft":
                        await _quadruped.TurnLeftAsync();
                        break;
                    case "TurnRight":
                        await _quadruped.TurnRightAsync();
                        break;
                    case "Bow":
                        await _quadruped.BowAsync();
                        break;
                    case "BendBack":
                        await _quadruped.BendBackAsync();
                        break;
                    case "PushUp":
                        await _quadruped.PushUpAsync();
                        break;
                    case "JumpUp":
                        await _quadruped.JumpUpAsync();
                        break;
                    case "JumpBack":
                        await _quadruped.JumpBackAsync();
                        break;
                    case "Dance":
                        await _quadruped.DanceAsync();
                        break;
                    case "Swerve":
                        await _quadruped.SwerveAsync();
                        break;
                    case "Demo":
                        await _quadruped.DemoAsync();
                        break;
                    case "SayHi":
                        await _quadruped.SayHiAsync();
                        break;
                    default:
                        break;
                }
            }


            return new MsgReply
            {
                StatusCode = 200,
                Message = "ok"
            };
        }
    }
}