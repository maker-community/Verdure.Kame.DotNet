using Google.Protobuf;
using Google.Protobuf.Collections;
using Verdure.Kame.Core.Models;
using Verdure.Kame.DataTransmission;

namespace Verdure.Kame.Core.Services
{
    public class DataTransmissionClient
    {
        private readonly DataTransmissionGrpc.DataTransmissionGrpcClient _client;
        public DataTransmissionClient(DataTransmissionGrpc.DataTransmissionGrpcClient client)
        {
            _client = client;
        }

        public async Task<string> SayHelloAsync(string content, CancellationToken cancellationToken = default)
        {
            var hello = new HelloRequest
            {
                Name = content
            };
            var ret = await _client.SayHelloAsync(hello, cancellationToken: cancellationToken);

            return ret.Message;
        }

        public async Task<string> PlayImageOnFaceScreenAsync(byte[] frameBuffer, CancellationToken cancellationToken = default)
        {
            var data = new FaceScreenFrameRequest
            {
                FrameBuffer = ByteString.CopyFrom(frameBuffer)
            };

            var ret = await _client.PlayImageOnFaceScreenAsync(data, cancellationToken: cancellationToken);

            return ret.Message;
        }

        public async Task<string> PlayVideoOnFaceScreenAsync(List<FaceScreenFrame> faceScreenFrames, CancellationToken cancellationToken = default)
        {
            var requestData = new FaceScreenFrameListRequest();

            var dataList = new RepeatedField<FaceScreenFrameRequest>();

            if (faceScreenFrames != null && faceScreenFrames.Count > 0)
            {
                foreach (var faceFrame in faceScreenFrames)
                {
                    var faceData = new FaceScreenFrameRequest
                    {
                        FrameBuffer = ByteString.CopyFrom(faceFrame.FrameBuffer)
                    };
                    dataList.Add(faceData);
                }
            }
            requestData.FaceScreenFrames.AddRange(dataList);

            var ret = await _client.PlayVideoOnFaceScreenAsync(requestData);

            return ret.Message;
        }
    }
}
