using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GoldOrHack_Wpf.Resources;
using Kinect_Tools.Cs_Kinect_Tools;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
// ReSharper disable PossibleNullReferenceException

// ReSharper disable once CheckNamespace
namespace Microsoft.Samples.Kinect.DepthBasics.WPF_Tools
{

    public class GoHInterface : IDisposable
    {

        #region const

        //private const string CONFIG_FOLDER =
        //    @"D:\felix_user\documents\_01_Codes_Sources_Felix\_02_Visual_Studio\Kinect\_01_Kinect_Project\_01_Config_Gold_Or_Hack\";

        //private const string CONFIG_FOLDER =
        //    @"..\..\..\";

        private const string CONFIG_NAME = @"config.txt";

        //private const string IMG_HAND = CONFIG_FOLDER + @"img\main_01.png";
        //private const string IMG_HAND_FIST = CONFIG_FOLDER + @"img\fist.jpg";
        //private const string IMG_BACKGROUND = CONFIG_FOLDER + @"img\kinect.jpg";


        //private static string GetFilePath(string fileName)
        //{

        //    if (File.Exists(fileName))
        //    {
        //        return fileName;
        //    }

        //    if (File.Exists(CONFIG_FOLDER + fileName))
        //    {
        //        return CONFIG_FOLDER + fileName;
        //    }

        //    throw new ApplicationException(CONFIG_FOLDER + fileName);
        //}

        private const int X_DECALAGE_TEXT_VERS_DROITE = 30;
        private const int Y_DECALAGE_TEXT_VERS_BAS = 60;

        private const int X_DECALAGE_TEXT_VERS_GAUCHE = 160;
        private const int Y_DECALAGE_TEXT_VERS_HAUT_02 = 10;
        private const int Y_DECALAGE_TEXT_VERS_HAUT_03 = 85;

        private const int X_TAILLE = 100;
        private const int Y_TAILLE = 50;
        private const int X_DECALAGE_BACK = 50 + X_TAILLE / 2;
        private const int Y_DECALAGE_BACK = 50 + Y_TAILLE / 2;

        #endregion

        #region member

        //private readonly bool m_isBackground;

        public bool IsBackground { get; set; }

        private const double HAND_SIZE = 30;

        private readonly ImageSource m_img;

        private readonly I_ClientGoH m_connection;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private static readonly Brush s_HandClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private static readonly Brush s_HandOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private static readonly Brush s_HandLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        //private static readonly Brush s_TrackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        //private static readonly Brush s_InferredJointBrush = Brushes.Yellow;


        private readonly Point m_center;
        private readonly Pen m_pen;
        private readonly int m_radiusX;
        private readonly int m_radiusY;
        private readonly Typeface m_typeface;

        private readonly SolidColorBrush m_noir_Transparent;
        private readonly ImageSource m_hand_Transparent_Img;
        private readonly ImageSource m_fist_Transparent_Img;
        private BackgroundWorker m_bw;

        //private event refreshEvent ;

        public ConcurrentBag<string> LaListeDesCommandes { get; set; }

        #endregion member

        #region constructeur

        public FormattedText GetFormattedText(string msg)
        {
            return new FormattedText(msg, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, m_typeface, 25.0, Brushes.White);
        }

        private void RefreshMenu()
        {
            if (!this.m_connection.IsBusy())
            {
                string[] ts = m_connection.GetMenu();

                this.LaListeDesCommandes = new ConcurrentBag<string>(Inverser(ts));
            }
            else
            {
                this.LaListeDesCommandes = new ConcurrentBag<string>();
            }
        }


        private static ImageSource Get_Img_From_Bitmap(Bitmap bitmap01)
        {
            IntPtr handle = bitmap01.GetHbitmap();

            ImageSource img02 = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return img02;
        }


        public GoHInterface(bool isBackground)
        {

            this.LaListeDesCommandes = new ConcurrentBag<string>();

            this.IsBackground = isBackground;

            this.m_noir_Transparent = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));

            try
            {
                this.m_img = Get_Img_From_Bitmap(Resource1.kinect);
                this.m_hand_Transparent_Img = Get_Img_From_Bitmap(Resource1.hand_transparent);
                this.m_fist_Transparent_Img = Get_Img_From_Bitmap(Resource1.fist_transparent);
            }
            catch (Exception ex)
            {
                Outil.Ecrire_Erreur(ex);

                throw;
            }



            string[] ts_Lines;

            string filePath;

            try
            {


                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly == null)
                    throw new Exception(" assembly == null ");

                string codeBase = assembly.CodeBase;

                DirectoryInfo exeFolder_Di = new FileInfo(new Uri(codeBase).LocalPath).Directory;

