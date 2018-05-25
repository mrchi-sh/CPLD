using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace CpldUI.CheckManager
{
    /// <summary>
    /// PointSearch.xaml 的交互逻辑
    /// </summary>
    public partial class PointSearch : Window
    {
        private static bool _needStopFlag = true;
        private Thread _threadHandler;

        private void Search()
        {
            while (!_needStopFlag)
            {
                List<string> pointList;
                if (!CpldControl.Check.SampleCheck.BeginPointCheck(out pointList))
                {
                    continue;
                }

                if (pointList != null)
                {
                    Dispatcher.Invoke(new Action(
                        delegate
                        {
                            if (CheckRepeat.IsChecked == true)
                            {
                                var result = "";
                                foreach (var tmp in pointList)
                                    result += tmp + "-";
                                        
                                result = result.Remove(result.Length - 1);
                                if (pointList.Count >= 2)
                                    CpldBase.InfoBox.PlaySound(false);

                                if (LbPoint.Items.Contains(result)) return;
                                LbPoint.Items.Add(result);
                                SvPoint.ScrollToEnd();
                            }
                            else
                            {
                                var result = "";
                                foreach (var tmp in pointList)
                                    result += tmp + "-";
                                if (pointList.Count >= 2)
                                    CpldBase.InfoBox.PlaySound(false);

                                result = result.Remove(result.Length - 1);
                                LbPoint.Items.Add(result);

                                SvPoint.ScrollToEnd();
                            }
                        }
                    ));
                }
                Dispatcher.Invoke(new Action(
                    delegate
                    {
                        TbPointNo.Text = LbPoint.Items.Count.ToString();
                    }
                ));
                Thread.Sleep(200);
            }
        }

        public PointSearch()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AbortThread();
        }

        private void AbortThread()
        {
            if (_threadHandler != null && _threadHandler.IsAlive)
                _needStopFlag = true;
              
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_needStopFlag)
            {
                BtnStart.Content = "停  止";
                _needStopFlag = false;
                _threadHandler = new Thread(Search);
                _threadHandler.Start();
            }
            else
            {
                BtnStart.Content = "开  始";
                TbPointNo.Text = "0";
                AbortThread();
                LbPoint.Items.Clear();                             
            }
        }

        private void checkRepeat_Click(object sender, RoutedEventArgs e)
        {
            LbPoint.Items.Clear();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            LbPoint.Items.Clear();

        }
    }
}
