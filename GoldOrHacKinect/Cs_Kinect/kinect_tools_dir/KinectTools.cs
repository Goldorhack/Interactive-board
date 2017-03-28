using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.DepthBasics.WPF_Tools;

namespace Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.kinect_tools_dir
{

    public class KinectTools : IDisposable, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public event RefreshEventHandler Refresh;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private void NotifyRefresh()
        {
            if (Refresh != null)
            {
                Refresh.Invoke(new RefreshEventArgs(""));
            }
        }

        #endregion INotifyPropertyChanged

        #region members

        private readonly GoHInterface m_goHInterface;

        /// <summary>
        /// Map depth range to byte range
        /// </summary>
        private const int MAP_DEPTH_TO_BYTE = 8000 / 256;


        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HAND_SIZE = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JOINT_THICKNESS = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private const double CLIP_BOUNDS_THICKNESS = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float INFERRED_Z_POSITION_CLAMP = 0.1f;


        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush m_handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush m_handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush m_handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush m_trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush m_inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen m_inferredBonePen = new Pen(Brushes.Gray, 1);


        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor m_kinectSensor = null;

        /// <summary>
        /// Reader for depth frames
        /// </summary>
        private DepthFrameReader m_depthFrameReader = null;

        /// <summary>
        /// Description of the data contained in the depth frame
        /// </summary>
        private FrameDescription m_depthFrameDescription = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap m_depthBitmap = null;

