using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Kinect_Tools.Cs_Kinect_Tools;
using Microsoft.Samples.Kinect.BodyBasics.WPF;
using Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.kinect_tools_dir;
using Microsoft.Samples.Kinect.DepthBasics.WPF_Tools;

namespace Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.Wpf_Kinect
{
    /// <summary>
    /// Interaction logic for KinectWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class KinectWindow : Window
    {

        #region constructor

        private readonly KinectTools m_kinectTools;

        private readonly FailOverWindow m_failOverWindow;

        //public KinectWindow()
        //{
        //    InitializeComponent();
        //}

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public KinectWindow(Window window)
        {
            
#if (DEBUG)
            ConsoleManager.Show();
#endif
            
            // use the window object as the view model in this simple example
            this.DataContext = this;

            // initialize the components (controls) of the window
            this.InitializeComponent();


            Rect rectangle = new Rect(0, 0, 450, 350);

            GoHInterface goHInterface = new GoHInterface(true);

            this.m_kinectTools = new KinectTools(goHInterface);
            this.m_kinectTools.PropertyChanged += KinectTools_PropertyChanged;
            this.m_kinectTools.Init_01();

            this.m_kinectTools.Refresh += KinectTools_Refresh;

            this.m_failOverWindow = new FailOverWindow(this, goHInterface, rectangle);

            window.Hide();

        }

        private void KinectTools_Refresh(RefreshEventArgs e)
        {
            this.ImageSource = this.m_kinectTools.ImageRenvoyer;
            this.ImgMain.Source = this.ImageSource;
        }

        private void KinectTools_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string value = this.m_kinectTools.StatusText;

            if (m_failOverWindow != null && value == Properties.Resources.RunningStatusText)
            {
                m_failOverWindow.Hide();
                this.Show();
            }

            if (m_failOverWindow != null && value == Properties.Resources.SensorNotAvailableStatusText)
            {
                this.Hide();
                m_failOverWindow.Show();
            }

            this.LblStatutKindle.Content = value;
        }

        #endregion constructor

        #region prop

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource { get; set; }

        #endregion prop

        #region event

        ///// <summary>
        ///// Execute shutdown tasks
        ///// </summary>
        ///// <param name="sender">object sending the event</param>
        ///// <param name="e">event arguments</param>
        //private void MainWindow_Closing(object sender, CancelEventArgs e)
        //{
        //    this.m_kinectTools.Dispose();
        //}

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            this.m_kinectTools.Screenshot();
        }

        #endregion event

        #region properties

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        #endregion properties

    }
}
