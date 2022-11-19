namespace Verdure.Kame.Core.Models
{
    public class FaceScreenFrame
    {
        public FaceScreenFrame(byte[] frameBuffer)
        {
            FrameBuffer= frameBuffer;
        }
        public byte[] FrameBuffer { get; set; }
    }
}
