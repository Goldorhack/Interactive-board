using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kinect_Tools.kinect_tools_dir
{
    public class Menu : IDisposable
    {

        private const string URL_GET_BASH = "ssh/GetBash.php?host=";
        private const string URL_EXEC_BASH = "ssh/ExecBashFromTitle.php?host=";


        private readonly string[] m_sep1 = { "\n", "\r", "<br />" };
        private const char SEP2 = ';';

        private readonly string m_rc = Environment.NewLine;

        private readonly Uri m_mainHost;

        private string m_targetHost;

        private int m_startMenu = 0;

        private List<MenuLigne> m_lesMenuLignes;
        private readonly Uri m_url_Menu_Host;

        public Uri MainHost
        {
            get { return m_mainHost; }
        }

        public List<MenuLigne> MenuLignes
        {
            get { return m_lesMenuLignes; }
        }

        private static string GetStringFromUrl(Uri uri)
        {
            using (WebClient client = new WebClient())
            {

                try
                {
                    return client.DownloadString(uri);
                }
                catch (Exception ex)
                {
                    Outil.Ecrire_Erreur(ex);
                    throw;
                }

            }
        }

        private void Init_02(string csvEntier)
        {

            string[] cmdCsv = csvEntier
                .Split(m_sep1, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x)).ToArray();

            this.m_lesMenuLignes = new List<MenuLigne>();

            if (m_startMenu != 0)
            {
                m_lesMenuLignes.Add(new MenuLigne("Back", "Back"));
            }

            foreach (string cmd in cmdCsv)
            {

                if (string.IsNullOrWhiteSpace(cmd)) continue;

                //try
                //{

                string[] uneLigne = cmd.Split(SEP2);

                MenuLigne ml;


                if (m_startMenu == 0)
                {
                    // menu principal de selection de mainHost vers menu bash
                    ml = new MenuLigne(uneLigne[0], uneLigne[0]);
                }
                else if (m_startMenu == 1)
                {
                    // menu bash
                    ml = new MenuLigne(uneLigne[0], uneLigne[1]);
                }
                else
                {
                    // menu bash
                    ml = new MenuLigne(uneLigne[0], uneLigne[1]);
                }

                m_lesMenuLignes.Add(ml);

                //}
                //catch
                //{
                //    // ignored
                //}

            }

            Console.WriteLine(" nombre de menuLigne : ");
            Console.WriteLine(MenuLignes.Count);
        }

        public Menu(string csvEntier)
        {
            Init_02(csvEntier);
        }

        public Menu(Uri mainHost, string urlCsv)
        {
            this.m_mainHost = mainHost;

            this.m_url_Menu_Host = new Uri(this.m_mainHost + urlCsv);

            string csvEntier = GetStringFromUrl(m_url_Menu_Host);

            Init_02(csvEntier);
        }

        public string VersString()
        {

            StringBuilder r_Sb = new StringBuilder(" liste des commandes disponibles : ");

            foreach (MenuLigne menuLigne in MenuLignes)
            {
                r_Sb.Append(m_rc);
                r_Sb.Append("- ");
                r_Sb.Append(menuLigne.TitreCmd);
            }

            return r_Sb.ToString();
        }

        public void ExecCmd(string cmdUser)
        {

            if (this.m_startMenu == 0)
            {
                this.m_startMenu = 1;
            }

            if ("Back".Equals(cmdUser))
            {
                this.m_startMenu = 0;
            }

            if (this.m_startMenu == 0)
            {
                Init_02(GetStringFromUrl(m_url_Menu_Host));
                this.m_startMenu = 1;
            }
            else if (this.m_startMenu == 1)
            {

                this.m_targetHost = cmdUser;

                MenuLigne menuLigne = MenuLignes.Find(x => x.TitreCmd == cmdUser);

                if (menuLigne != null)
                {
                    Uri uri = new Uri(m_mainHost + URL_GET_BASH + m_targetHost);
                    string reponce = GetStringFromUrl(uri);

                    if (!reponce.StartsWith("<br />\n<font size='1'>"))
                    {
                        // pas d'erreur
                        Init_02(reponce);
                    }
                    else
                    {
                        // erreur
                        Init_02("");
                    }

                }

                this.m_startMenu = 2;
            }
            else
            {

                MenuLigne menuLigne = MenuLignes.Find(x => x.TitreCmd == cmdUser);

                if (menuLigne != null)
                {
                    Uri uri = new Uri(m_mainHost + URL_EXEC_BASH + m_targetHost + "&option=" + cmdUser);
                    GetStringFromUrl(uri);
                }

            }

            Console.WriteLine(" fin ExecCmd " + cmdUser);

        }

        public void Dispose()
        {

        }

    }

}
