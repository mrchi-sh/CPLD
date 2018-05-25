using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using CpldBase;
using CpldControl.Check;
using CpldDB;
using CpldUI.Check;

namespace CpldUI.CheckManager
{
    /// <summary>
    /// SelfCheck.xaml 的交互逻辑
    /// </summary>
    public partial class SelfCheck : Window
    {
        public ObservableCollection<BindCheckResult> CheckResults;

        public SelfCheck()
        {
            InitializeComponent();
            CheckResults = new ObservableCollection<BindCheckResult>();
            GridSelfCheck.ItemsSource = CheckResults;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            BtnOk.IsEnabled = false;

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
            List<List<string>> shortCircuitInfoResult;
            bool isSelfCheckOk;
            if (!SampleCheck.BeginSampleCheck(out isSelfCheckOk, out shortCircuitInfoResult, out pointNo, out circuitNo))
            {
                InfoBox.ErrorMsg("自检失败,请检查设备连接");
                wb.Dispatcher.Invoke((Action)(() => wb.Close()));
                return;
            }


            if(!isSelfCheckOk)
            {
                CheckResults.Clear();
                var id = 0;
                foreach (var tmpList in shortCircuitInfoResult)
                {
                    var tmpResult = tmpList.Aggregate("", (current, tmp) => current + (tmp + "-"));
                    ++id;
                    CheckResults.Add(new BindCheckResult
                    {
                        Id = id,
                        CheckResult = tmpResult.Remove(tmpResult.Length - 1, 1)
                    });
                }
            }
           
            BtnOk.IsEnabled = true;
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
            InfoBox.PlaySound(isSelfCheckOk);
            new ResultPopUp(isSelfCheckOk, true, true).ShowDialog();
            
        }

        private void btnPointSearch_Click(object sender, RoutedEventArgs e)
        {
            new PointSearch().ShowDialog();
        }
    }
}