                FileInfo fi_02 = new FileInfo(exeFolder_Di.Parent.Parent.Parent.FullName + @"\" + CONFIG_NAME);

                if (File.Exists(CONFIG_NAME))
                {
                    filePath = CONFIG_NAME;
                }
                else if (fi_02.Exists)
                {
                    filePath = fi_02.FullName;
                }
                else
                {
                    throw new ApplicationException(" Erreur : pas de fichier " + CONFIG_NAME);
                }

                Console.WriteLine(@" fichier de config : " + filePath);
                ts_Lines = File.ReadAllLines(filePath);

            }
            catch (Exception ex)
            {
                Outil.Ecrire_Erreur(ex);
                throw;
            }


            if (string.IsNullOrWhiteSpace(ts_Lines[0]))
            {
                Console.WriteLine(@" Pas de parametre dans le fichier : ");
                Console.WriteLine(filePath);

                return;
            }
            else if (ts_Lines[0].StartsWith(@"http://"))
            {
                this.m_connection = new ClientGoHWeb(this.LaListeDesCommandes, ts_Lines[0]);
            }
            else
            {
                this.m_connection = new ClientGoHSsh(this.LaListeDesCommandes, filePath);
            }


            RefreshMenu();


            //Return the typeface collection for the fonts in the selected URI location.
            using (IEnumerator<Typeface> t = Fonts.GetTypefaces("file:///C:\\Windows\\Fonts\\#Georgia").GetEnumerator())
            {
                t.MoveNext();
                this.m_typeface = t.Current;
            }

