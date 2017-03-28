namespace Kinect_Tools.kinect_tools_dir
{

    public class MenuLigne
    {

        public string TitreCmd { get; set; }

        public string UrlCommandeWeb { get; set; }

        public MenuLigne(string titreCmd, string urlCommandeWeb)
        {
            this.TitreCmd = titreCmd;
            this.UrlCommandeWeb = urlCommandeWeb;
        }

    }

}
