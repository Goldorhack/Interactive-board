//using System.ComponentModel;
//using System.Runtime.CompilerServices;

//namespace Kinect_Tools.kinect_tools_dir
//{

//    // This is a simple customer class that 
//    // implements the IPropertyChange interface.
//    public class TestINotify_V1 : INotifyPropertyChanged
//    {

//        // These fields hold the values for the public properties.
//        //private Guid idValue = Guid.NewGuid();

//        private string m_customerNameValue = string.Empty;
//        private string m_phoneNumberValue = string.Empty;

//        public event PropertyChangedEventHandler PropertyChanged;

//        // This method is called by the Set accessor of each property.
//        // The CallerMemberName attribute that is applied to the optional propertyName
//        // parameter causes the property name of the caller to be substituted as an argument.
//        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
//            }

//        }

//        // The constructor is private to enforce the factory pattern.
//        //private TestINotify_V2()
//        //{
//        //    customerNameValue = "Customer";
//        //    phoneNumberValue = "(312)555-0100";
//        //}

//        // This is the public factory method.
//        public static TestINotify_V1 CreateNewCustomer()
//        {
//            return new TestINotify_V1();
//        }

//        // This property represents an ID, suitable
//        // for use as a primary key in a database.
//        //public Guid ID
//        //{
//        //    get
//        //    {
//        //        return this.idValue;
//        //    }
//        //}

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

//    }

//}
