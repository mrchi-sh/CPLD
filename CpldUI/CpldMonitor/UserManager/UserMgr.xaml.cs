using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CpldDB;

namespace CpldUI.UserManager
{
    /// <summary>
    /// UserMgr.xaml 的交互逻辑
    /// </summary>
    public partial class UserMgr : Window
    {        
        private UserAdd userAddHandler = null;
        private UserEdit userEditHandler = null;
        private AuthenEdit authenEditHandler = null;

        public UserMgr()
        {
            InitializeComponent();
        }

        private void loadUserData()
        {
            List<UserInfo> userInfoList = null;
            if (DbUser.LoadAllUser(out userInfoList))
            {
                gridUserMgr.ItemsSource = userInfoList;
            }     
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadUserData();  
        }

        private void userDataRefresh_onClosed(object sender, EventArgs e)
        {
            loadUserData();
        }

        private void btnUserAdd_Click(object sender, RoutedEventArgs e)
        {
            userAddHandler = new UserAdd();
            userAddHandler.Closed += new EventHandler(userDataRefresh_onClosed);
            userAddHandler.ShowDialog();
        }

        private void btnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            string username = "";
            string userLevel = "";

            if (gridUserMgr.SelectedItems.Count > 0)
            {
                username = (gridUserMgr.Columns[1].GetCellContent(gridUserMgr.SelectedItem) as TextBlock).Text;
                userLevel = (gridUserMgr.Columns[2].GetCellContent(gridUserMgr.SelectedItem) as TextBlock).Text;
                userEditHandler = new UserEdit();
                userEditHandler.Username = username;
                userEditHandler.UserLevel = userLevel;
                userEditHandler.Closed += new EventHandler(userDataRefresh_onClosed);
                userEditHandler.ShowDialog();
            }
            else
                CpldBase.InfoBox.InfoMsg("请选择需要修改的用户");
        }

        private void btnUserEdutAuthen_Click(object sender, RoutedEventArgs e)
        {
            string username = "";
            string userAuthen = "";
            string userLevel = "";

            if (gridUserMgr.SelectedItems.Count > 0)
            {
                username = (gridUserMgr.Columns[1].GetCellContent(gridUserMgr.SelectedItem) as TextBlock).Text;
                userLevel = (gridUserMgr.Columns[2].GetCellContent(gridUserMgr.SelectedItem) as TextBlock).Text;
                if (!DbUser.GetUserAuthen(username, out userAuthen))
                {
                    CpldBase.InfoBox.InfoMsg("读取用户权限错误，请联系管理员");
                    return;
                }

                authenEditHandler = new AuthenEdit();
                authenEditHandler.Username = username;
                authenEditHandler.UserLevel = userLevel;
                authenEditHandler.UserAuthen = userAuthen;
                authenEditHandler.ShowDialog();
            }
            else
                CpldBase.InfoBox.InfoMsg("请选择需要修改权限的用户");
        }

        private void btnUserDel_Click(object sender, RoutedEventArgs e)
        {
            string username = "";
            if (gridUserMgr.SelectedItems.Count > 0)
            {
                MessageBoxResult mbr = MessageBox.Show("确定要删除选中的账号吗？", "提示信息", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    username = (gridUserMgr.Columns[1].GetCellContent(gridUserMgr.SelectedItem) as TextBlock).Text;
                    if (!DbUser.DelUser(username))
                    {
                        CpldBase.InfoBox.InfoMsg("读取用户权限错误，请联系管理员");
                        return;
                    }
                    loadUserData();
                }
            }

        }
    }
}
