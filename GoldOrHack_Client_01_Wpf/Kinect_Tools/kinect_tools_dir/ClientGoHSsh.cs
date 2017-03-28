using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Renci.SshNet;

namespace Kinect_Tools.kinect_tools_dir
{

    public class ClientGoHSsh : I_ClientGoH
    {

        private readonly string m_config_Path;
        //private const string HOSTNAME_OR_IP = "192.168.43.164";
        //private const string USERNAME = "pi";

        private SshClients.I_SshClient m_client;
        private BackgroundWorker m_bw;
        private volatile Menu m_menu;

        public ConcurrentBag<string> LaListeDesCommandes { get; set; }


        public string LastResult { get; private set; }

        //private const string HOSTNAME_OR_IP = "azuril.ddns.net";
        //private const string USERNAME = "tyrix";

        #region ClientGoHSsh

        private void Init_01()
        {

            ConsoleManager.Show();

            string[] ts_Lines = File.ReadAllLines(m_config_Path);

            if (string.IsNullOrWhiteSpace(ts_Lines[0]))
            {
                Console.WriteLine(@" Pas de parametre dans le fichier : ");
                Console.WriteLine(m_config_Path);

                return;
            }
            else
            {

                string ip = ts_Lines[0];
                string username = ts_Lines[1];
                string password = ts_Lines[2];

                Console.WriteLine(@" connection a ");
                Console.WriteLine(username + @"@" + ip);
                Console.WriteLine(@" en cour ... ");

                AuthenticationMethod[] t_Auth = { new PasswordAuthenticationMethod(username, password) };
                ConnectionInfo connectionInfo = new ConnectionInfo(ip, username, t_Auth);

                try
                {
                    //throw new ApplicationException(" mock ");

                    this.m_client = new SshClients.SshClient2(ip, username, password);
                    this.m_client.Connect();

                    Console.WriteLine(@" connection reussi. ");


                    if (this.m_bw == null)
                    {
                        this.m_bw = new BackgroundWorker();
                    }

                    if (!this.m_bw.IsBusy)
                    {

                        this.m_bw = new BackgroundWorker();

                        const string bashGetCsv = "cat ~/Desktop/bash.csv.txt";

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(@" bash : ");
                        Console.WriteLine(bashGetCsv);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        m_bw.DoWork += (sender, args) => GetStringFromExecCommand(m_client, bashGetCsv);

                        m_bw.RunWorkerCompleted += M_bw_RunWorkerCompleted;

                        m_bw.RunWorkerAsync();
                    }

                }
                catch (Exception ex)
                {

                    Outil.Ecrire_Erreur(ex);

                    this.m_client = new SshClients.SshClientBouchon(connectionInfo);

                    Console.WriteLine(@" bouchon. ");

                    //throw;
                }

            }
            
            Console.WriteLine(@" fin ClientGoHSsh() ");
        }

        public ClientGoHSsh(ConcurrentBag<string> laListeDesCommandes, string config_Path)
        {
            this.LaListeDesCommandes = laListeDesCommandes;
            this.m_config_Path = config_Path;

            Init_01();
        }


        internal static void ExecCommand(SshClients.I_SshClient client, string commandText)
        {

            try
            {
                client.RunCommand(commandText);
            }
            catch (ArgumentException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@" Erreur : votre commande est vide. ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                Outil.Ecrire_Erreur(ex);
            }

        }


        internal void GetStringFromExecCommand(SshClients.I_SshClient client, string commandText)
        {

            try
            {
                this.LastResult = client.RunCommand(commandText).Result;

                this.m_menu = new Menu(LastResult);

            }
            catch (ArgumentException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@" Erreur : votre commande est vide. ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                Outil.Ecrire_Erreur(ex);
            }

        }

        private void ExecBashPrivate(string msg, string commandeHat)
        {

            if (this.m_bw == null)
            {
                this.m_bw = new BackgroundWorker();
            }

            if (!this.m_bw.IsBusy)
            {
                this.m_bw = new BackgroundWorker();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@" msg : " + msg);
                Console.ForegroundColor = ConsoleColor.Gray;

                m_bw.DoWork += (sender, args) => ExecCommand(m_client, commandeHat + "\"" + msg + "\"");

                m_bw.RunWorkerCompleted += M_bw_RunWorkerCompleted;

                m_bw.RunWorkerAsync();
            }

        }

        public void Ecrire(string msg)
        {
            const string commandeHat = "python /home/pi/Desktop/AZURIL/hat/hatSay.py ";

            ExecBashPrivate(msg, commandeHat);
        }

        private static void M_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        public void Dispose()
        {
            this.m_client.Disconnect();
            this.m_client.Dispose();
        }

        ~ClientGoHSsh()
        {
            this.Dispose();
        }

        public string[] GetMenu()
        {
            
            
            string[] ts =
            {
                "haut gauche",
                "haut droite",
                "milieu gauche",
                "milieu droite",
                "bas gauche",
                "bas droite",
            };

            if (this.m_menu != null)
            {
                List<MenuLigne> menuLignes = this.m_menu.MenuLignes;
                ts = menuLignes.Select(x => x.TitreCmd).ToArray();
            }
            else
            {
                Console.WriteLine(" menu is null. ");
            }

            return ts;
        }

        public void ExecuteFromTitreCmd(string titreCmd)
        {



        }

        #endregion ClientGoHSsh

    }

}
