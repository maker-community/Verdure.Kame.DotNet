using System.Drawing;
using System.Runtime.InteropServices;
using Verdure.Kame.Maui.Assistant.Services;

namespace Verdure.Kame.Maui.Assistant.Platforms.Windows
{
    public class FaceScreenMediaPlayer : IFaceScreenMediaPlayer
    {
        public Task<byte[]> ConvertImageStreamToBytesAsync(Stream stream)
        {

            var image = new Bitmap(stream);

            var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(image);

            var mat1 = mat.Resize(new OpenCvSharp.Size(127, 320), 0, 0, OpenCvSharp.InterpolationFlags.Area);

            var mat2 = mat1.CvtColor(OpenCvSharp.ColorConversionCodes.RGBA2BGR);

            var dataMeta = mat2.Data;

            var data = new byte[127 * 320 * 2];

            Marshal.Copy(dataMeta, data, 0, 127 * 320 * 2);

            return Task.FromResult(data);
        }
    }
}
