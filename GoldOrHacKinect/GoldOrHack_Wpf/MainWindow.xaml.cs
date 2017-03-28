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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kinect_Tools.Cs_Kinect_Tools;
using Microsoft.Samples.Kinect.BodyBasics.WPF;
using Microsoft.Samples.Kinect.DepthBasics.WPF_Tools;

namespace GoldOrHack_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            //InitializeComponent();
            

            try
            {

#if(DEBUG)
                ConsoleManager.Show();
#endif

                Rect rectangle = new Rect(0, 0, 450, 350);
                GoHInterface goHInterface = new GoHInterface(true);

                FailOverWindow failOverWindow = new FailOverWindow(this, goHInterface, rectangle);
                failOverWindow.Show();

            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                Console.WriteLine(err);
                Console.WriteLine(ex.Message);
                throw;
            }


        }
    }
}
