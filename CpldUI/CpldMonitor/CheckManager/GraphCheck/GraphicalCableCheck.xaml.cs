using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CpldBase;
using CpldControl.Check;
using CpldDB;
using CpldLog;
using CpldUI.Check.Base;
using CpldUI.CheckManager.Base;
using Window = System.Windows.Window;
using Color = System.Windows.Media.Color;
using Label = System.Windows.Controls.Label;
using TextBox = System.Windows.Controls.TextBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using RadioButton = System.Windows.Controls.RadioButton;
using TabControl = System.Windows.Controls.TabControl;


namespace CpldUI.CheckManager.GraphCheck
{
    /// <summary>
    /// GraphicalConfigCable.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicalCableCheck : Window
    {
        public CableInfo Cable { get; set; }
        public UserInfo User { get; set; }

        private ObservableCollection<Dot> _dots;
        private ObservableCollection<Circuit> _circuits;
        private Circuit _emptyCircuit;

        public delegate void ScaleChangeEventHandler(double scale);
        public event ScaleChangeEventHandler ScaleChangeEvent;

        #region 构造

        public GraphicalCableCheck(CableInfo cable, UserInfo user)
        {
            InitializeComponent();
            if (cable == null)
            {
                LogControl.LogError("选择样线失败，空样线信息");
                return;
            }
            Cable = cable;
            if (user == null)
            {
                LogControl.LogError("选择样线失败，空用户信息");
                return;
            }
            User = user;
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            _dots = new ObservableCollection<Dot>();
            _circuits = new ObservableCollection<Circuit>();

            //错误线
            _errorLineCanvas = new Canvas ();
            ZoomCanvas.Children.Add(_errorLineCanvas);

            List<CircuitInfo> allCircuits;
            CacheGraph.GetCircuits(out allCircuits);
            var circuits = allCircuits.Where(circuit => circuit.ParentCableId == Cable.CableId);
            foreach (var circuit in circuits)
            {
                GraphAddCircuit(circuit);
            }

            List<DotInfo> alldots;
            CacheGraph.GetDos(out alldots);
            foreach (var circuit in _circuits)
            {
                var dots = alldots.Where(dot => (dot.ParentCableId == Cable.CableId && dot.ParentCircuitId == circuit.Info.CircuitId))
                    .ToList();
                foreach (var dotInfo in dots)
                {
                    GraphAddDot(dotInfo,circuit);
                }
                circuit.GetAllLines(_zoomScale);
            }
            LoadPointLocation();

            LbCableName.Content = Cable.CableName;
            LbRemark.Content = Cable.Remark;
            LbOperator.Content = User.UserName;

            InitBgConfig();
            
            CreateBarControl();
            LoadCheckSetting();
        }

     
        #endregion
        
        private void InitBgConfig()
        {
            if (!string.IsNullOrWhiteSpace(Cable.BgImgPath))
            {
                //有图
                try
                {
                    var bgImage = new BitmapImage(new Uri(Cable.BgImgPath, UriKind.Absolute));
                    ImgBackGround.Source = bgImage;

                    _zoomHeight = bgImage.Height;
                    _zoomWidth = bgImage.Width;
                    ZoomCanvas.Height = bgImage.Height * _zoomScale; //触发缩放
                    ZoomCanvas.Width = bgImage.Width * _zoomScale;
                    return;
                }
                catch (Exception e)
                {
                    InfoBox.ErrorMsg("请配置正确的图片路径");
                    LogControl.LogError(e);
                }
            }
            //没图或图片读取失败
            ImgBackGround.Source = null;

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
      
        private void GraphAddDot(DotInfo info, Circuit curCircuit)
        {
            var dot = new Dot(info, curCircuit, ZoomCanvas, _zoomScale);
            //图形点相关事件
            ScaleChangeEvent += dot.ScaleChange;         //点响应界面缩放事件
            //回路
            curCircuit.CircuitDots.Add(dot);
            //界面
            _dots.Add(dot);
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

        #region 缩放
        private double _zoomWidth;
        private double _zoomHeight;
        private double _zoomScale = 1.0;


        private void zoomCanvasReset_Click(object sender, RoutedEventArgs e)
        {
            ScaleSlider.Value = 100;                            //触发 ScaleSlider_OnValueChanged
            Canvas.SetLeft(ZoomCanvas, 10);
            Canvas.SetTop(ZoomCanvas, 10);
        }

        private void ScaleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _zoomScale = e.NewValue/100;
            ZoomCanvas.Height = _zoomHeight * _zoomScale;
            ZoomCanvas.Width = _zoomWidth * _zoomScale;         //触发 ZoomCanvas_OnSizeChanged
        }

        private void ZoomCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RemoveErrorLine();

            ImgBackGround.Height = ZoomCanvas.Height;
            ImgBackGround.Width = ZoomCanvas.Width;
            if (_errorLineCanvas != null)
            {
                _errorLineCanvas.Height = ZoomCanvas.Height;
                _errorLineCanvas.Width = ZoomCanvas.Width;
            }
            if (ScaleChangeEvent != null)
                ScaleChangeEvent(_zoomScale);
        }

        private void RootCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (TabInfo.SelectedIndex != 0)
                return;
            if (!(ZoomCanvas.Width > RootCanvas.ActualWidth) && !(ZoomCanvas.Height > RootCanvas.ActualHeight)) return;
            ScaleSlider.Value = Math.Round(Math.Min(RootCanvas.ActualWidth / ZoomCanvas.Width,
                                    RootCanvas.ActualHeight / ZoomCanvas.Height)*100 - 1,0);                            //触发 ScaleSlider_OnValueChanged
            Canvas.SetLeft(ZoomCanvas, 10);
            Canvas.SetTop(ZoomCanvas, 10);
        }


