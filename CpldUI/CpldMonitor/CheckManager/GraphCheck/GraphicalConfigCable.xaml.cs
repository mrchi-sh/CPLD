using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CpldBase;
using CpldDB;
using CpldLog;
using CpldUI.Check.Base;
using CpldUI.Check.GraphCheck;
using Window = System.Windows.Window;
using Panel = System.Windows.Controls.Panel;
using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;
using Cursors = System.Windows.Input.Cursors;
using Button = System.Windows.Controls.Button;

using DragEventArgs = System.Windows.DragEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using DragDropEffects = System.Windows.DragDropEffects;



namespace CpldUI.CheckManager.GraphCheck
{
    /// <summary>
    /// GraphicalConfigCable.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicalConfigCable : Window
    {
        private CableInfo Cable { get; set; }
        private UserInfo User { get; set; }

        private ObservableCollection<Dot> _dots;
        private ObservableCollection<Circuit> _circuits;

        private Dot _curSelectDot;
        private Circuit _curCircuit;

        private Circuit _emptyCircuit;
        public delegate void ScaleChangeEventHandler(double scale);
        public event ScaleChangeEventHandler ScaleChangeEvent;


        #region 构造

        public GraphicalConfigCable(CableInfo cable, UserInfo user)
        {
            InitializeComponent();
            Cable = cable;
            User = user;
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            _dots = new ObservableCollection<Dot>();
            _circuits = new ObservableCollection<Circuit>();

            #region 初始化图层
            //高亮蒙版  
            _maskCanvas = new Canvas {Opacity = 0};
            Panel.SetZIndex(_maskCanvas, MaskLayer); //图层
            ZoomCanvas.Children.Add(_maskCanvas);

            //选择框 
            _selectCanvas = new Canvas {Opacity = 0};
            ZoomCanvas.Children.Add(_selectCanvas);

            //辅助线 
            _auxiliaryLineCanvas = new Canvas {Opacity = 0};
            ZoomCanvas.Children.Add(_auxiliaryLineCanvas);
            #endregion

            //回路
            CircuitsTreeView.ItemsSource = _circuits;
            List<CircuitInfo> allCircuits;
            CacheGraph.GetCircuits(out allCircuits);
            var circuits = allCircuits.Where(circuit => circuit.ParentCableId == Cable.CableId);
            foreach (var circuit in circuits)
            {
                GraphAddCircuit(circuit);
            }
            //点
            List<DotInfo> alldots;
            CacheGraph.GetDos(out alldots);
            foreach (var circuit in _circuits)
            {
                var dots = alldots.Where(dot =>(dot.ParentCableId == Cable.CableId && dot.ParentCircuitId == circuit.Info.CircuitId))
                    .ToList();
                foreach (var dotInfo in dots)
                {
                    GraphAddDot(dotInfo,circuit);
                }
                circuit.GetAllLines(_zoomScale);
            }
            LoadPointLocation();
            //信息
            LbCableName.Content = Cable.CableName;
            LbRemark.Content = Cable.Remark;
            LbOperator.Content = User.UserName;
            //背景
            InitBgConfig();
           
        }
        
        #endregion

        #region 背景
        private void InitBgConfig()
        {
            if (!string.IsNullOrWhiteSpace(Cable.BgImgPath))
            {
                //有图
                CbImg.IsChecked = true;         //触发CbImg_Checked

            }
            else
            {
                //没图
                CbImg.IsChecked = false;        //触发CbImg_Unchecked

                if (_dots != null && _dots.Count > 0)
                {
                    _zoomHeight = Math.Max(_dots.Max(x => x.Info.Position.Y) + 50, 600);
                    _zoomWidth = Math.Max(_dots.Max(x => x.Info.Position.X) + 50, 800);
                }
                else
                {
                    _zoomHeight = 600;
                    _zoomWidth = 800;
                }

                ZoomCanvas.Height = _zoomHeight * _zoomScale;
                ZoomCanvas.Width = _zoomWidth * _zoomScale;
            }

            TbBgWidth.Text = _zoomWidth.ToString(CultureInfo.CurrentCulture);
            TbBgHeight.Text = _zoomHeight.ToString(CultureInfo.CurrentCulture);
            TbBgPath.Text = Cable.BgImgPath;
            TbOpacity.Text = ImgBackGround.Opacity.ToString(CultureInfo.CurrentCulture);
            SliderBgOpacity.Value = ImgBackGround.Opacity;
        }

        private void CbImg_Checked(object sender, RoutedEventArgs e)
        {
            TbBgHeight.IsEnabled = false;
            TbBgWidth.IsEnabled = false;
            TbBgPath.IsEnabled = true;
            BtnImport.IsEnabled = true;

            if (string.IsNullOrWhiteSpace(TbBgPath.Text)) return;

            Cable.BgImgPath = TbBgPath.Text;
            ChangeImg();
        }

        private void CbImg_Unchecked(object sender, RoutedEventArgs e)
        {
            TbBgHeight.IsEnabled = true;
            TbBgWidth.IsEnabled = true;
            TbBgPath.IsEnabled = false;
            BtnImport.IsEnabled = false;

            Cable.BgImgPath = string.Empty;
            ImgBackGround.Source = null;

        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = dialog.Filter = "All Files (*.*)|*.*";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() != true)
            {
                return;
            }
            TbBgPath.Text = dialog.FileName.Replace('\\','/');//触发TbImg_TextChanged
        }

