   M S B A M L   `   `   `  ����J  FDepthBasics-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Gclr-namespace:Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.Wpf_Kinect9Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.Wpf_Kinect  5   	   L    FMicrosoft.Samples.Kinect.DepthBasics.Cs_Kinect.Wpf_Kinect.KinectWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   YlocalGclr-namespace:Microsoft.Samples.Kinect.DepthBasics.Cs_Kinect.Wpf_Kinect     5   	     � Title$  KinectWindow��5   	   $	��600��6   $	��700��6+    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  ScreenshotButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName��%a  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   �� $ ScreenshotText��6,   #��z��6D   $
	 Left=�6e   $��Center=�6{   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $
 ScreenshotNormal��6,   $!��Images/ScreenshotNormal.png��6F   $
��None=�6k   $��Center=�6z   �� 5   &   $
 ScreenshotHover��6,   $ ��Images/ScreenshotHover.png��6E   $
��None=�6i   $��Center=�6x   $y�	Collapsed=�6�   5   #   5      5        5!      P� 5"       �@J�IsMouseOver . . �5#   "   �� $ ScreenshotNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5$   "   $ ScreenshotHover��6O    � y�$ Visible=�6?   6O   �� 5%   "   8  $ ScreenshotText��6h    � ��5&      5(      5*      5+      5,      .��5-      � $��	10 0 10 0q�6    � RowDefinitions 5.   
   �� 5/      $
��Auto �6   �� 50      $��* �6   �� 51      $
��Auto �6   �� 52      $
��Auto �6   .+�55   
   �� $��0��6   $��Images\Logo.png��6   $
��Left=�66   $
��Fill=�6Q   $��32��6`   $��81��6l   $��0 10 0 5q�6w   �� 56   
   $��0��6   $��0 0 -1 0q�6!   $��Right=�63   $��Bottom=�6O   #��[  6j   $��Segoe UI)�6�   $��18(�6�   Depth Basics6�   �� 58   
   $��0��6   $��Images/Status.png��6   $
��None=�68   $��Center=�6G   $��0 0 0 5q�6d   (� 5:   
   $��1��6   $��Center=�6   .��5<      �� -   5        $
 ImgMain��5<      $��UniformToFill=�6E   ��6&   �� *ImageSource+6E   5>      �� 5@   
   -   5        $��2��5@      #��[ 6   $��
Screenshot��6H   $
��Auto��6]   $��Right=�6k   $��Center=�6�   $��
10 10 0 10q�6�   6�   �� 5B   
   -   5        $��	statusBar��5B   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5D      �� -   5         ��Name$ LblStatutKindle��5D      .��6.   gnap64   �� 5F      ��6   �� *
StatusText+5G      5I      5M      