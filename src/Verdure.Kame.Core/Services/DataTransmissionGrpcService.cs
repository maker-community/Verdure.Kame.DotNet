using Grpc.Core;
using Microsoft.Extensions.Logging;
using Verdure.Kame.DataTransmission;
using Verdure.Kame.IotGrpcService;

namespace Verdure.Kame.IotGrpcService.Services
{
    public class DataTransmissionGrpcService : DataTransmissionGrpc.DataTransmissionGrpcBase
    {
        private readonly ILogger<DataTransmissionGrpcService> _logger;
        public DataTransmissionGrpcService(ILogger<DataTransmissionGrpcService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}