using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdure.Kame.Core
{
    public interface IQuadrupedFaceScreen
    {
        Task ShowImageAsync(byte[] data);

        void ClearScreen();
    }
}
