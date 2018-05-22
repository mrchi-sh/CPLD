using System;
using System.Windows;
using CpldBase;
using CpldDB;

namespace CpldUI.UserManager
{
    /// <summary>
    /// UserAdd.xaml 的交互逻辑
    /// </summary>
    public partial class UserAdd : Window
    {
        public UserAdd()
        {
            InitializeComponent();
        }

        private void addUser()
        {
            string username;
            string password;
            int userLevel;
            string userAuthen;

            if (tbUsername.Text.Trim().Equals(""))
            {
                InfoBox.InfoMsg("用户名不能为空");
                tbUsername.Focus();
            }
            else if (tbPassword1.Password.Equals(""))
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
                username = tbUsername.Text.Trim();
                password = tbPassword1.Password.Trim();
                if (cbUserLevel.Text.Equals("普通员工"))
                {
                    userLevel = 1;
                    userAuthen = "0";
                }
                else if(cbUserLevel.Text.Equals("班长"))
                {
                    userLevel = 2;
                    userAuthen = "0,1,2,3";
                }
                else
                {
                    userLevel = 0;
                    userAuthen = "0,1,2,3,4,5";
                }

                if (!DbUser.AddUser(username, password, userLevel, userAuthen)) return;
                InfoBox.InfoMsg("添加成功");
                Close();
            }        
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            addUser();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbUsername.Focus();
        }
    }
}
