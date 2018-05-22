using System;
using System.Windows;
using CpldBase;
using CpldDB;

namespace CpldUI.UserManager
{
    /// <summary>
    /// UserEdit.xaml 的交互逻辑
    /// </summary>
    public partial class UserEdit : Window
    {
        private string username;
        private string userLevel;

        public UserEdit()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string UserLevel
        {
            get { return userLevel; }
            set { userLevel = value; }
        }

        private void editUser()
        {
            string password;
            int userLevel;
            string userAuthen;

            if (tbPassword1.Password.Equals(""))
            {
                InfoBox.InfoMsg("密码不能为空");
                tbPassword1.Focus();
            }
            else if (!tbPassword1.Password.Equals(tbPassword2.Password))
            {
                InfoBox.InfoMsg("输入的两次密码不一致");
                tbPassword1.Focus();
            }
            else
            {
                password = tbPassword1.Password.Trim();
                if (cbUserLevel.Text.Equals("普通员工"))
                {
                    userLevel = 1;
                    userAuthen = "0";
                }
                else if (cbUserLevel.Text.Equals("班长"))
                {
                    userLevel = 2;
                    userAuthen = "0,1,2,3";
                }
                else
                {
                    userLevel = 0;
                    userAuthen = "0,1,2,3,4,5";
                }

                if (DbUser.EditUser(username, password, userLevel, userAuthen))
                {
                    InfoBox.InfoMsg("修改成功");
                    this.Close();
                }
            }        
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.editUser();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsername.Text = username;
            cbUserLevel.Text = userLevel;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbPassword1.Focus();
        }
    }
}
