using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdure.Kame.Maui.Assistant.Services
{
    public interface IFaceScreenMediaPlayer
    {
        public Task<byte[]> ConvertImageStreamToBytesAsync(Stream stream);
    }
}
