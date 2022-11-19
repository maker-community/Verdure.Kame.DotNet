using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verdure.Kame.Core.Models;

namespace Verdure.Kame.Maui.Assistant.Services
{
    public interface IFaceScreenMediaPlayer
    {
        public Task<byte[]> ConvertImageStreamToBytesAsync(Stream stream);

        public Task<IEnumerable<FaceScreenFrame>> ConvertVideoToFaceScreenFramesAsync(string filePath);
    }
}
