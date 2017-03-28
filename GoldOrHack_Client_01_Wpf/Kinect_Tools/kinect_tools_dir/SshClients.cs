using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Kinect_Tools.kinect_tools_dir
{
    public class SshClients
    {
        
        #region bouchon

        internal interface I_SshClient
        {
            void Connect();

            SshCommand RunCommand(string commandText);

            void Disconnect();

            void Dispose();

            ConnectionInfo ConnectionInfo { get; }

        }

        public class SshClientBouchon : I_SshClient
        {
            public SshClientBouchon(ConnectionInfo connectionInfo)
            {
                ConnectionInfo = connectionInfo;
            }

            public void Connect()
            {
            }

            SshCommand I_SshClient.RunCommand(string commandText)
            {
                Wait();
                return null;
            }

            private static void Wait()
            {
                const int seconde = 1;
                const int miliseconde = 1000 * seconde;
                System.Threading.Thread.Sleep(miliseconde);
            }

            public void Disconnect()
            {
            }

            public void Dispose()
            {
            }

            public ConnectionInfo ConnectionInfo { get; }
        }

        #endregion bouchon

        #region sshClientReel

        internal class SshClient2 : Renci.SshNet.SshClient, I_SshClient
        {


            public SshClient2(ConnectionInfo connectionInfo) : base(connectionInfo)
            {
            }

            public SshClient2(string host, int port, string username, string password) : base(host, port, username, password)
            {
            }

            public SshClient2(string host, string username, string password) : base(host, username, password)
            {
            }

            public SshClient2(string host, int port, string username, params PrivateKeyFile[] keyFiles) : base(host, port, username, keyFiles)
            {
            }

            public SshClient2(string host, string username, params PrivateKeyFile[] keyFiles) : base(host, username, keyFiles)
            {
            }
            
        }

        #endregion sshClientReel

    }
}