        #endregion

        #region 状态指示
        private void StatusFlush(object sender, MouseEventArgs e)
        {
            if (_dots != null && _emptyCircuit != null)
            {
                LbDotCountInfo.Content = (_dots.Count - _emptyCircuit.CircuitDots.Count).ToString();
            }
            if (_emptyCircuit != null)
                LbCircuitCountInfo.Content = (_circuits.Count - 1).ToString();
        }

        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            StatusFlush(sender, e);
        }

        #endregion

        #region 打印标签

        private DataTable _barDt;
        private void CreateBarControl()
        {
            if (!DbBar.LoadBarInfo(Cable.CableName,out _barDt))
                return;
            if (_barDt.Rows.Count == 0)
                return;

            for (var i = 0; i < _barDt.Rows.Count; i++)
            {
                var gridRow = new Grid();
                //标签定义
                var cd1 = new RowDefinition {Height = new GridLength(1, GridUnitType.Star)};
                gridRow.RowDefinitions.Add(cd1);
                //标签名称
                var barFieldName = _barDt.Rows[i]["bar_field_name"].ToString();
                var lb = new Label
                {
                    Content = barFieldName + ":",
                    FontSize = 16
                };
                Grid.SetRow(lb, 0);
                gridRow.Children.Add(lb);
                //输入框定义
                var cd2 = new RowDefinition {Height = new GridLength(1, GridUnitType.Star)};
                gridRow.RowDefinitions.Add(cd2);

                var tb = new TextBox
                {
                    Name = "tbName_" + barFieldName,
                    Margin = new Thickness(0,3,0,3),
                    Text = "",
                    IsReadOnly = false
                };

                RegisterName(tb.Name, tb);
                var type = (BarFieldType)_barDt.Rows[i]["bar_field_type"];
                var isBarEditable = Convert.ToBoolean(_barDt.Rows[i]["is_editable"]);
                if (!isBarEditable)                    
                {
                    var barFieldValue = "";
                    switch (type)
                    {
                        case BarFieldType.AutoIncrement:
                        case BarFieldType.Constant:
                            barFieldValue = _barDt.Rows[i]["bar_field_value"].ToString();
                            break;
                        case BarFieldType.Month:
                            break;
                        case BarFieldType.Day:
                            break;
                        case BarFieldType.Year:
                            break;
                        default:
                            barFieldValue = "";
                            break;
                    }
                    tb.Text = barFieldValue;
                    tb.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                    tb.IsReadOnly = true;
                }
                Grid.SetRow(tb, 1);
                gridRow.Children.Add(tb);

                //
                ListBar.Items.Add(gridRow);
            }
        }
        

        #endregion

        #region 检测设定
        private void LoadCheckSetting()
        {
            LbOkTime.Content = Cable.Settings.OkTime;
            LbNgTime.Content = Cable.Settings.NgTime;

            if (Cable.Settings.AutoStart == false)
            {
                LblStartMode.Content = "当前工作模式:手动开始，请点击开始检测";
                RbManual.IsChecked = true;
            }
            else
            {
                LblStartMode.Content = "当前工作模式：OK等待，第一次请手动点击开始";
                RbOkWait.IsChecked = true;
            }

            if (Cable.Settings.OkAutoRelease == false)
                RbOkManual.IsChecked = true;
            else
                RbOkAuto.IsChecked = true;

            if (Cable.Settings.NgAutoRelease == false)
                RbNgManual.IsChecked = true;
            else
                RbNgAuto.IsChecked = true;
        }
        
        private void RbManual_Checked(object sender, RoutedEventArgs e)
        {
            var tmpRadioButton = sender as RadioButton;
            if (tmpRadioButton == null) return;
            switch (tmpRadioButton.Name)
            {
                case "RbManual":
                    Cable.Settings.AutoStart = false;
                    break;
                case "RbOkWait":
                    Cable.Settings.AutoStart = true;
                    break;
                case "RbOkManual":
                    Cable.Settings.OkAutoRelease = false;
                    break;
                case "RbOkAuto":
                    Cable.Settings.OkAutoRelease = true;
                    break;
                case "RbNgManual":
                    Cable.Settings.NgAutoRelease = false;
                    break;
                case "RbNgAuto":
                    Cable.Settings.NgAutoRelease = true;
                    break;
                default:break;
            }
            DbGraph.UpdateCables(new List<CableInfo> {Cable});
        }
        
        #endregion

        #region 检测

        private static bool _autoStartFlag;
        private static bool _needStopFlag;
        private static bool _manualStopFlag = true;
        private static bool _manualStartOnce;
        private Thread _autoStartThreadHandler;
        private Canvas _errorLineCanvas;                //错误线图层
        private string _circuitLoop = "";

        private Hashtable _phyAddrMapToDot;             //点地址 -》点引用

        private void AbortThread()
        {
            if (_autoStartThreadHandler == null || !_autoStartThreadHandler.IsAlive) return;
            _autoStartFlag = false;
            _needStopFlag = false;
            _manualStopFlag = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _manualStartOnce = false;
            AbortThread();
        }
        
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

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var dot in _dots)
            {
                if (string.IsNullOrEmpty(dot.Info.PhyAddr))
                {
                    InfoBox.ErrorMsg(dot.Info.Name + " 未标定");
                    return;
                }
                foreach (var curDot in _dots)
                {
                    if (curDot.Info.PhyAddr != dot.Info.PhyAddr || curDot == dot) continue;
                    InfoBox.ErrorMsg(dot.Info.Name + "与" + curDot.Info.Name + "物理地址重复");
                    return;
                }
            }
            _circuitLoop = "";
            foreach (var circuit in _circuits)
            {
                if (circuit.Info.CircuitId == 0)
                    continue;
                var tmpCircuitLoop = circuit.CircuitDots.Aggregate("", (current, dot) => current + (dot.Info.PhyAddr + "-"));
                if (!string.IsNullOrEmpty(tmpCircuitLoop))
                    _circuitLoop += tmpCircuitLoop.Remove(tmpCircuitLoop.Length - 1, 1) + ";";
            }
            if (!string.IsNullOrEmpty(_circuitLoop))
                _circuitLoop = _circuitLoop.Remove(_circuitLoop.Length - 1, 1);
            if (Cable.Settings.AutoStart)
            {
                if (!_manualStartOnce)
                {
                    _autoStartFlag = true;
                    _manualStopFlag = true;
                    _manualStartOnce = true;
                    _autoStartThreadHandler = new Thread(OkWaitCheck);
                    _autoStartThreadHandler.Start();
                }
                else
                {
                    _manualStartOnce = false;
                    AbortThread();
                    Thread.Sleep(500);
                    ManualCheck();

                    Dispatcher.Invoke(new Action(delegate
                    {
                        LblStartMode.Content = "当前工作模式：OK等待，第一次请手动点击开始";
                    }));
                }
            }
            else
            {
                ManualCheck();
            }

        }

        private void RemoveErrorLine()
        {
            if (_errorLineCanvas != null)
                _errorLineCanvas.Children.Clear();
        }

        private void DrawErrorLines(IEnumerable<List<string>> shortCircuitList, IEnumerable<List<string>> openCircuitList)
        {
            try
            {                
                foreach (var circuit in openCircuitList)
                {
                    var info = new CircuitInfo
                    {
                        CircuitId = -1,
                        LineStyle = new CpldDB.Style
                        {
                            Type = 1,
                            Size = 2,
                            ForegroundColor = Colors.Red,
                            BackgroundColor = Colors.Transparent,

                        },
                        DotStyle = new CpldDB.Style
                        {
                            Type = 1,
                            Size = 4,
                            ForegroundColor = Colors.Transparent,
                            BackgroundColor = Colors.Transparent,
                        },
                    };
                    var tmpCircuit = new Circuit(info);
                    foreach (var dot in circuit)
                    {
                        if (_phyAddrMapToDot.ContainsKey(dot))
                            tmpCircuit.CircuitDots.Add((Dot)_phyAddrMapToDot[dot]);
                    }
                    tmpCircuit.GetAllLines(_zoomScale);
                    _errorLineCanvas.Children.Add(tmpCircuit.CircuitCanvas);
                }
                foreach (var circuit in shortCircuitList)
                {
                    var info = new CircuitInfo
                    {
                        CircuitId = -1,
                        LineStyle = new CpldDB.Style
                        {
                            Type = 2,
                            Size = 6,
                            ForegroundColor = Colors.Green,
                            BackgroundColor = Colors.Transparent,

                        },
                        DotStyle = new CpldDB.Style
                        {
                            Type = 1,
                            Size = 4,
                            ForegroundColor = Colors.Transparent,
                            BackgroundColor = Colors.Transparent,
                        },
                    };
                    var tmpCircuit = new Circuit(info);
                    foreach (var dot in circuit)
                    {
                        if (_phyAddrMapToDot.ContainsKey(dot))
                            tmpCircuit.CircuitDots.Add((Dot)_phyAddrMapToDot[dot]);
                    }
                    tmpCircuit.GetAllLines(_zoomScale);
                    _errorLineCanvas.Children.Add(tmpCircuit.CircuitCanvas);
                }
            }
            catch (Exception e)
            {
                LogControl.LogError(e);
                throw;
            }

        }

        private void HandleCheckResult(bool isCheckOk, IEnumerable<List<string>> shortCircuitList, IEnumerable<string[]> openCircuitList)
        {
            var shortCircuit = "";
            var openCircuit = "";

            if (isCheckOk)
            {
                CommonCheck.UpdateBarList(Cable.CableName, this, _barDt);                                                      
                CpldControl.Bartend.BartendControl.PrintBar(Cable.CableName);                              

                CommonCheck.AddCheckResult(this, _barDt, Cable.CableName, "OK", shortCircuit, openCircuit, User.UserName);     
                Cable.Settings.OkTime++;                                                                                        
                CommonControl.SendOkRelay(1000);
            }
            else
            {
                foreach (var tmpList in shortCircuitList)
                {
                    var scTmp = "";
                    foreach (var tmp in tmpList)
                    {
                        if (_phyAddrMapToDot.ContainsKey(tmp))
                        {
                            scTmp += ((Dot)_phyAddrMapToDot[tmp]).Info.Name + "-";
                        }
                        else
                        {
                            scTmp += tmp + "(未标定)-";
                        }
                    }
                    shortCircuit += scTmp.Remove(scTmp.Length - 1) + "; ";
                }

                openCircuit = openCircuitList.Aggregate(openCircuit, (current, arrTmp) => current + (arrTmp[0] + "/" + arrTmp[1] + "; "));

                CommonCheck.AddCheckResult(this, _barDt, Cable.CableName, "NG", shortCircuit, openCircuit, User.UserName);            //add check result to db
                Cable.Settings.NgTime++;    
            }
            LbOkTime.Content = Cable.Settings.OkTime;
            LbNgTime.Content = Cable.Settings.NgTime;
            InfoBox.PlaySound(isCheckOk);
            new ResultPopUp(isCheckOk, Cable.Settings.OkAutoRelease, Cable.Settings.NgAutoRelease).ShowDialog();
        }

        private void ManualCheck()
        {
            WaitBox wb = null;
            var t = new Thread(() =>
            {
                wb = new WaitBox();
                wb.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            int pointNo;
            int circuitNo;
            List<string[]> shortCircuitList;
            List<string[]> openCircuitList;
            if (!SampleCheck.ManualCheck(_circuitLoop, out pointNo, out circuitNo, out shortCircuitList, out openCircuitList))
            {
                wb.Dispatcher.Invoke((Action)(() => wb.Close()));
                return;
            }

            List<List<string>> shortCircuitResult;
            List<List<string>> openCircuitResult;
            CpldControl.DataParser.CommonParser.PointToCircuit(shortCircuitList, out shortCircuitResult);
            CpldControl.DataParser.CommonParser.PointToPair(openCircuitList, out openCircuitResult);

            RemoveErrorLine();
            var isCheckOk = true;                         //0-NG, 1-OK
            if (!(shortCircuitList.Count == 0 && openCircuitList.Count == 0))
            {
                isCheckOk = false;
                DrawErrorLines(shortCircuitResult, openCircuitResult);
            }
            string tmp;
            InfoBox.RichTextMsg(out tmp, isCheckOk, pointNo, circuitNo, shortCircuitResult, openCircuitList, _phyAddrMapToDot);

            TbInfo.Text = tmp;
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
            HandleCheckResult(isCheckOk, shortCircuitResult, openCircuitList);
        }

        private void OkWaitCheck()
        {
            var pointNo = 0;
            var circuitNo = 0;
            List<string[]> openCircuitList;
            List<List<string>> shortCircuitResult = null;
            List<List<string>> openCircuitResult = null;

            var isCheckOk = false;
            var currErrorMsg = "";
            var prevErrorMsg = "";   

            Dispatcher.Invoke(new Action(delegate{LblStartMode.Content = "OK等待中......";}));

            while (_autoStartFlag)
            {
                List<string[]> shortCircuitList;
                if (!SampleCheck.ManualCheck(_circuitLoop, out pointNo, out circuitNo, out shortCircuitList, out openCircuitList))
                {
                    InfoBox.ErrorMsg("检测失败,请检查设备连接");
                    Dispatcher.Invoke(new Action(delegate
                    {
                        LblStartMode.Content = "当前工作模式：OK等待，第一次请手动点击开始";
                        BtnStart.Content = "开  始";
                    }));
                    return;
                }

                CpldControl.DataParser.CommonParser.PointToCircuit(shortCircuitList, out shortCircuitResult);

                while (_needStopFlag)
                {
                    if (!CpldControl.Check.SampleCheck.ManualCheck(_circuitLoop, out pointNo, out circuitNo, out shortCircuitList, out openCircuitList))
                    {
                        InfoBox.ErrorMsg("检测失败,请检查设备连接");
                        Dispatcher.Invoke(new Action(delegate
                        {
                            LblStartMode.Content = "当前工作模式：OK等待，第一次请手动点击开始";
                            BtnStart.Content = "开  始";
                        }));
                        return;
                    }
                    if (pointNo == 0)
                    {
                        Dispatcher.Invoke(new Action(delegate
                        {
                            LblStartMode.Content = "请插上电缆";
                        }));
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(delegate
                        {
                            LblStartMode.Content = "OK等待中......";
                        }));
                        _needStopFlag = false;
                    }
                    Thread.Sleep(200);
                }

                if (shortCircuitList.Count == 0 && openCircuitList.Count == 0)
                {
                    isCheckOk = true;
                    Dispatcher.Invoke(new Action(delegate
                    {
                        RemoveErrorLine();
                        TbInfo.Text = "检测结果：OK";
                        LblStartMode.Content = "请取下电缆";
                    }));
                    Dispatcher.Invoke(new Action(delegate
                    {
                        HandleCheckResult(isCheckOk, shortCircuitResult, openCircuitList);
                    }));
                    while (pointNo != 0 && _manualStopFlag)
                    {
                        if (!CpldControl.Check.SampleCheck.ManualCheck(_circuitLoop, out pointNo, out circuitNo, out shortCircuitList, out openCircuitList))
                        {
                            CpldBase.InfoBox.ErrorMsg("检测失败,请检查设备连接");
                            Dispatcher.Invoke(new Action(delegate
                            {
                                LblStartMode.Content = "当前工作模式：OK等待，第一次请手动点击开始";
                                BtnStart.Content = "开  始";
                            }));
                            return;
                        }
                        Thread.Sleep(200);
                    }

                    _needStopFlag = true;
                }
                else
                {
                    isCheckOk = false;

                    var list = shortCircuitList;
                    Dispatcher.Invoke(new Action(delegate
                    {
                        try
                        {
                            CpldControl.DataParser.CommonParser.PointToCircuit(list, out shortCircuitResult);
                            CpldControl.DataParser.CommonParser.PointToPair(openCircuitList, out openCircuitResult);

                            InfoBox.RichTextMsg(out currErrorMsg, isCheckOk, pointNo, circuitNo, shortCircuitResult, openCircuitList, _phyAddrMapToDot);
                            TbInfo.Text = currErrorMsg;
                            if (_manualStartOnce != true) return;
                            if (prevErrorMsg.Equals(currErrorMsg)) return;
                            RemoveErrorLine();
                            DrawErrorLines(shortCircuitResult, openCircuitResult);
                            if (list.Count != 0)
                            {
                                InfoBox.PlaySound(isCheckOk);
                                //new ResultPopUp(isCheckOk, okAutoRelease, ngAutoRelease).ShowDialog();
                            }
                            prevErrorMsg = currErrorMsg;
                        }
                        catch (Exception e)
                        {
                            InfoBox.ErrorMsg(e.ToString());
                            throw;
                        }
                    }));
                }
                Thread.Sleep(200);
            }
        }

        bool _isDotMenuEnable = true;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = sender as TabControl;
            if (tab == null) return;
            switch (tab.SelectedIndex)
            {
                case 0:
                    if (_isDotMenuEnable)
                    {
                        foreach (var dot in _dots)
                        {
                            dot.RemoveDotMenu();
                        }
                    }
                    _isDotMenuEnable = false;
                    break;
                case 1:
                    AbortThread();
                    if (!_isDotMenuEnable)
                    {
                        foreach (var dot in _dots)
                        {
                            dot.AddDotMenu();
                        }
                    }
                    _isDotMenuEnable = true;
                    break;
                case 2:
                    AbortThread();
                    if (!_isDotMenuEnable)
                    {
                        foreach (var dot in _dots)
                        {
                            dot.AddDotMenu();
                        }
                    }
                    _isDotMenuEnable = true;
                    break;
                default:break;
            }
        }

        #endregion
    }
}
