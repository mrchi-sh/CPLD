using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using CpldBase;
using CpldControl.Check;
using CpldDB;
using CpldUI.CheckManager.Base;

namespace CpldUI.CheckManager
{
    public class SampleViewModel : BindableObject
    {
        private List<List<string>> _shortCircuitInfoResult;
        private int _pointCount;
        private int _circuitCount;

        public CableInfo CableInfo { get; set; }
        public UserInfo UserInfo { get; set; }
        public ObservableCollection<BindCheckResult> CheckResults { get; set; }
        public int PointCount
        {
            get { return _pointCount; }
            set { SetProperty(ref _pointCount, value); }
        }
        public int CircuitCount
        {
            get { return _circuitCount; }
            set { SetProperty(ref _circuitCount, value); }
        }

        public bool Sample()
        {
            bool isNoShortCircuit;
            int pointNo;
            int circuitNo;
            if (!SampleCheck.BeginSampleCheck(out isNoShortCircuit, out _shortCircuitInfoResult, out pointNo, out circuitNo))
            {
                InfoBox.ErrorMsg("取样失败,请检查设备连接");
                return false;
            }

            CircuitCount = circuitNo;
            PointCount = pointNo;
            CheckResults.Clear();
            if (!isNoShortCircuit)
            {
                var id = 0;
                foreach (var tmpList in _shortCircuitInfoResult)
                {
                    CommonCheck.OrderCheckResult(tmpList);
                    var tmpCircuitLoop = tmpList.Aggregate("", (current, tmp) => current + (tmp + "-"));
                    ++id;
                    CheckResults.Add(new BindCheckResult { Id = id, CheckResult = tmpCircuitLoop.Remove(tmpCircuitLoop.Length - 1, 1) });
                }
            }
            InfoBox.PlaySound(true);
            return true;
        }

        public bool SaveCable()
        {
            if (string.IsNullOrEmpty(CableInfo.CableName))
            {
                InfoBox.ErrorMsg("请输入产品名");
                return false;
            }

            bool isProductNameExist;
            CacheGraph.IsCableNameExist(CableInfo, CableInfo.CableName, out isProductNameExist);
            if (isProductNameExist)
            {
                InfoBox.ErrorMsg("产品名已存在");
                return false;
            }

            CableInfo.ModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            CableInfo.CreateUser = UserInfo.UserName;
            if (_shortCircuitInfoResult == null)
            {
                InfoBox.InfoMsg("请取样");
                return false;
            }
            //加线
            CacheGraph.AddCable(CableInfo);
            var circuitId = 1;
            var dotId = 1;
            foreach (var circuit in _shortCircuitInfoResult)
            {
                //加回路
                var circuitInfo = new CircuitInfo
                {
                    ParentCableId = CableInfo.CableId,
                    CircuitId = circuitId
                };
                circuitInfo.Name = "回路" + circuitInfo.CircuitId;
                CacheGraph.AddCircuit(circuitInfo);

                foreach (var dot in circuit)
                {
                    //加点
                    var dotInfo = new DotInfo
                    {
                        ParentCableId = CableInfo.CableId,
                        ParentPlugId = 0,
                        ParentCircuitId = circuitId,
                        DotId = dotId,

                        PhyAddr = dot,
                        Name = "点" + dotId,
                        Position = new Point(20 + 20 * circuit.IndexOf(dot), 20 + 20 * circuitId)
                    };
                    CacheGraph.AddDot(dotInfo);
                    dotId++;
                }
                circuitId++;
            }
            InfoBox.InfoMsg(CacheGraph.CacheCopyCable() ? "保存成功" : "保存失败");
            return true;
        }
    }
    
    public partial class Sample : Window
    {
        private readonly SampleViewModel _viewModel;
        
        public Sample(UserInfo user)
        {
            InitializeComponent();
            _viewModel = new SampleViewModel
            {
                CableInfo = new CableInfo(),
                UserInfo = user,
                CheckResults = new ObservableCollection<BindCheckResult>()
            };
            Grid.DataContext = _viewModel;
            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            WaitBox wb = null;
            var t = new Thread(() =>
            {
                wb = new WaitBox();
                wb.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            _viewModel.Sample();
           
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveCable();
        }
    }
}
