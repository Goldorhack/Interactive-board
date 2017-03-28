using System;
using System.Collections.Generic;
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
using GoldOrHack_Client_01_Wpf;
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
        //private readonly MainWindow m_mainWindow;

        public Rect ZoneDessinRectangle { get; set; }

        private void Refresh()
        {
            this.ImgMain.Source = this.ImageSource;
        }

        private void ImgMain_MouseMove(object sender, MouseEventArgs e)
        {

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
                        m_goHInterface.AddMainFermer(point, dc, this.ZoneDessinRectangle);
                    }
                    else
                    {
                        m_goHInterface.AddMainOuverte(point, dc, ImageSource.Width, ImageSource.Height);
                    }

                }
            }
            else
            {
                Console.WriteLine(@" element <=> null ");
            }

            Refresh();


            //Console.WriteLine(@" fin mouseMove ");

        }

        public FailOverWindow(MainWindow mainWindow, GoHInterface goHInterface, Rect zoneDessinRectangle)
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
        
        ~FailOverWindow()
        {
            this.m_goHInterface.Dispose();
        }

    }
}
