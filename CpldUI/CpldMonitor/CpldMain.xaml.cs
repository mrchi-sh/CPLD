using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CpldControl.Check;
using CpldDB;
using CpldLog;
using CpldUI.CheckManager;
using CpldUI.CheckManager.GraphCheck;
using CpldUI.SettingManager;

namespace CpldUI
{
    /// <summary>
    /// CpldMain.xaml 的交互逻辑
    /// </summary>
    public partial class CpldMain : Window
    {
        private UserInfo _user;
        private int _boardCount = -1;
        private Thread _checkConnStateThread;

        public CpldMain()
        {
            InitializeComponent();
        }

        #region WindowEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            BtnLogout.IsEnabled = false;
            DbCommon.InitDbConn();
            var cache = new CacheGraph();
            if (!cache.CacheInit())
            {
                LogControl.LogError("缓存数据初始化失败");
                return;
            }
            DisableAllOperation();

            LoadMachineSetting();

            _checkConnStateThread = new Thread(CheckConnState);
            _checkConnStateThread.Start();

            if (!CpldBase.CustomizeVersion.Debug) return;
            TbUsername.Text = "admin";
            TbPassword.Password = "1";
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            CpldBase.ThreadFlags.ConnStatCheck = false;
            try
            {
                _checkConnStateThread.Abort();
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _checkConnStateThread.Abort();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            TbUsername.Focus();
            CpldSocket.CpldSocket.ReceiveTimeout = 5000;
            CpldBase.ThreadFlags.ConnStatCheck = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            CpldSocket.CpldSocket.ReceiveTimeout = Convert.ToInt32(CpldBase.TimeoutConfig.TimeoutHt[_boardCount.ToString()]) * 1000;
            CpldBase.ThreadFlags.ConnStatCheck = false;
        }
        #endregion


        private void LoadMachineSetting()
        {
            var cpldAddress = "127.0.0.1";
            var bartendPath = "";
            DbAppSetting.LoadCpldSetting(out cpldAddress, out bartendPath);
            CpldBase.Params.ServerHost = cpldAddress;
            CpldBase.BartendParams.BartendPath = bartendPath;

            CpldIp.Text = CpldBase.Params.ServerHost;
        }

        private void CheckConnState()
        {
            while (true)
            {
                if (!CpldBase.ThreadFlags.ConnStatCheck)
                {
                    Thread.Sleep(5000);
                    continue;
                }

                Dispatcher.Invoke(new Action(
                    delegate
                    {
                        LoadBoardCount();
                        if (CommonControl.CheckSocketState())
                        {
                            CpldStatus.Text = "在线";
                            ImgCpldStatus.Source = new BitmapImage(new Uri("Icon/cpldOnline.png", UriKind.RelativeOrAbsolute));
                            if (CpldPoint.Text == "未知")
                                LoadBoardCount();
                        }
                        else
                        {
                            CpldStatus.Text = "连接中..";
                            CpldPoint.Text = "未知";
                            ImgCpldStatus.Source = new BitmapImage(new Uri("Icon/cpldOffline.png", UriKind.RelativeOrAbsolute));
                            OpenConnection();
                        }
                    }
                ));
                Thread.Sleep(5000);
            }
        }

        private void LoadBoardCount()
        {
            if (!CommonControl.LoadBoardCount(out _boardCount)) return;
            CpldPoint.Text = _boardCount == -1 ? "未知" : (_boardCount * 128).ToString();
        }

        private void OpenConnection()
        {
            CommonControl.OpenConnection();

            if (CommonControl.CheckSocketState())
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    CpldStatus.Text = "在线";
                    ImgCpldStatus.Source = new BitmapImage(new Uri("Icon/cpldOnline.png", UriKind.RelativeOrAbsolute));
                    LoadBoardCount();
                }
            ));
            }
        }


        private void OnCpldSettingClosed(object sender, EventArgs e)
        {
            CpldIp.Text = CpldBase.Params.ServerHost;
        }

        private void DisableAllOperation()
        {
            BtnSCheck.IsEnabled = false;
            BtnSelfCheck.IsEnabled = false;
            BtnSample.IsEnabled = false;
            BtnRecord.IsEnabled = false;
            BtnSetting.IsEnabled = false;
            BtnUserMgr.IsEnabled = false;
        }

        private void EnableVerifiedOperation(IEnumerable<int> oper)
        {
            foreach (var tmpOper in oper)
            {
                switch (tmpOper)
                {
                    case 0:
                        BtnSCheck.IsEnabled = true;
                        break;
                    case 1:
                        BtnSample.IsEnabled = true;
                        break;
                    case 2:
                        BtnSelfCheck.IsEnabled = true;
                        break;
                    case 3:
                        BtnRecord.IsEnabled = true;
                        break;
                    case 4:
                        BtnSetting.IsEnabled = true;
                        break;
                    case 5:
                        BtnUserMgr.IsEnabled = true;
                        break;
                    default: break;
                }
            }
        }


        #region ButtonClick
        private void btnSelfCheck_Click(object sender, RoutedEventArgs e)
        {
            new SelfCheck().ShowDialog();
        }

        private void btnUserManager_Click(object sender, RoutedEventArgs e)
        {
            new UserManager.UserMgr().ShowDialog();
        }

        private void btnSample_Click(object sender, RoutedEventArgs e)
        {
            new Sample(_user).ShowDialog();
        }

        private void btnStandardCheck_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new GraphicalMangementCable(_user).ShowDialog();
            Show();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            new RecordManager.CheckResultManager().ShowDialog();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            var csHandler = new CpldSetting();
            csHandler.Closed += OnCpldSettingClosed;
            csHandler.ShowDialog();
        }
        #endregion



        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool isVerified;

            if (!DbUser.UserLogin(TbUsername.Text.Trim(), TbPassword.Password.Trim(), out isVerified, out _user))
                return;
            if (_user.UserLevel == 0)
            {
                App.UnlockOper();
            }
            else
            {
                App.LockOper();
            }

            if (!isVerified)
            {
                CpldBase.InfoBox.InfoMsg("用户名或密码错误");
                return;
            }

            try
            {
                var iUserAuthen = Array.ConvertAll(_user.UserAuthen.Split(','), int.Parse);
                EnableVerifiedOperation(iUserAuthen);
                BtnLogin.IsEnabled = false;
                BtnLogout.IsEnabled = true;
                TbUsername.IsEnabled = false;
                TbPassword.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LogControl.LogError("user level string parse error", ex);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            DisableAllOperation();
            TbUsername.Text = "";
            TbPassword.Password = "";
            BtnLogout.IsEnabled = false;
            BtnLogin.IsEnabled = true;
            TbUsername.IsEnabled = true;
            TbPassword.IsEnabled = true;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var mbr = MessageBox.Show("确认关闭计算机？", "提示信息", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (mbr != MessageBoxResult.OK) return;
                var mp = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                mp.Start();
                mp.StandardInput.WriteLine("shutdown -s -t 0");
            }
            catch (Exception)
            {
                LogControl.LogError("Close Computer Cxception");
            }
        }
    }
}
