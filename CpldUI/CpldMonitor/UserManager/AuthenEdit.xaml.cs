using System;
using System.Windows;

namespace CpldUI.UserManager
{
    /// <summary>
    /// AuthenEdit.xaml 的交互逻辑
    /// </summary>
    public partial class AuthenEdit : Window
    {
        private string username;
        private string userAuthen;
        private string userLevel;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string UserAuthen
        {
            get { return userAuthen; }
            set { userAuthen = value; }
        }

        public string UserLevel
        {
            get { return userLevel; }
            set { userLevel = value; }
        }

        public AuthenEdit()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            setAuthen();
        }

        private void loadAuthen()
        {
            try
            {
                int[] iUserAuthen = Array.ConvertAll<string, int>(userAuthen.Split(','), s => int.Parse(s));

                foreach (int tmpAuthen in iUserAuthen)
                {
                    switch (tmpAuthen)
                    {
                        case 0:
                            cb0.IsChecked = true;
                            continue;
                        case 1:
                            cb1.IsChecked = true;
                            continue;
                        case 2:
                            cb2.IsChecked = true;
                            continue;
                        case 3:
                            cb3.IsChecked = true;
                            continue;
                        case 4:
                            cb4.IsChecked = true;
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError("user level string parse error", ex);
                return;
            }           
        }

        private void setAuthen()
        {
            userAuthen = "";
            if (cb0.IsChecked == true)
                userAuthen += "0,";
            if (cb1.IsChecked == true)
                userAuthen += "1,";
            if (cb2.IsChecked == true)
                userAuthen += "2,";
            if (cb3.IsChecked == true)
                userAuthen += "3,";
            if (cb4.IsChecked == true)
                userAuthen += "4,";
            if (userLevel.Equals("管理员"))
                userAuthen += "5,";

            userAuthen = userAuthen.Remove(userAuthen.Length-1, 1);

            if(CpldDB.DbUser.SetUserAuthen(username, userAuthen))
            {
                CpldBase.InfoBox.InfoMsg("修改权限成功");
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Text = username;
            loadAuthen();
        }
    }
}