            this.m_center = new Point(X_DECALAGE_BACK, Y_DECALAGE_BACK);
            this.m_pen = new Pen(Brushes.White, 1);
            this.m_radiusX = X_TAILLE;
            this.m_radiusY = Y_TAILLE;

        }

        public void Dispose()
        {
            this.m_connection.Dispose();
        }

        ~GoHInterface()
        {
            this.Dispose();
        }

        #endregion

        #region fonction

        private static T GetAt<T>(T[] tableDeT, int i)
        {
            T r;

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (tableDeT.Length > i)
            {
                r = tableDeT[i];
            }
            else
            {
                r = tableDeT[0];
            }

            return r;
        }

        private static T GetAt<T>(IEnumerable<T> enu, int i)
        {
            return GetAt(enu.ToArray(), i);
        }


        public void AddVisualInterface(DrawingContext dc, Rect zoneDessinRectangle)
        {

            //Console.WriteLine(@" debut AddVisualInterface ");

            RefreshMenu();

            if (this.IsBackground)
            {
                // dessine le contour
                dc.DrawRectangle(Brushes.Black, this.m_pen, zoneDessinRectangle);
                dc.DrawImage(m_img, zoneDessinRectangle);
            }

            if (this.LaListeDesCommandes.Count >= 1)
            {

                int haut = (int)m_center.Y;
                int mi_Hauteur = (int)(zoneDessinRectangle.Height / 2);
                int bas = (int)(zoneDessinRectangle.Height - m_center.Y);
                int gauche = (int)m_center.X;
                int droite = (int)(zoneDessinRectangle.Width - m_center.X);




                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(gauche, haut), this.m_radiusX, this.m_radiusY);
                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(droite, haut), this.m_radiusX, this.m_radiusY);
                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(gauche, mi_Hauteur), this.m_radiusX, this.m_radiusY);
                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(droite, mi_Hauteur), this.m_radiusX, this.m_radiusY);
                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(gauche, bas), this.m_radiusX, this.m_radiusY);
                dc.DrawEllipse(m_noir_Transparent, this.m_pen, new Point(droite, bas), this.m_radiusX, this.m_radiusY);



                FormattedText[] t_FormattedTexts = this.LaListeDesCommandes.Select(GetFormattedText).ToArray();

                haut = Y_DECALAGE_TEXT_VERS_BAS;
                mi_Hauteur = (int)(zoneDessinRectangle.Height / 2 - Y_DECALAGE_TEXT_VERS_HAUT_02);
                bas = (int)(zoneDessinRectangle.Height - Y_DECALAGE_TEXT_VERS_HAUT_03);
                gauche = X_DECALAGE_TEXT_VERS_DROITE;
                droite = (int)(zoneDessinRectangle.Width - X_DECALAGE_TEXT_VERS_GAUCHE);

                //FormattedText s0 = GetAt(t_FormattedTexts, 0);
                //FormattedText s5 = GetAt(t_FormattedTexts, 5);

                dc.DrawText(GetAt(t_FormattedTexts, 0), new Point(gauche, haut));
                dc.DrawText(GetAt(t_FormattedTexts, 1), new Point(droite, haut));
                dc.DrawText(GetAt(t_FormattedTexts, 2), new Point(gauche, mi_Hauteur));
                dc.DrawText(GetAt(t_FormattedTexts, 3), new Point(droite, mi_Hauteur));
                dc.DrawText(GetAt(t_FormattedTexts, 4), new Point(gauche, bas));
                dc.DrawText(GetAt(t_FormattedTexts, 5), new Point(droite, bas));

            }
            else
            {
                // ne pas afficher de bouton
            }

            //Console.WriteLine(@" fin AddVisualInterface ");
        }

        private static T[] Inverser<T>(IEnumerable<T> laListeDesCommandes)
        {
            T[] ts_01 = laListeDesCommandes.ToArray();

            T[] ts_02 = new T[ts_01.Length];

            for (int i = 0; i < ts_01.Length; i++)
            {
                ts_02[ts_01.Length - i - 1] = ts_01[i];
            }

            return ts_02;
        }

        #endregion fonction

        #region main

        public void AddMainOuverte(Point handPosition, DrawingContext dc, int xMax, int yMax)
        {

            const int marge = 32;

            double xMax2 = xMax - marge;

            double yMax2 = yMax - marge;

            // si ???
            if (handPosition.X >= marge && handPosition.Y >= marge && handPosition.X <= xMax2 && handPosition.Y <= yMax2)
            {

                dc.DrawEllipse(s_HandOpenBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

                Point pxy = new Point(handPosition.X - 12, handPosition.Y - 12);

                Rect rectangle = new Rect(pxy, new Size(25, 25));

                dc.DrawImage(m_hand_Transparent_Img, rectangle);

            }
            else
            {
                //Console.WriteLine(@" mouse out-scope ");
            }

        }

        public void AddMainOuverte(Point handPosition, DrawingContext dc, double xMax, double yMax)
        {
            AddMainOuverte(handPosition, dc, (int)xMax, (int)yMax);
        }

        public BackgroundWorker AddMainFermer(Point handPosition, DrawingContext dc, Rect zoneDessinRectangle)
        {

            try
            {

                dc.DrawEllipse(GoHInterface.s_HandClosedBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

                Point pxy = new Point(handPosition.X - 12, handPosition.Y - 12);

                Rect rectangle = new Rect(pxy, new Size(25, 25));

                dc.DrawImage(m_fist_Transparent_Img, rectangle);

                if (this.LaListeDesCommandes.Count >= 1)
                {

                    int haut = (int)(zoneDessinRectangle.Height * (1.0 / 3.0));
                    int bas = (int)(zoneDessinRectangle.Height * (2.0 / 3.0));

                    int milieu_X = (int)(zoneDessinRectangle.Width * (1.0 / 2.0));

                    //string msg;

                    //if (!(handPosition.X < (m_rectangle.Width / 2)))
                    //    msg = (handPosition.Y < (m_rectangle.Height / 2) ? "haut droite" : "bas droite");
                    //else
                    //    msg = (handPosition.Y < (m_rectangle.Height / 2) ? "haut gauche" : "bas gauche");


                    int num_Instruction = 0;
                    string msg_01;

                    if (handPosition.Y < haut)
                    {
                        msg_01 = "haut";
                        num_Instruction += 0;
                    }
                    else if (handPosition.Y < bas)
                    {
                        msg_01 = "milieu";
                        num_Instruction += 2;
                    }
                    else
                    {
                        msg_01 = "bas";
                        num_Instruction += 4;
                    }

                    string msg_02 =
                        (handPosition.X < milieu_X)
                        ? "gauche"
                        : "droite";

                    num_Instruction +=
                        (handPosition.X < milieu_X)
                        ? 0
                        : 1;

                    string clickPositionString = msg_01 + " " + msg_02 + " click ";

                    //Console.WriteLine(clickPositionString);

                    string cmdTitle = GetAt(this.LaListeDesCommandes, num_Instruction);

                    this.m_bw = this.m_connection.ExecuteFromTitreCmd_And_Msg(cmdTitle, clickPositionString);

                    m_bw.RunWorkerCompleted += M_bw_RunWorkerCompleted;

                    //this.m_connection.Ecrire();

                    //this.m_connection.Ecrire(msg + " R " + handPosition);

                    //this.m_connection.Ecrire("R");

                }
                else
                {
                    // ne pas afficher
                }

                RefreshMenu();

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            
            return this.m_bw;
        }

        private void M_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshMenu();
        }

        public void AddMainLasso(Point handPosition, DrawingContext dc)
        {

            //this.m_connection.Ecrire("L");

            dc.DrawEllipse(GoHInterface.s_HandLassoBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

            Point pxy = new Point(handPosition.X - 12, handPosition.Y - 12);

            Rect rectangle = new Rect(pxy, new Size(25, 25));

            dc.DrawImage(m_fist_Transparent_Img, rectangle);

        }

        #endregion main

        //public BackgroundWorker GetBw()
        //{
        //    return this.m_connection.GetBw();
        //}

    }
}
