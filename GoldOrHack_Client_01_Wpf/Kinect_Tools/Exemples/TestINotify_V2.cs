//using System.ComponentModel;
//using System.Runtime.CompilerServices;

//namespace Kinect_Tools.kinect_tools_dir
//{

//    public class TestINotify_V2 : INotifyPropertyChanged
//    {

//        #region propf

//        private string m_customerNameValue;
//        private string m_phoneNumberValue;

//        public string CustomerName
//        {

//            get
//            {
//                return this.m_customerNameValue;
//            }

//            set
//            {
//                if (value != this.m_customerNameValue)
//                {
//                    this.m_customerNameValue = value;
//                    NotifyPropertyChanged();
//                }
//            }

//        }

//        public string PhoneNumber
//        {

//            get
//            {
//                return this.m_phoneNumberValue;
//            }

//            set
//            {
//                if (value != this.m_phoneNumberValue)
//                {
//                    this.m_phoneNumberValue = value;
//                    NotifyPropertyChanged();
//                }
//            }

//        }

//        #endregion propf

//        #region INotifyPropertyChanged

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        #endregion INotifyPropertyChanged

//    }

//}
