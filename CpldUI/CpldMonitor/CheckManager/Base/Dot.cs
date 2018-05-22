using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CpldDB;
using CpldUI.Check.GraphCheck;

namespace CpldUI.Check.Base
{
    public class Dot
    {
        public Canvas DotCanvas { get; set; }
        public Circuit InCircuit { get; set; }
        public Canvas ParentCanvas { get; set; }
        public DotInfo Info { get; set; }

        public delegate void RemoveDotEventHandler(Dot dot);
        public delegate void MovingDotEventHandler(Dot dot, Point position);
        public delegate void MoveDotEndEventHandler(Dot dot);
        public delegate void EmptyDotEventHandler(Dot dot);
        public delegate void CalibrationDotEndEventHandler(Dot dot);
        
        public event CalibrationDotEndEventHandler CalibrationDotEvent;
        public event RemoveDotEventHandler RemoveDotEvent;
        public event MovingDotEventHandler MovingDotEvent;
        public event MoveDotEndEventHandler MoveDotEndEvent;
        public event EmptyDotEventHandler EmptyDotEvent;
        
        public Dot(DotInfo dot, Circuit circuit, Canvas parentCanvas, double scale)
        {
            if (dot == null) 
            {
                CpldLog.LogControl.LogError("创建点失败，无效的点信息");
                return;
            }

            Info = dot;
            if (Info.DotStyle == null) 
            {
                Info.DotStyle = new CpldDB.Style
                {
                    Type = 1,
                    Size = 5,
                    ForegroundColor = Colors.White,
                    BackgroundColor = Colors.White
                };
            }
            

            DotCanvas = new Canvas();

            InCircuit = circuit;
            ParentCanvas = parentCanvas;

            Panel.SetZIndex(DotCanvas, 10000);
            parentCanvas.Children.Add(DotCanvas);
            ScaleChange(scale);

            AddDotMenu();
        }

        public void FlushDotStyle(CpldDB.Style dotStyle, double scale)
        {
            Info.DotStyle.Type = dotStyle.Type;
            Info.DotStyle.Size = dotStyle.Size;
            Info.DotStyle.ForegroundColor = dotStyle.ForegroundColor;
            Info.DotStyle.BackgroundColor = dotStyle.BackgroundColor;

            ScaleChange(scale);
        }

        public void RemoveDotMenu()
        {
            DotCanvas.ContextMenu = null;
            DotCanvas.MouseLeftButtonDown -= Mask_MouseLeftButtonDown;
        }

        public void AddDotMenu()
        {
            DotCanvas.ContextMenu = new ContextMenu();
            var deleteMenu = new MenuItem {Header = "删除"};
            deleteMenu.Click += DotDelete_MenuSelected;

            var editMenu = new MenuItem {Header = "标定"};
            editMenu.Click += DotEdit_MenuSelected;

            var emptyMenu = new MenuItem {Header = "标注为空点"};
            emptyMenu.Click += EmptyDot_MenuSelected;

            if (DotCanvas.ContextMenu == null) return;
            DotCanvas.ContextMenu.Items.Add(deleteMenu);
            DotCanvas.ContextMenu.Items.Add(editMenu);
            DotCanvas.ContextMenu.Items.Add(emptyMenu);

            DotCanvas.MouseLeftButtonDown += Mask_MouseLeftButtonDown;
        }

        private void EmptyDot_MenuSelected(object sender, RoutedEventArgs e)
        {
            if (EmptyDotEvent != null)
            {
                EmptyDotEvent(this);
            }
        }
        
        private void DotEdit_MenuSelected(object sender, RoutedEventArgs e)
        {
            if (CalibrationDotEvent != null)
                CalibrationDotEvent(this);
        }

        private void DotDelete_MenuSelected(object sender, RoutedEventArgs e)
        {
            DestroyDot();
        }

