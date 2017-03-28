using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Tools.kinect_tools_dir
{
    public interface I_ClientGoH : IDisposable
    {
        void Ecrire(string s);
        string[] GetMenu();
        void ExecuteFromTitreCmd(string titreCmd);
    }
}
