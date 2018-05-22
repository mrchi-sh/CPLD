using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using CpldBase;
using CpldLog;

namespace CpldUI.SettingManager
{
    /// <summary>
    /// CpldSetting.xaml 的交互逻辑
    /// </summary>
    public partial class CpldSetting : Window
    {
        public bool IsSettingChanged { set; get; }

        public CpldSetting()
        {
            IsSettingChanged = false;
            InitializeComponent();
        }

        private void LoadCpldSetting()
        {
            string cpldAddress;
            string bartendPath;
            CpldDB.DbAppSetting.LoadCpldSetting(out cpldAddress,out bartendPath);

            IpString.Text = cpldAddress;
            TbBartendPath.Text = bartendPath;
        }

        private void LoadSettings()
        {
            if (Settings.IsDelayScan)            
                RbScanDelayTrue.IsChecked = true;           
            else
                RbScanDelayFalse.IsChecked = true;
            TbDelayTime.Text = Settings.DelayTime.ToString();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCpldSetting();
            LoadSettings();
        }
        
        private static bool UpdateSettingToFile()
        {
            try
            {
                var sw = new StreamWriter("cfg.ini", false);
                sw.WriteLine(Settings.IsDelayScan? "1": "0");
                sw.WriteLine(Settings.IsShareSerial? "1": "0");
                sw.WriteLine(Settings.DelayTime);
                sw.Close();
                return true;
            }
            catch (Exception ex) 
            {
                LogControl.LogError(ex);
                return false; 
            }
        }

        private bool UpdateSetting()
        {
            var cpldAddress = IpString.Text;
            var bartendPath = TbBartendPath.Text.Trim();

            //update common settings
            Settings.IsDelayScan = RbScanDelayTrue.IsChecked == true;
            try
            {
                Settings.DelayTime = Convert.ToInt32(TbDelayTime.Text);
            }
            catch (Exception)
            {
                CpldLog.LogControl.LogError("时间转换错误");
            }

            return UpdateSettingToFile() && CpldDB.DbAppSetting.UpdateSetting(cpldAddress, bartendPath);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var defaultPath = string.Empty;
                const string virthPath86 = @"\Program Files\Seagull\BarTender Suite\bartend.exe";
                if (File.Exists("C:" + virthPath86))
                {
                    defaultPath = "C:" + virthPath86;
                }
                if (File.Exists("D:" + virthPath86))
                {
                    defaultPath = "D:" + virthPath86;
                }
                const string virthPath64 = @"\Program Files (x86)\Seagull\BarTender Suite\bartend.exe";

                if (File.Exists("C:" + virthPath64))
                {
                    defaultPath = "C:" + virthPath64;
                }
                if (File.Exists("D:" + virthPath64))
                {
                    defaultPath = "D:" + virthPath64;
                }
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "bartend.exe|bartend.exe",
                    InitialDirectory = defaultPath,
                    Multiselect = false
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                TbBartendPath.Text = dialog.FileName.Replace('\\', '/');
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
        }
        
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!UpdateSetting()) return;
            InfoBox.InfoMsg("设置成功");
            BartendParams.BartendPath = TbBartendPath.Text.Trim();

            Params.ServerHost = IpString.Text;
            CpldSocket.CpldSocket.Reconnect();
        }

        private void btnSelectDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "*.sql|*.sql",
                    Multiselect = false
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                TbFilePath.Text = dialog.FileName.Replace('\\', '/');
            }
            catch (Exception ex)
            {
                InfoBox.InfoMsg(ex.ToString());
                CpldLog.LogControl.LogError(ex);
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            var mbr = MessageBox.Show("本操作将删除原数据库表中所有的数据,请谨慎使用！", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (mbr == MessageBoxResult.OK)
            {
                if (TbFilePath.Text.EndsWith("sql"))
                {
                    if(!CpldDB.DbAppSetting.ImportSqlFile(TbFilePath.Text))
                    {
                        InfoBox.InfoMsg("导入失败");
                        return;
                    }
                }
                else
                {
                    InfoBox.InfoMsg("请选择mysql文件");
                    return;
                }
            }
            else
            {
                return;
            }

            InfoBox.InfoMsg("导入成功");
        }
        
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            const string user = "root";
            const string pwd = "asdfghjkl";
            var server = Params.DbHost;
            const string database = "tds_data";
            var proc = new Process();

            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "*.sql|*.sql",
                    FileName = "tds_data.sql"
                };

                if (dialog.ShowDialog() != true)
                {
                    return;
                }

                var backupFile = dialog.FileName;

                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = string.Format("/c mysqldump --default-character-set=utf8 -h {0} -u {1} -p{2} {3} > {4}", server, user, pwd, database, backupFile);
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
                InfoBox.InfoMsg("备份成功");
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
        }
    }
}