        /// <summary>
        /// Intermediate storage for frame data converted to color
        /// </summary>
        private byte[] m_depthPixels = null;


        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper m_coordinateMapper = null;


        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader m_bodyFrameReader = null;


        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] m_bodies = null;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> m_bones;


        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> m_bodyColors;


        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup m_drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage m_imageSource;


        private string m_statusText = "";

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get { return this.m_statusText; }

            set
            {
                if (this.m_statusText != value)
                {
                    this.m_statusText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ImageSource ImageRenvoyer { get; set; }

        //private bool m_isBody;
        //private bool m_isDepthFrameProcessed;
        private int m_displayWidth;
        private int m_displayHeight;
        private Rect m_rect;

        #endregion members

        #region Refresh01

        private void Refresh01()
        {

            //if (isBody && isDepthFrameProcessed)
            //{

            //this.ImageRenvoyer = this.imageSource;
            this.ImageRenvoyer = this.m_imageSource;

            NotifyRefresh();

            //m_isDepthFrameProcessed = false;
            //m_isBody = false;

            // create the bitmap to display
            this.m_depthBitmap = new WriteableBitmap(this.m_depthFrameDescription.Width, this.m_depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);

            //}

        }

        #endregion Refresh01

        #region constructor

        public void Init_01()
        {
            //recuperre 
            //le kinect
            //le detecteur de profondeur
            //branche le detecteur de profondeur


            // get the kinectSensor object
            this.m_kinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.m_coordinateMapper = this.m_kinectSensor.CoordinateMapper;


            // open the reader for the depth frames
            this.m_depthFrameReader = this.m_kinectSensor.DepthFrameSource.OpenReader();

            // wire handler for frame arrival
            this.m_depthFrameReader.FrameArrived += this.Reader_FrameArrived;

            // get FrameDescription from DepthFrameSource
            this.m_depthFrameDescription = this.m_kinectSensor.DepthFrameSource.FrameDescription;

            // allocate space to put the pixels being received and converted
            this.m_depthPixels = new byte[this.m_depthFrameDescription.Width * this.m_depthFrameDescription.Height];

            this.m_rect = new Rect(0, 0, this.m_depthFrameDescription.Width, this.m_depthFrameDescription.Height);

            // create the bitmap to display
            this.m_depthBitmap = new WriteableBitmap((int)m_rect.Width, (int)m_rect.Height, 96.0, 96.0, PixelFormats.Gray8, null);

            // set IsAvailableChanged event notifier
            this.m_kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.m_kinectSensor.Open();

            // set the status text
            this.StatusText = this.m_kinectSensor.IsAvailable
                ? Properties.Resources.RunningStatusText
                : Properties.Resources.NoSensorStatusText;


            // Create the drawing group we'll use for drawing
            this.m_drawingGroup = new DrawingGroup();


            // get the depth (display) extents
            FrameDescription frameDescription = this.m_kinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            this.m_displayWidth = frameDescription.Width;
            this.m_displayHeight = frameDescription.Height;


            // open the reader for the body frames
            this.m_bodyFrameReader = this.m_kinectSensor.BodyFrameSource.OpenReader();

            // a bone defined as a line between two joints
            this.m_bones = new List<Tuple<JointType, JointType>>
            {
                new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight),
                new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft)
            };



            // populate body colors, one for each BodyIndex
            this.m_bodyColors = new List<Pen>
            {
                new Pen(Brushes.Red, 6),
                new Pen(Brushes.Orange, 6),
                new Pen(Brushes.Green, 6),
                new Pen(Brushes.Blue, 6),
                new Pen(Brushes.Indigo, 6),
                new Pen(Brushes.Violet, 6)
            };



            // Create the drawing group we'll use for drawing
            this.m_drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.m_imageSource = new DrawingImage(this.m_drawingGroup);

            this.m_bodyFrameReader.FrameArrived += this.Reader_FrameArrived;


        }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public KinectTools(GoHInterface goHInterface)
        {
            this.m_goHInterface = goHInterface;
        }



        #endregion constructor

        #region event


        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext)
        {


            switch (handState)
            {

                case HandState.Closed:
                    drawingContext.DrawEllipse(this.m_handClosedBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

                    m_goHInterface.AddMainFermer(handPosition, drawingContext, this.m_rect);

                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.m_handOpenBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

                    m_goHInterface.AddMainOuverte(handPosition, drawingContext, this.m_displayWidth, this.m_displayHeight);

                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.m_handLassoBrush, null, handPosition, HAND_SIZE, HAND_SIZE);

                    m_goHInterface.AddMainLasso(handPosition, drawingContext);

                    break;

            }



        }


        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.m_inferredBonePen;

            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }


        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            foreach (Tuple<JointType, JointType> bone in this.m_bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.m_trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.m_inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JOINT_THICKNESS, JOINT_THICKNESS);
                }
            }
        }



        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.m_bodies == null)
                    {
                        this.m_bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.m_bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {



                using (DrawingContext dc = this.m_drawingGroup.Open())
                {

                    Rect zoneDessinRectangle = new Rect(0.0, 0.0, this.m_displayWidth, this.m_displayHeight);

                    // Draw a transparent background to set the render size
                    dc.DrawRectangle(Brushes.Black, null, zoneDessinRectangle);

                    dc.DrawImage(this.m_depthBitmap, zoneDessinRectangle);

                    this.m_goHInterface.IsBackground = false;
                    this.m_goHInterface.AddVisualInterface(dc, zoneDessinRectangle);

                    int penIndex = 0;

                    foreach (Body body in this.m_bodies)
                    {

                        Pen drawPen = this.m_bodyColors[penIndex++];

                        if (body.IsTracked)
                        {

                            //this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            //convert the joint points to depth (display)space

                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = INFERRED_Z_POSITION_CLAMP;
                                }

                                DepthSpacePoint depthSpacePoint = this.m_coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }

                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }
                    }

                    // prevent drawing outside of our render area
                    this.m_drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.m_displayWidth, this.m_displayHeight));
                }
            }


            //m_isBody = true;


            Refresh01();
        }


        /// <summary>
        /// Handles the depth frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            TraitementFrameDeProfondeur(e, this.m_depthFrameDescription, this.m_depthBitmap);

            //this.m_isDepthFrameProcessed = true;

            this.RenderDepthPixels();

        }

        private void TraitementFrameDeProfondeur(DepthFrameArrivedEventArgs e, FrameDescription depthFrameDescription,
            WriteableBitmap depthBitmap)
        {

            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {

                if (depthFrame != null)
                {

                    // the fastest way to process the body index data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                    {

                        // verify data and write the color data to the display bitmap
                        if (((depthFrameDescription.Width * depthFrameDescription.Height) ==
                             (depthBuffer.Size / depthFrameDescription.BytesPerPixel)) &&
                            (depthFrameDescription.Width == depthBitmap.PixelWidth) &&
                            (depthFrameDescription.Height == depthBitmap.PixelHeight))
                        {
                            // Note: In order to see the full range of depth (including the less reliable far field depth)
                            // we are setting maxDepth to the extreme potential depth threshold
                            const ushort maxDepth = ushort.MaxValue;

                            // If you wish to filter by reliable depth distance, uncomment the following line:
                            //// maxDepth = depthFrame.DepthMaxReliableDistance

                            this.ProcessDepthFrameData(depthBuffer.UnderlyingBuffer, depthBuffer.Size,
                                depthFrame.DepthMinReliableDistance, maxDepth);
                        }
                    }
                }
            }



        }

        /// <summary>
        /// Directly accesses the underlying image buffer of the DepthFrame to 
        /// create a displayable bitmap.
        /// This function requires the /unsafe compiler option as we make use of direct
        /// access to the native memory pointed to by the depthFrameData pointer.
        /// </summary>
        /// <param name="depthFrameData">Pointer to the DepthFrame image data</param>
        /// <param name="depthFrameDataSize">Size of the DepthFrame image data</param>
        /// <param name="minDepth">The minimum reliable depth value for the frame</param>
        /// <param name="maxDepth">The maximum reliable depth value for the frame</param>
        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth,
            ushort maxDepth)
        {
            // depth frame data is a 16 bit value
            ushort* frameData = (ushort*)depthFrameData;

            // convert depth to a visual representation
            for (int i = 0; i < (int)(depthFrameDataSize / this.m_depthFrameDescription.BytesPerPixel); ++i)
            {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                this.m_depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MAP_DEPTH_TO_BYTE) : 0);
            }
        }

        /// <summary>
        /// Renders color pixels into the writeableBitmap.
        /// </summary>
        private void RenderDepthPixels()
        {

            Int32Rect deepImg = new Int32Rect(0, 0, this.m_depthBitmap.PixelWidth, this.m_depthBitmap.PixelHeight);

            this.m_depthBitmap.WritePixels(
                deepImg,
                this.m_depthPixels,
                this.m_depthBitmap.PixelWidth,
                0);

        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            if (this.m_kinectSensor.IsAvailable)
            {
                string statusTest = Properties.Resources.RunningStatusText;
                this.StatusText = statusTest;
            }
            else
            {
                string statusTest = Properties.Resources.SensorNotAvailableStatusText;
                this.StatusText = statusTest;
            }
        }

        #endregion event

        #region function

        internal void Screenshot()
        {
            if (this.m_depthBitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.m_depthBitmap));

                string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss",
                    CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = Path.Combine(myPhotos, "KinectScreenshot-Depth-" + time + ".png");

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.StatusText = string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.SavedScreenshotStatusTextFormat, path);
                }
                catch (IOException)
                {
                    this.StatusText = string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }
        }

        public void Dispose()
        {
            if (this.m_depthFrameReader != null)
            {
                // DepthFrameReader is IDisposable
                this.m_depthFrameReader.Dispose();
                this.m_depthFrameReader = null;
            }

            if (this.m_kinectSensor != null)
            {
                this.m_kinectSensor.Close();
                this.m_kinectSensor = null;
            }

            m_goHInterface.Dispose();

        }

        #endregion function
    }
}