        private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount >= 2)
            {
                var editDot = new GraphicalConfigDot(this);
                editDot.ShowDialog();
            }
            else
            {
                ParentCanvas.MouseMove += Canvas_MouseMove;
                DotCanvas.MouseMove += Canvas_MouseMove;

                ParentCanvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                DotCanvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ParentCanvas.MouseMove -= Canvas_MouseMove;
            DotCanvas.MouseMove -= Canvas_MouseMove;
            ParentCanvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
            DotCanvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
            if (MoveDotEndEvent != null)
                MoveDotEndEvent(this);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingDotEvent != null)
                    MovingDotEvent(this, e.GetPosition(ParentCanvas));
            }
            else
            {
                ParentCanvas.MouseMove -= Canvas_MouseMove;
                DotCanvas.MouseMove -= Canvas_MouseMove;
                ParentCanvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                DotCanvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                if (MoveDotEndEvent != null)
                    MoveDotEndEvent(this);

            }

        }

        public void SelectedChangedEventHandler()
        {
            
            var rect = new Rectangle
            {
                Width = Info.DotStyle.Size*4,
                Height = Info.DotStyle.Size*4,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(rect, -Info.DotStyle.Size * 2);
            Canvas.SetTop(rect, -Info.DotStyle.Size * 2);
            DotCanvas.Children.Add(rect);
          
        }

        public void SetPosition(double x, double y, double scale)
        {    
            Info.Position = new Point(x / scale, y / scale);
            Canvas.SetLeft(DotCanvas, x);
            Canvas.SetTop(DotCanvas, y);
        }

        public void ScaleChange(double scale)
        {
            ChangeDotStyle(Info.DotStyle.Type, Info.DotStyle.Size * scale, Info.DotStyle.ForegroundColor, Info.DotStyle.BackgroundColor);
            Canvas.SetLeft(DotCanvas, Info.Position.X * scale);
            Canvas.SetTop(DotCanvas, Info.Position.Y * scale);
        }

        public void DestroyDot()
        {
            if (RemoveDotEvent != null)
            {
                RemoveDotEvent(this);
            }

        }

        public void ChangeDotStyle(int type, double r, Color fgColor, Color bgColor)
        {
            switch (type)
            {
                case 1:
                    DrawDotType1(DotCanvas, r, fgColor, bgColor);
                    break;
                case 2:
                    DrawDotType2(DotCanvas, r, fgColor, bgColor);
                    break;
                case 3:
                    DrawDotType3(DotCanvas, r, fgColor, bgColor);
                    break;
                default:
                    DrawDotType1(DotCanvas, r, fgColor, bgColor);
                    break;
            }
        }

        private static void DrawDotType1(Panel canv, double r, Color fgColor, Color bgColor)
        {
            canv.Children.Clear();
            if (r < 0) r = 0;
            canv.Height = 2 * r + 2;
            canv.Width = 2 * r + 2;

            var el = new Ellipse
            {
                Fill = new SolidColorBrush(bgColor),
                Stroke = new SolidColorBrush(Colors.Black),
                Width = r * 2,
                Height = r * 2
            };
            Canvas.SetLeft(el, -r);
            Canvas.SetTop(el, -r);
            canv.Children.Add(el);
        }

        private static void DrawDotType2(Panel canv, double r, Color fgColor, Color bgColor)
        {
            canv.Children.Clear();
            if (r < 0) r = 0;
            var el = new Ellipse
            {
                Fill = new SolidColorBrush(bgColor),
                Stroke = new SolidColorBrush(Colors.Black),
                Width = r * 2 + 2,
                Height = r * 2 + 2
            };
            el.SetValue(Panel.ZIndexProperty, 1);
            Canvas.SetLeft(el, -r);
            Canvas.SetTop(el, -r);

            canv.Children.Add(el);

            el = new Ellipse
            {
                Fill = new SolidColorBrush(fgColor),
                Stroke = new SolidColorBrush(Colors.Black),
                Width = r + 1,
                Height = r + 1
            };
            el.SetValue(Panel.ZIndexProperty, 2);
            el.SetValue(Canvas.LeftProperty, r / 2);
            el.SetValue(Canvas.TopProperty, r / 2);
            Canvas.SetLeft(el, -r / 2);
            Canvas.SetTop(el, -r / 2);
            canv.Children.Add(el);
        }

        private static void DrawDotType3(Panel canv, double r, Color fgColor, Color bgColor)
        {
            canv.Children.Clear();
            if (r < 0) r = 0;
            canv.Height = 2 * r + 2;
            canv.Width = 2 * r + 2;

            var myPath = new Path
            {
                HorizontalAlignment = HorizontalAlignment.Left, //对齐方式
                VerticalAlignment = VerticalAlignment.Center,   //线色线宽
                Stroke = Brushes.Black,                         //填充前景色
                StrokeThickness = 1
            };

            Brush br = new SolidColorBrush(fgColor);
            myPath.Fill = br;

            //创建路径
            var myPathGeometry = new PathGeometry();
            //1/4扇形
            var myPathFigure = new PathFigure { StartPoint = new Point(r, r) };

            var myLineSegment = new LineSegment { Point = new Point(r - r / Math.Sqrt(2), r - r / Math.Sqrt(2)) };
            myPathFigure.Segments.Add(myLineSegment);

            var myArcSegment = new ArcSegment
            {
                IsLargeArc = false,
                Point = new Point(r + r / Math.Sqrt(2), r - r / Math.Sqrt(2)),
                Size = new Size(r, r),
                SweepDirection = SweepDirection.Clockwise
            };
            myPathFigure.Segments.Add(myArcSegment);

            myLineSegment = new LineSegment { Point = new Point(r, r) };
            myPathFigure.Segments.Add(myLineSegment);

            myPathGeometry.Figures.Add(myPathFigure);
            myPath.Data = myPathGeometry;
            Canvas.SetLeft(myPath, -r);
            Canvas.SetTop(myPath, -r);
            canv.Children.Add(myPath);

            myPath = new Path
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            br = new SolidColorBrush(bgColor);
            myPath.Fill = br;

            //创建路径
            myPathGeometry = new PathGeometry();
            //1/4扇形
            myPathFigure = new PathFigure { StartPoint = new Point(r, r) };

            myLineSegment = new LineSegment { Point = new Point(r - r / Math.Sqrt(2), r - r / Math.Sqrt(2)) };
            myPathFigure.Segments.Add(myLineSegment);

            myArcSegment = new ArcSegment
            {
                IsLargeArc = true,
                Point = new Point(r + r / Math.Sqrt(2), r - r / Math.Sqrt(2)),
                Size = new Size(r, r),
                SweepDirection = SweepDirection.Counterclockwise
            };
            myPathFigure.Segments.Add(myArcSegment);

            myLineSegment = new LineSegment { Point = new Point(r, r) };
            myPathFigure.Segments.Add(myLineSegment);

            myPathGeometry.Figures.Add(myPathFigure);
            myPath.Data = myPathGeometry;
            Canvas.SetLeft(myPath, -r);
            Canvas.SetTop(myPath, -r);
            canv.Children.Add(myPath);
        }

    }
}
