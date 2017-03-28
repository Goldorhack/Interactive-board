using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Tools.kinect_tools_dir
{
    public class Outil
    {
        public static void Ecrire_Erreur(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string err = ex.ToString();
            Console.WriteLine(err);
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
