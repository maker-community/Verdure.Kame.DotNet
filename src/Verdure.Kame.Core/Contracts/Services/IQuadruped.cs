using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verdure.Kame.Core
{
    public interface IQuadruped
    {
        Task HomePosAsync();
        Task WalkForwardAsync();
        Task WalkBackwardAsync();
        Task TurnLeftAsync();
        Task TurnRightAsync();
        Task BowAsync();
        Task BendBackAsync();
        Task PushUpAsync();
        Task JumpUpAsync();
        Task JumpBackAsync();
        Task DanceAsync();
        Task SwerveAsync();
        Task DemoAsync();
        Task SayHiAsync();
    }
}
