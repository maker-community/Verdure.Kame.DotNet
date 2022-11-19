using OpenCvSharp;
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

            var mat3 = new Mat();

            Cv2.Rotate(mat, mat3, RotateFlags.Rotate90Clockwise); //再旋转

            var mat1 = mat3.Resize(new OpenCvSharp.Size(172, 320), 0, 0, OpenCvSharp.InterpolationFlags.Area);

            var mat2 = mat1.CvtColor(OpenCvSharp.ColorConversionCodes.RGBA2BGR565);

            var dataMeta = mat2.Data;

            var data = new byte[172 * 320 * 2];

            Marshal.Copy(dataMeta, data, 0, 172 * 320 * 2);

            return Task.FromResult(data);
        }
    }
}
