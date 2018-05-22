using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CpldUI.GraphCheckUI
{
    /// <summary>
    /// GraphEdit.xaml 的交互逻辑
    /// </summary>
    /// 

    public class PinPoint
    {
        public string PointNo { get; set; }
        public string AliasName { get; set; }
        public string PinControl { get; set; }
    }
    
    public partial class GraphEdit : Window
    {
        private double canvasHeight = 0;
        private double canvasWidth = 0;

        private double left = 0;
        private double top = 0;

        public GraphEdit()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvasHeight = imgPoint.ActualHeight;
            canvasWidth = imgPoint.ActualWidth;
            left =(double) bt1.GetValue(Canvas.LeftProperty);
            top =(double) bt1.GetValue(Canvas.TopProperty);
            
            Console.WriteLine(left);
            Console.WriteLine(top);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<PinPoint> tmpList = new List<PinPoint>();
            tmpList.Add(new PinPoint (){ 
                PointNo = "A1111",
                AliasName = "ttttttt",
                PinControl = "A1111"
            });

            lvPoint.ItemsSource = tmpList;
        }

        private void canvasDraw_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            imgPoint.Height = canvasDraw.ActualHeight;
            imgPoint.Width = canvasDraw.ActualWidth;         
        }

        private void imgPoint_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (canvasHeight != 0 && canvasWidth != 0)
            {
                double leftPos = left / canvasWidth * imgPoint.ActualWidth;
                double topPos = top / canvasHeight * imgPoint.ActualHeight;

                bt1.SetValue(Canvas.LeftProperty, leftPos);
                bt1.SetValue(Canvas.TopProperty, topPos);             
            }
        }

        private void bt1_MouseMove(object sender, MouseEventArgs e)
        {
            Button bt = (Button)sender;

            if ( e.GetPosition(canvasDraw).X >= imgPoint.ActualWidth)
                bt.SetValue(Canvas.LeftProperty, imgPoint.ActualWidth);
            else if (e.GetPosition(canvasDraw).Y >= imgPoint.ActualHeight)
                bt.SetValue(Canvas.TopProperty, imgPoint.ActualHeight);
            else if( e.GetPosition(canvasDraw).X <= 0)
                bt.SetValue(Canvas.LeftProperty, 0.0);
            else if (e.GetPosition(canvasDraw).Y <= 0)
                bt.SetValue(Canvas.TopProperty, 0.0);
            else                   
            {
                bt.SetValue(Canvas.LeftProperty, e.GetPosition(canvasDraw).X);
                bt.SetValue(Canvas.TopProperty, e.GetPosition(canvasDraw).Y);
            }
            
        }

        private void bt1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bt1.PreviewMouseMove+= bt1_MouseMove;
        }

        private void bt1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bt1.PreviewMouseMove -= bt1_MouseMove;
            left = (double)bt1.GetValue(Canvas.LeftProperty) / imgPoint.ActualWidth * canvasWidth;
            top = (double)bt1.GetValue(Canvas.TopProperty) / imgPoint.ActualHeight * canvasHeight;
           
        }
        private static PinImage pi = null;

        private void pin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pi = sender as PinImage;
            pi.PreviewMouseMove += pin_MouseMove;
        }

        private void pin_MouseMove(object sender, MouseEventArgs e)
        {
            //PinImage pi = sender as PinImage;

            Console.WriteLine("mouse move");
            pi.SetValue(Canvas.LeftProperty, e.GetPosition(canvasDraw).X -10);
            pi.SetValue(Canvas.TopProperty, e.GetPosition(canvasDraw).Y -10);
        }

        private void pin_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //PinImage pi = sender as PinImage;

            pi.PreviewMouseMove -= pin_MouseMove;
            left = (double)pi.GetValue(Canvas.LeftProperty) / imgPoint.ActualWidth * canvasWidth;
            top = (double)pi.GetValue(Canvas.TopProperty) / imgPoint.ActualHeight * canvasHeight;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //btnBarEdit.Chi
            left = (double)bt1.GetValue(Canvas.LeftProperty);
            top = (double)bt1.GetValue(Canvas.TopProperty);
            Console.WriteLine(left);
            Console.WriteLine(top);
            Console.WriteLine("size changed");
            Console.WriteLine(imgPoint.Height);
            Console.WriteLine(imgPoint.ActualHeight);
            Console.WriteLine(imgPoint.Width);
            Console.WriteLine(imgPoint.ActualWidth);
            
        }

        private void btnImportBackGround_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = dialog.Filter = "JPG Files (*.jpg)|*.jpeg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpg|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*";
                dialog.Multiselect = false;

                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                imgPoint.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
            }
            catch (Exception ex) 
            {
                CpldLog.LogControl.logError(ex);
                CpldBase.MsgControl.errorMsg(ex.ToString());
            }
        }

        private void canvasDraw_Drop(object sender, DragEventArgs e)
        {
            PinImage test = e.Data.GetData(typeof(PinImage)) as PinImage;


            //Console.WriteLine(test.Parent.GetType());
            Grid testg = test.Parent as Grid;

           // test.ParentGrid = testg;


            testg.Children.Remove(test);
            Console.WriteLine(test.ToolTip);
            Console.WriteLine(test.Name);
            test.ToolTip = "A121";
            test.Uid = "A121";
            test.MouseLeftButtonDown -= bt2_PreviewMouseLeftButtonDown;
            test.AllowDrop = false;

            Console.WriteLine(test.AllowDrop);

            if (test.Parent == null)
            {
                test.SetValue(Canvas.LeftProperty, e.GetPosition(canvasDraw).X);
                test.SetValue(Canvas.TopProperty, e.GetPosition(canvasDraw).Y);
                canvasDraw.Children.Add(test);
            }

        }

        private void bt2_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //PinImage item = sender as PinImage;
            //(item.Parent as Canvas).Children.Remove(item);
            //item.ParentGrid.Children.Add(item);
        }
        private void bt2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            PinImage test = sender as PinImage;

            //Grid tmp = test.Parent as Grid;
            //tmp.Children.Remove(test);

//Console.WriteLine(test.Parent.GetType());


            DragDrop.DoDragDrop(lvPoint, test, DragDropEffects.Move);  
        }
    }
}
