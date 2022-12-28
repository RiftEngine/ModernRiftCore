using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    public abstract class Subroutine
    {
        public abstract bool IsDone();
        public abstract void Initialize();
        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }

    public class SubroutineManager
    {
        public static void StartSubroutine(Subroutine subroutine)
        {
            subroutine.Initialize();
            subroutine.Start();
            while(!subroutine.IsDone())
            {
                subroutine.Update();
            }
            subroutine.End();
        }
    }
}
