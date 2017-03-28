using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_Tools.kinect_tools_dir;

namespace Kinect_Tools.Exemples
{
    internal class SshClientScenarios
    {
        private readonly SshClients.I_SshClient m_client;

        public SshClientScenarios(SshClients.I_SshClient sshClient)
        {
            this.m_client = sshClient;
        }

        #region scenario

        public void Exec01()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@" Connecté a : ");
            Console.WriteLine(m_client.ConnectionInfo.Host);

            ClientGoHSsh.ExecCommand(m_client, "uname -a");

        }


        public void Exec02()
        {

            ClientGoHSsh.ExecCommand(m_client, "piName=$(uname -n) && python /home/pi/Desktop/AZURIL/hat/hatSay.py $piName ");

        }



        public void Exec03()
        {



            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@" Entrer une commande ssh : ");

            string readLine = Console.ReadLine();

            while (readLine != "exit")
            {
                ClientGoHSsh.ExecCommand(m_client, readLine);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(@" Entrer une commande ssh : ");
                readLine = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }


        public void Exec04()
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@" Entrer un message a ecrire : ");

            string readLine = Console.ReadLine();

            while (readLine != "exit")
            {
                //ClientGoHSsh.Ecrire(readLine);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(@" Entrer un message a ecrire : ");
                readLine = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #endregion exemple

    }
}
