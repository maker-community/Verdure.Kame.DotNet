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

        public override Task<MsgReply> PlayVideoOnFaceScreen(FaceScreenFrameListRequest request, ServerCallContext context)
        {

            return base.PlayVideoOnFaceScreen(request, context);
        }
    }
}