        private void TbImg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbBgPath.Text)) return;
            Cable.BgImgPath = TbBgPath.Text;
            ChangeImg();//修改图片
        }

        private void ChangeImg()
        {
            try
            {
                var bgImage = new BitmapImage(new Uri(Cable.BgImgPath, UriKind.Absolute));
                ImgBackGround.Source = bgImage;

                _zoomHeight = bgImage.Height;
                _zoomWidth = bgImage.Width;
                ZoomCanvas.Height = bgImage.Height * _zoomScale;//触发缩放
                ZoomCanvas.Width = bgImage.Width * _zoomScale;
            }
            catch (Exception e)
            {
                LogControl.LogError("图片选择错误或丢失"+e);
                InfoBox.ErrorMsg("请选择正确的图片");
                if (_dots != null && _dots.Count > 0)
                {
                    _zoomHeight = Math.Max(_dots.Max(x => x.Info.Position.Y) + 50, 600);
                    _zoomWidth = Math.Max(_dots.Max(x => x.Info.Position.X) + 50, 800);
                }
                else
                {
                    _zoomHeight = 600;
                    _zoomWidth = 800;
                }

                ZoomCanvas.Height = _zoomHeight * _zoomScale;
                ZoomCanvas.Width = _zoomWidth * _zoomScale;
            }
        }

        private void SliderOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TbOpacity.Text = Convert.ToInt32(e.NewValue * 100) + "%";
            BgSettingChanged();
        }

        private void TbBackgroundSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            BgSettingChanged();
        }

        private void BgSettingChanged()
        {
            if (ZoomCanvas == null) return;
            if (ImgBackGround != null)
                ImgBackGround.Opacity = SliderBgOpacity.Value;
            if (!string.IsNullOrWhiteSpace(TbBgHeight.Text))
            {
                _zoomHeight = Convert.ToDouble(TbBgHeight.Text);
                ZoomCanvas.Height = _zoomHeight * _zoomScale;//触发缩放
            }
            if (string.IsNullOrWhiteSpace(TbBgWidth.Text)) return;
            _zoomWidth = Convert.ToDouble(TbBgWidth.Text);
            ZoomCanvas.Width = _zoomWidth * _zoomScale;//触发缩放
        }
        #endregion
        
        #region 缩放
        private double _zoomWidth;
        private double _zoomHeight;
        private double _zoomScale = 1.0;
        private bool _isMouseChange;
        private Point _refPosition = new Point(10.0, 10.0);

        private void RootCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _refPosition.X = e.GetPosition(ZoomCanvas).X / _zoomScale;
            _refPosition.Y = e.GetPosition(ZoomCanvas).Y / _zoomScale;
            _isMouseChange = true;
            ScaleSlider.Value += (double)e.Delta / 1000;
        }

        private void ZoomCanvasReset_Click(object sender, RoutedEventArgs e)
        {
            ScaleSlider.Value = 1.0;                                //触发 ScaleSlider_OnValueChanged
            Canvas.SetLeft(ZoomCanvas, 10);
            Canvas.SetTop(ZoomCanvas, 10);
        }

        private void ScaleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _zoomScale = e.NewValue;
            LbScale.Text = Math.Round(_zoomScale * 100.0, 2) + "%";
            ZoomCanvas.Height = _zoomHeight * _zoomScale;
            ZoomCanvas.Width = _zoomWidth * _zoomScale;             //触发 ZoomCanvas_OnSizeChanged
        }

        private void ZoomCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isMouseChange)
            {
                _isMouseChange = false;
                var x = Canvas.GetLeft(ZoomCanvas) + (Mouse.GetPosition(ZoomCanvas).X / _zoomScale - _refPosition.X) * _zoomScale;
                var y = Canvas.GetTop(ZoomCanvas) + (Mouse.GetPosition(ZoomCanvas).Y / _zoomScale - _refPosition.Y) * _zoomScale;

                if (x < 200 - ZoomCanvas.Width) x = 200 - ZoomCanvas.Width;
                if (x > 800) x = 800;
                if (y < 200 - ZoomCanvas.Height) y = 200 - ZoomCanvas.Height;
                if (y > 600) y = 600;
                Canvas.SetLeft(ZoomCanvas, x);
                Canvas.SetTop(ZoomCanvas, y);
            }

            ImgBackGround.Height = ZoomCanvas.Height;
            ImgBackGround.Width = ZoomCanvas.Width;
            if (_maskCanvas != null)
            {
                _maskCanvas.Height = ZoomCanvas.Height;
                _maskCanvas.Width = ZoomCanvas.Width;
            }
           
            if (ScaleChangeEvent != null)
                ScaleChangeEvent(_zoomScale);
        }

        private void RootCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(ZoomCanvas.Width > RootCanvas.ActualWidth) && !(ZoomCanvas.Height > RootCanvas.ActualHeight)) return;
            ScaleSlider.Value = Math.Min(RootCanvas.ActualWidth / ZoomCanvas.Width, RootCanvas.ActualHeight / ZoomCanvas.Height) - 0.01;    //触发 ScaleSlider_OnValueChanged
            Canvas.SetLeft(ZoomCanvas, 10);
            Canvas.SetTop(ZoomCanvas, 10);
        }


        #endregion
        
        #region 点

        #region 添加点

        private Dot GraphAddDot(DotInfo info, Circuit curCircuit)
        {
            var dot = new Dot(info, curCircuit, ZoomCanvas, _zoomScale);
            //图形点相关事件
            ScaleChangeEvent += dot.ScaleChange;         //点响应界面缩放事件

            dot.MovingDotEvent += GraphMovingDot;        //界面响应点拖动事件
            dot.MoveDotEndEvent += GraphMoveDotEnd;      //界面响应点拖动结束事件
            dot.RemoveDotEvent += GraphRemoveDot;        //界面响应点删除事件
            dot.RemoveDotEvent += curCircuit.RemoveDot;  //回路响应点删除事件
            
            dot.EmptyDotEvent += GraphEmptyDot;          
            dot.CalibrationDotEvent += CalibrationDot;   //界面响应点标定事件

            dot.DotCanvas.Cursor = Cursors.SizeAll;

            //回路
            curCircuit.CircuitDots.Add(dot);

            //界面
            _dots.Add(dot);
            return dot;
        }

        private void ZoomCanvas_Drop(object sender, DragEventArgs e)
        {
            _curCircuit = (Circuit)CircuitsTreeView.SelectedItem;
            var dotInfo = new DotInfo
            {
                ParentCableId = Cable.CableId,
                ParentPlugId = 0,
                ParentCircuitId = _curCircuit.Info.CircuitId,

                DotStyle = new CpldDB.Style(_curCircuit.Info.DotStyle),
                Position = e.GetPosition(ZoomCanvas)
            };
            if (_dots.Count > 0)
            {
                dotInfo.DotId = _dots.Max(dot => dot.Info.DotId) + 1;
            }
            else
            {
                dotInfo.DotId = 1;
            }
            dotInfo.Name = "点" + dotInfo.DotId;
            _curSelectDot = GraphAddDot(dotInfo, _curCircuit);

            //ZoomCanvas.MouseLeftButtonDown -= Mask_MouseLeftButtonDown;
            ZoomCanvas.MouseLeftButtonUp += ZoomCanvas_MouseLeftButtonUp;
            ZoomCanvas.MouseMove += ZoomCanvas_MouseMove;
            ZoomCanvas.MouseRightButtonUp += ZoomCanvas_MouseRightUp;

            ZoomCanvas.Cursor = Cursors.None;
            _curSelectDot.DotCanvas.Cursor = Cursors.None;
        }
        
        private void ZoomCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _curCircuit.GetAllLines(_zoomScale);
            var dotInfo = new DotInfo
            {
                ParentCableId = Cable.CableId,
                ParentPlugId = 0,
                ParentCircuitId = _curCircuit.Info.CircuitId,

                DotStyle = _curCircuit.Info.DotStyle,
                Position = e.GetPosition(ZoomCanvas)
            };
            if (_dots.Count > 0)
            {
                dotInfo.DotId = _dots.Max(dot => dot.Info.DotId) + 1;
            }
            else
            {
                dotInfo.DotId = 1;
            }
            dotInfo.Name = "点" + dotInfo.DotId;
            GraphAddDot(dotInfo, _curCircuit);
        }

        private void ZoomCanvas_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            ZoomCanvas.MouseLeftButtonUp -= ZoomCanvas_MouseLeftButtonUp;
            ZoomCanvas.MouseMove -= ZoomCanvas_MouseMove;
            ZoomCanvas.MouseRightButtonUp -= ZoomCanvas_MouseRightUp;

            ClearAuxiliaryLine();

            ZoomCanvas.Cursor = Cursors.Arrow;
            _curSelectDot.DestroyDot();
        }

        #endregion

        #region 移动点

        private void GraphMovingDot(Dot curDot, Point pos)
        {
            if (_dots.Count <= 0 || !Keyboard.IsKeyDown(Key.LeftShift))
            {
                curDot.SetPosition(pos.X, pos.Y, _zoomScale);
                ClearAuxiliaryLine();
            }
            else
            {
                pos = DrawAuxiliaryLine(curDot, pos);
                curDot.SetPosition(pos.X, pos.Y, _zoomScale);
                curDot.InCircuit.GetAllLines(_zoomScale);
            }
        }

        private void GraphMoveDotEnd(Dot dot)
        {
            dot.InCircuit.GetAllLines(_zoomScale);
            ClearAuxiliaryLine();
        }

        private void ZoomCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            GraphMovingDot(_curSelectDot, e.GetPosition(ZoomCanvas));
        }


        #endregion

        #region 删除点
        private void GraphRemoveDot(Dot dot)
        {
            if (_dots.IndexOf(dot) == -1) return;

            _dots.Remove(dot);
            ScaleChangeEvent -= dot.ScaleChange;
            ZoomCanvas.Children.Remove(dot.DotCanvas);
        }
        
        private void GraphEmptyDot(Dot dot)
        {
            _emptyCircuit.MoveDotToCircuit(dot.InCircuit, dot, _zoomScale);
        }

        #endregion
        
        #endregion
        
        #region 选择框
        ////选择框
        private Point _startPosition;               //起始点
        private Canvas _selectCanvas;
        //private void FlushSelectedCanvas(Point endPosition)
        //{
        //    _selectCanvas.Children.Clear();
        //    Canvas line1 = MyLine.DrawLine(2, 0, endPosition.Y - _startPosition.Y, 1, Colors.Red);
        //    Canvas line2 = MyLine.DrawLine(2, 0, endPosition.Y - _startPosition.Y, 1, Colors.Red);
        //    Canvas line3 = MyLine.DrawLine(2, endPosition.X - _startPosition.X, 0, 1, Colors.Red);
        //    Canvas line4 = MyLine.DrawLine(2, endPosition.X - _startPosition.X, 0, 1, Colors.Red);
        //    Canvas.SetLeft(line2, endPosition.X - _startPosition.X);
        //    Canvas.SetTop(line4, endPosition.Y - _startPosition.Y);
        //    _selectCanvas.Children.Add(line1);
        //    _selectCanvas.Children.Add(line2);
        //    _selectCanvas.Children.Add(line3);
        //    _selectCanvas.Children.Add(line4);
        //}

        //private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ZoomCanvas.MouseMove += image_MouseMove;
        //    ZoomCanvas.MouseLeftButtonUp += Mask_MouseLeftButtonUp;
        //    _startPosition = e.GetPosition(ZoomCanvas);

        //    Canvas.SetLeft(_selectCanvas, e.GetPosition(ZoomCanvas).X);
        //    Canvas.SetTop(_selectCanvas, e.GetPosition(ZoomCanvas).Y);

        //}

        //private void image_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        _selectCanvas.Opacity = 1;
        //        FlushSelectedCanvas(e.GetPosition(ZoomCanvas));
        //    }
        //    else
        //    {
        //        _selectCanvas.Opacity = 0;
        //        ZoomCanvas.MouseMove -= image_MouseMove;
        //        ZoomCanvas.MouseLeftButtonUp -= Mask_MouseLeftButtonUp;
        //    }

        //}

        //private void Mask_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    Canvas board = sender as Canvas;
        //    ZoomCanvas.MouseMove -= image_MouseMove;
        //    ZoomCanvas.MouseLeftButtonUp -= Mask_MouseLeftButtonUp;
        //    _selectCanvas.Opacity = 0;

        //}


        #endregion

        #region 回路

        #region 添加
        private void MenuAddCircuit_Click(object sender, RoutedEventArgs e)
        {
            var info = new CircuitInfo
            {
                ParentCableId= Cable.CableId,
                CircuitId = _circuits.Max(circuit => circuit.Info.CircuitId) + 1
            };
            info.Name = "回路" + info.CircuitId;
            var newCircuit = new GraphicalConfigCircuit(info) { Owner = this };
            if (newCircuit.ShowDialog() == true)
            {
                GraphAddCircuit(newCircuit.CurCircuit);
            }
        }
        private void GraphAddCircuit(CircuitInfo circuitInfo)
        {
            var circuit = new Circuit(circuitInfo);
            _circuits.Add(circuit);
            ScaleChangeEvent += circuit.ScaleChange;
            if (circuit.Info.CircuitId == 0)
                _emptyCircuit = circuit;
            ZoomCanvas.Children.Add(circuit.CircuitCanvas);
        }
        #endregion

        #region 编辑
        private void MenuEditCircuit_Click(object sender, RoutedEventArgs e)
        {
            GraphEditCircuit();
        }
        private void CircuitsTreeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GraphEditCircuit();
        }
        private void GraphEditCircuit()
        {
            if (CircuitsTreeView.SelectedItem == null)
                return;
            if (CircuitsTreeView.SelectedItem.GetType() == typeof(Circuit))
            {
                var circuit = (Circuit)CircuitsTreeView.SelectedItem;
                var addCircuitWindow = new GraphicalConfigCircuit(circuit.Info) { Owner = this };
                if (addCircuitWindow.ShowDialog() != true) return;
                circuit.FlushCircuitStyle(_zoomScale);
                CircuitsTreeView.ItemsSource = null;
                CircuitsTreeView.ItemsSource = _circuits;
            }
            else if (CircuitsTreeView.SelectedItem.GetType() == typeof(Dot))
            {
                var dot = (Dot)CircuitsTreeView.SelectedItem;
                var editDotWindow = new GraphicalConfigDot(dot) { Owner = this };
                editDotWindow.ShowDialog();
            }
        }
        private void BtnModifyAllStyle_Click(object sender, RoutedEventArgs e)
        {
            var info = new CircuitInfo
            {
                ParentCableId = Cable.CableId,
                Name = "回路"
            };
            var newCircuit = new GraphicalConfigCircuit(info) { Owner = this };
            if (newCircuit.ShowDialog() != true) return;
            foreach (var circuit in _circuits)
            {
                circuit.FlushCircuitStyle(newCircuit.CurCircuit, _zoomScale);
            }
            CircuitsTreeView.ItemsSource = null;
            CircuitsTreeView.ItemsSource = _circuits;
        }

        #endregion

        #region 添加点

        private void CircuitsTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var circuitPanel = sender as StackPanel;
            if (circuitPanel != null)
                circuitPanel.MouseMove += CircuitPanel_MouseMove;
        }
        private void CircuitPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var circuitPanel = sender as StackPanel;
            if (circuitPanel != null)
                circuitPanel.MouseMove -= CircuitPanel_MouseMove;
        }
        private void CircuitPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var circuitPanel = sender as StackPanel;
            if (circuitPanel == null)
                return;
            circuitPanel.MouseMove -= CircuitPanel_MouseMove;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(circuitPanel, CircuitsTreeView.SelectedItem, DragDropEffects.Copy);
            }
        }
        #endregion

        #region 移除
        private void GraphRemoveCircuit(Circuit circuit)
        {
            foreach (var dot in circuit.CircuitDots)
            {
                GraphRemoveDot(dot);
            }
            circuit.CircuitDots.Clear();
            if (circuit.Info.CircuitId == 0) return;
            _circuits.Remove(circuit);
            //ZoomCanvas.Children.Remove(circuit);
        }
        private void MenuRemoveCircuit_Click(object sender, RoutedEventArgs e)
        {
            if (CircuitsTreeView.SelectedItem == null)
                return;
            if (CircuitsTreeView.SelectedItem.GetType() == typeof(Circuit))
            {
                GraphRemoveCircuit((Circuit)CircuitsTreeView.SelectedItem);
            }
        }

        #endregion

        #region 查看

        private void CircuitsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CircuitsTreeView.SelectedItem == null)
            {
                return;
            }
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ResetLayers();
                return;
            }
            ResetLayers();
            if (CircuitsTreeView.SelectedItem.GetType() == typeof(Circuit))
            {
                HightLightCircuit((Circuit)CircuitsTreeView.SelectedItem);
            }
            else if (CircuitsTreeView.SelectedItem.GetType() == typeof(Dot))
            {
                HightLightDot((Dot)CircuitsTreeView.SelectedItem);
            }
        }
        
        private void CircuitsTreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem == null) return;
            treeViewItem.Focus();
            e.Handled = true;
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);
            return source;
        }
        #endregion

        #endregion

        #region 状态指示

        private Point _originPos;

        private void StatusFlush(MouseEventArgs e)
        {
            LbMousePositionZoom.Text = Convert.ToInt32(e.GetPosition(RootCanvas).X) + "," +
                                       Convert.ToInt32(e.GetPosition(RootCanvas).Y);
            LbMousePositionPanel.Text = Convert.ToInt32(e.GetPosition(ZoomCanvas).X / _zoomScale) + "," +
                                        Convert.ToInt32(e.GetPosition(ZoomCanvas).Y / _zoomScale);
            LbZoomOffset.Text = Convert.ToInt32(RootCanvas.ActualHeight) + "," +
                                Convert.ToInt32(RootCanvas.ActualWidth);
            if (_dots != null && _emptyCircuit != null)
            {
                LbDotCountInfo.Text = (_dots.Count - _emptyCircuit.CircuitDots.Count).ToString();
            }
            LbCircuitCountInfo.Text = (_circuits.Count - 1).ToString();
            LbScale.Text = Math.Round(_zoomScale * 100.0, 2) + "%";
        }

        private void RootCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            LbMousePositionZoom.Text = "";
            LbMousePositionPanel.Text = "";
        }

        private void RootCanvas_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _refPosition = e.GetPosition(RootCanvas);
            _originPos = new Point(Canvas.GetLeft(ZoomCanvas), Canvas.GetTop(ZoomCanvas));

        }

        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            StatusFlush(e);
            if (e.RightButton != MouseButtonState.Pressed) return;

            var x = e.GetPosition(RootCanvas).X - _refPosition.X;
            var y = e.GetPosition(RootCanvas).Y - _refPosition.Y;

            Canvas.SetLeft(ZoomCanvas, _originPos.X + x);
            Canvas.SetTop(ZoomCanvas, _originPos.Y + y);
        }

        #endregion

        #region 图层 蒙版

        //蒙版
        private Canvas _maskCanvas;

        private const int BackgroundLayer = -1;
        private const int MaskLayer = -2;
        private const int AuxiliaryLineLayer = 11;
        private const int CircuitLayer = 1;
        private const int DotLayer = 2;

        private const int HightLightMaskLayer = 9;
        private const int HightLightCircuitLayer = 11;
        private const int HightLightDotLayer = 12;

        private void HightLightDot(Dot dot)
        {
            _maskCanvas.Opacity = 0.8;
            Panel.SetZIndex(_maskCanvas, HightLightMaskLayer);               
            Panel.SetZIndex(dot.DotCanvas, HightLightDotLayer);               

        }

        private void HightLightCircuit(Circuit curCircuit)
        {
            _maskCanvas.Opacity = 0.8;
            Panel.SetZIndex(_maskCanvas, HightLightMaskLayer);               

            foreach (var dot in curCircuit.CircuitDots)
            {
                Panel.SetZIndex(dot.DotCanvas, HightLightDotLayer);            
            }
            Panel.SetZIndex(curCircuit.CircuitCanvas, HightLightCircuitLayer); 
        }

        private void ResetLayers()
        {
            _maskCanvas.Background = Brushes.White;
            _maskCanvas.Opacity = 0;
            _maskCanvas.Height = ZoomCanvas.Height;
            _maskCanvas.Width = ZoomCanvas.Width;
            Panel.SetZIndex(_maskCanvas, MaskLayer);           //蒙版层
            Panel.SetZIndex(ImgBackGround, BackgroundLayer);   //背景层
            //暗光层
            foreach (var dot in _dots)
            {
                Panel.SetZIndex(dot.DotCanvas, DotLayer);      //所有点
            }
            foreach (var circuit in _circuits)
            {
                Panel.SetZIndex(circuit.CircuitCanvas, CircuitLayer);//所有回路
            }
        }

        #endregion

        #region 辅助线
        //辅助线
        private Canvas _auxiliaryLineCanvas;

        private void ClearAuxiliaryLine()
        {
            _auxiliaryLineCanvas.Opacity = 0;
            _auxiliaryLineCanvas.Children.Clear();
        }

        private Point DrawAuxiliaryLine(Dot curDot, Point pos)
        {
            Dot dotX1 = null;
            Dot dotY1 = null;
            Dot dotX2 = null;
            Dot dotY2 = null;
            ClearAuxiliaryLine();
            _auxiliaryLineCanvas.Opacity = 1;
            var dotsXList = _dots.Where(dot =>(Math.Abs(pos.X - dot.Info.Position.X * _zoomScale) <= 5) && (!dot.Equals(curDot))).ToList();
            var dotsYList = _dots.Where(dot =>(Math.Abs(pos.Y - dot.Info.Position.Y * _zoomScale) <= 5) && (!dot.Equals(curDot))).ToList();

            if (dotsXList.Count > 0)
            {
                foreach (var dot in dotsXList)
                {
                    if (dotX1 != null)
                    {
                        if (Math.Abs(dot.Info.Position.X * _zoomScale - pos.X) < Math.Abs(dotX1.Info.Position.X * _zoomScale - pos.X))
                            dotX1 = dot;
                    }
                    else
                    {
                        dotX1 = dot;
                    }
                }
                dotsXList.Remove(dotX1);
                foreach (var dot in dotsXList)
                {
                    if (Math.Abs(Math.Abs(dotX1.Info.Position.X * _zoomScale - dot.Info.Position.X * _zoomScale) - Math.Abs(dotX1.Info.Position.X * _zoomScale - pos.X)) <= 5)
                    {
                        dotY2 = dot;
                        pos.X = dotX1.Info.Position.X * _zoomScale + (dotX1.Info.Position.X * _zoomScale - dotY2.Info.Position.X * _zoomScale);
                    }
                }
            }
            if (dotsYList.Count > 0)
            {
                foreach (var dot in dotsYList)
                {
                    if (dotY1 != null)
                    {
                        if (Math.Abs(dot.Info.Position.Y * _zoomScale - pos.Y) < Math.Abs(dotY1.Info.Position.Y * _zoomScale - pos.Y))
                            dotY1 = dot;
                    }
                    else
                    {
                        dotY1 = dot;
                    }
                }
                dotsYList.Remove(dotY1);
                foreach (var dot in dotsYList)
                {
                    if (Math.Abs(Math.Abs(dotY1.Info.Position.Y * _zoomScale - dot.Info.Position.Y * _zoomScale) - Math.Abs(dotY1.Info.Position.Y * _zoomScale - pos.Y)) <= 5)
                    {
                        dotX2 = dot;
                        pos.Y = (dotY1.Info.Position.Y + (dotY1.Info.Position.Y - dotX2.Info.Position.Y)) * _zoomScale;
                    }
                }
            }
            if (dotX1 != null)
            {
                _auxiliaryLineCanvas.Children.Add(new Line
                {
                    X1 = dotX1.Info.Position.X * _zoomScale,
                    Y1 = dotX1.Info.Position.Y * _zoomScale,
                    X2 = pos.X - dotX1.Info.Position.X * _zoomScale + dotX1.Info.Position.X * _zoomScale,
                    Y2 = 0 + dotX1.Info.Position.Y * _zoomScale,
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Red)
                });
                if (dotY2 != null)
                {
                    _auxiliaryLineCanvas.Children.Add(new Line
                    {
                        X1 = dotY2.Info.Position.X * _zoomScale,
                        Y1 = dotY2.Info.Position.Y * _zoomScale - 25,
                        X2 = 0 + dotY2.Info.Position.X * _zoomScale,
                        Y2 = 50 + dotY2.Info.Position.Y * _zoomScale - 25,
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush(Colors.Red)
                    });
                }
            }

            if (dotY1 != null)
            {
                _auxiliaryLineCanvas.Children.Add(new Line
                {
                    X1 = dotY1.Info.Position.X * _zoomScale,
                    Y1 = dotY1.Info.Position.Y * _zoomScale,
                    X2 = 0 + dotY1.Info.Position.X * _zoomScale,
                    Y2 = pos.Y - dotY1.Info.Position.Y * _zoomScale + dotY1.Info.Position.Y * _zoomScale,
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Red)
                });
                
                if (dotX2 != null)
                {
                    _auxiliaryLineCanvas.Children.Add(new Line
                    {
                        X1 = dotX2.Info.Position.X * _zoomScale - 25,
                        Y1 = dotX2.Info.Position.Y * _zoomScale,
                        X2 = 50 + dotX2.Info.Position.X * _zoomScale - 25,
                        Y2 = 0 + dotX2.Info.Position.Y * _zoomScale,
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush(Colors.Red)
                    });
                }
               
            }
            
            return pos;


        }

        #endregion

        #region 标定
        //标定
        private Hashtable _phyAddrMapToDot;//点地址 -》点引用

        private void LoadPointLocation()
        {
            _phyAddrMapToDot = new Hashtable();
            ResetPointLocation();
        }

        private void ResetPointLocation()
        {
            _phyAddrMapToDot.Clear();
            foreach (var dot in _dots)
            {
                if (!string.IsNullOrEmpty(dot.Info.PhyAddr) && !_phyAddrMapToDot.ContainsKey(dot.Info.PhyAddr))
                    _phyAddrMapToDot.Add(dot.Info.PhyAddr, dot);
            }
        }
        private Thread _threadHandler;
        private bool _isOverFlag = true;
        public delegate void CalibrationEventHandler(Dot dot, string dotPhyAddr);

        private bool _singleFlage;
        private Dot _calibrationDot;
        public CalibrationEventHandler CalibrationEvent;


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<CircuitInfo> allCircuits;
            CacheGraph.GetCircuits(out allCircuits);
            var dbCircuits = allCircuits.Where(circuit => circuit.ParentCableId == Cable.CableId).ToList();
            var curCircuits = _circuits.Select(circuit => circuit.Info).ToList();
            CacheGraph.AddCircuits(curCircuits.Except(dbCircuits).ToList());
            CacheGraph.DelCircuits(dbCircuits.Except(curCircuits).ToList());


            List<DotInfo> alldots;
            CacheGraph.GetDos(out alldots);
            var dbDots = alldots.Where(dot => dot.ParentCableId == Cable.CableId).ToList();
            var curDots = _dots.Select(dot => dot.Info).ToList();
            CacheGraph.AddDots(curDots.Except(dbDots).ToList());
            CacheGraph.DelDots(dbDots.Except(curDots).ToList());

            CacheGraph.CacheSaveCable();
            InfoBox.InfoMsg("保存成功");
        }
        
        private void BtnCombineCircuit_Click(object sender, RoutedEventArgs e)
        {
            foreach (var circuit in _circuits)
            {
                if (circuit != _emptyCircuit)
                {
                    ZoomCanvas.Children.Remove(circuit.CircuitCanvas);
                }
            }

            if (_emptyCircuit == null) return;
            _emptyCircuit.CircuitDots.Clear();
            foreach (var dot in _dots)
            {
                _emptyCircuit.CircuitDots.Add(dot);
            }
            _emptyCircuit.FlushCircuitStyle(_zoomScale);
            ResetPointLocation();
            _circuits.Clear();
            _circuits.Add(_emptyCircuit);
        }

        private void BtnSampleCable_Click(object sender, RoutedEventArgs e)
        {
            var btnButton = sender as Button;
            if (btnButton == null) return;
            BtnCombineCircuit_Click(sender, e);

            WaitBox wb = null;
            var t = new Thread(() =>
            {
                wb = new WaitBox();
                wb.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            bool isNoShortCircuit;
            List<List<string>> shortCircuitInfoResult;
            int pointNo;
            int circuitNo;
            if (!CpldControl.Check.SampleCheck.BeginSampleCheck(out isNoShortCircuit, out shortCircuitInfoResult, out pointNo, out circuitNo))
            {
                InfoBox.ErrorMsg("取样失败,请检查设备连接");
                wb.Dispatcher.Invoke((Action)(() => wb.Close()));
                return;
            }
            if (isNoShortCircuit)
            {
                InfoBox.ErrorMsg("未检测到回路");
                wb.Dispatcher.Invoke((Action)(() => wb.Close()));
                return;
            }
            var errDot = "";
            foreach (var circuit in shortCircuitInfoResult)
            {
                var info = new CircuitInfo
                {
                    ParentCableId = Cable.CableId, 
                    CircuitId = _circuits.Max(x => x.Info.CircuitId) + 1
                };
                info.Name = "回路" + info.CircuitId;
                var newCircuit = new Circuit(info);
                GraphAddCircuit(newCircuit.Info);
                foreach (var dot in circuit)
                {
                    if (_phyAddrMapToDot.ContainsKey(dot))
                        newCircuit.MoveDotToCircuit(((Dot)_phyAddrMapToDot[dot]).InCircuit, (Dot)_phyAddrMapToDot[dot], _zoomScale);
                    else
                        errDot += dot + ",";
                }
            }
            ResetLayers();
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
            if (string.IsNullOrEmpty(errDot)) return;
            errDot = errDot.Remove(errDot.Length - 1, 1);
            errDot += "在图中未找到";
            InfoBox.ErrorMsg(errDot);
        }

        private void PointSearch() //use device to search point
        {
            var index = 0;
            var pointAddr = "";
            while (_isOverFlag)
            {
                List<string> pointList;
                if (CpldControl.Check.SampleCheck.BeginPointCheck(out pointList))
                {
                    if (pointList == null || pointList.Count > 1 || pointAddr == pointList[0])
                        continue;

                    pointAddr = pointList[0];
                    if (_singleFlage)
                    {
                        if (_calibrationDot != null)
                        {
                            Dispatcher.Invoke(new Action(delegate { CalibrationEvent(_calibrationDot, pointList[0]); }));
                        }
                        _isOverFlag = false;
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(delegate { CalibrationEvent(_dots[index], pointList[0]); }));
                        index++;
                        if (index == _dots.Count)
                            _isOverFlag = false;
                        else
                            Dispatcher.Invoke(new Action(delegate { _dots[index].SelectedChangedEventHandler(); }));
                    }
                }
                Thread.Sleep(100);
            }
            Dispatcher.Invoke(new Action(
                delegate
                {
                    _singleFlage = false;
                    _isOverFlag = true;
                    _calibrationDot = null;
                    ResetLayers();
                    ResetPointLocation();
                    CircuitsTreeView.ItemsSource = null;
                    CircuitsTreeView.ItemsSource = _circuits;
                    BtnStartCalibration.IsEnabled = true;
                    foreach (var circuit in _circuits)
                    {
                        circuit.FlushCircuitStyle(_zoomScale);
                    }

                    foreach (var dot in _dots)
                    {
                        foreach (var curDot in _dots)
                        {
                            if (curDot == dot)
                                continue;

                            if (curDot.Info.PhyAddr == dot.Info.PhyAddr)
                            {
                                curDot.ChangeDotStyle(1, curDot.Info.DotStyle.Size, Colors.Red, Colors.Red);
                            }
                        }
                    }
                }
            ));
        }

        private void UpdataDotPhyAddr(Dot dot, string phyAddr)
        {
            dot.Info.PhyAddr = phyAddr;
            foreach (var curDot in _dots)
            {
                if (curDot.Info.PhyAddr == phyAddr && curDot != dot)
                {
                    InfoBox.ErrorMsg(dot.Info.Name + "与" + curDot.Info.Name + "物理地址重复");
                }
            }
            InfoBox.PlaySound(true);
            Panel.SetZIndex(dot.DotCanvas, DotLayer);       //暗光
            dot.FlushDotStyle(dot.Info.DotStyle, _zoomScale);             //去边框
        }

        private void EndCalibration_Click(object sender, RoutedEventArgs e)
        {
            _isOverFlag = false;
        }

        private void CalibrationDot(Dot dot)
        {
            ResetLayers();
            HightLightDot(dot);
            BtnStartCalibration.IsEnabled = false;

            CalibrationEvent += UpdataDotPhyAddr;
            _calibrationDot = dot;
            _singleFlage = true;

            dot.SelectedChangedEventHandler();

            _threadHandler = new Thread(PointSearch);
            _threadHandler.Start();
        }

        private void StartCalibration_Click(object sender, RoutedEventArgs e)
        {
            if (_dots.Count <= 0)
            {
                InfoBox.ErrorMsg("未定义任何点");
                return;
            }
            BtnStartCalibration.IsEnabled = false;
            ResetLayers();
            foreach (var dot in _dots)
            {
                HightLightDot(dot);
            }
            CalibrationEvent += UpdataDotPhyAddr;
            var sortedList = _dots.OrderBy(dot => dot.Info.Position.X).ThenBy(dot => dot.Info.Position.Y).ToList();
            for (var i = 0; i < sortedList.Count; i++)
            {
                _dots.Move(_dots.IndexOf(sortedList[i]), i);
            }

            _dots.First().SelectedChangedEventHandler();

            _threadHandler = new Thread(PointSearch);
            _threadHandler.Start();
        }
        #endregion
       
    }



}
