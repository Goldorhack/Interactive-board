using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GoldOrHack_Wpf;
using Microsoft.Samples.Kinect.DepthBasics;
using Microsoft.Samples.Kinect.DepthBasics.tools;
using Microsoft.Samples.Kinect.DepthBasics.WPF_Tools;

// ReSharper disable once CheckNamespace
namespace Microsoft.Samples.Kinect.BodyBasics.WPF
{
    /// <summary>
    /// Interaction logic for FailOverWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class FailOverWindow : Window
    {


        private readonly GoHInterface m_goHInterface;


        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource { get; set; }


        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private readonly DrawingGroup m_drawingGroup;

        private BackgroundWorker m_bw;
        //private readonly MainWindow m_mainWindow;

        public Rect ZoneDessinRectangle { get; set; }

        private void Refresh()
        {

            using (DrawingContext dc = this.m_drawingGroup.Open())
            {
                m_goHInterface.AddVisualInterface(dc, this.ZoneDessinRectangle);
            }

            this.ImgMain.Source = this.ImageSource;
        }

        private void ImgMain_MouseMove(object sender, MouseEventArgs e)
        {

            try
            {

                Refresh();


                //Console.WriteLine(@" debut moseMove ");

                UIElement element = sender as UIElement;

                if (element != null)
                {

                    UIElement ele = element;

                    MouseDevice mouse = e.MouseDevice;
                    Point point = mouse.GetPosition(ele);


                    //Console.WriteLine(@" emouse.GetPosition : " + point);


                    using (DrawingContext dc = this.m_drawingGroup.Open())
                    {
                        m_goHInterface.AddVisualInterface(dc, this.ZoneDessinRectangle);

                        if (mouse.LeftButton.Equals(MouseButtonState.Pressed))
                        {
                            this.m_bw = m_goHInterface.AddMainFermer(point, dc, this.ZoneDessinRectangle);
                            this.m_bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                        }
                        else
                        {
                            m_goHInterface.AddMainOuverte(point, dc, ImageSource.Width, ImageSource.Height);
                        }

                    }

                    this.ImgMain.Source = this.ImageSource;
                }
                else
                {
                    Console.WriteLine(@" element <=> null ");
                }

                //Console.WriteLine(@" fin mouseMove ");

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }

        public FailOverWindow(Window mainWindow, GoHInterface goHInterface, Rect zoneDessinRectangle)
        {

            //this.ImgLogo.Source = new BitmapImage(new Uri(@"..\Images\Logo.png"));
            //this.ImgStatus.Source = new BitmapImage(new Uri(@"..\Images\Status.png"));


            this.ZoneDessinRectangle = zoneDessinRectangle;

            goHInterface.IsBackground = true;

            this.m_goHInterface = goHInterface;

            //this.m_mainWindow = mainWindow;

            InitializeComponent();

            // Create the drawing group we'll use for drawing
            this.m_drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.ImageSource = new DrawingImage(this.m_drawingGroup);

            using (DrawingContext dc = this.m_drawingGroup.Open())
            {
                m_goHInterface.AddVisualInterface(dc, this.ZoneDessinRectangle);
            }

            Refresh();

            this.ImgMain.MouseMove += ImgMain_MouseMove;
            this.ImgMain.MouseDown += ImgMain_MouseMove;
            this.ImgMain.MouseUp += ImgMain_MouseMove;
            
            mainWindow.Hide();

        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(@" Bw_RunWorkerCompleted ");
            this.Refresh();
        }

        //public BackgroundWorker GetBw()
        //{
        //    if (this.m_goHInterface != null)
        //    {
        //        return this.m_goHInterface.GetBw();
        //    }

        //    return null;
        //}

        ~FailOverWindow()
        {
            this.m_goHInterface.Dispose();
        }

    }
}
