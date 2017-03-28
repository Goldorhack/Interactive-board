using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Tools.kinect_tools_dir
{
    public class ClientGoHWeb : I_ClientGoH
    {

        //private ConcurrentBag<string> m_laListeDesCommandes;

        private readonly Uri m_uri_Racine;

        //private string m_targetHost = "";
        private Menu m_leMenu;

        public ClientGoHWeb(ConcurrentBag<string> laListeDesCommandes, string url_Racine)
        {

            //this.m_laListeDesCommandes = laListeDesCommandes;

            this.m_uri_Racine = new Uri(url_Racine);

        }

        public void Dispose()
        {

        }

        public void Ecrire(string s)
        {
            throw new NotImplementedException();
        }

        private void RefreshMenu()
        {

            //string[] ts =
            //{
            //    "haut gauche",
            //    "haut droite",
            //    "milieu gauche",
            //    "milieu droite",
            //    "bas gauche",
            //    "bas droite",
            //};

            if (this.m_leMenu == null)
            {
                this.m_leMenu = new Menu(m_uri_Racine, @"ssh/GetSshList.php");
            }

            //string[] r = this.m_leMenu.MenuLignes.Select(x => x.TitreCmd).ToArray();

            //m_laListeDesCommandes = new ConcurrentBag<string>(r);

            //return;
        }

        public string[] GetMenu()
        {
            RefreshMenu();
            string[] r = this.m_leMenu.MenuLignes.Select(x => x.TitreCmd).ToArray();
            return r;

        }

        public void ExecuteFromTitreCmd(string titreCmd)
        {
            m_leMenu.ExecCmd(titreCmd);
            RefreshMenu();
        }
    }
}
