using System;
using System.Windows;
using CpldBase;
using CpldDB;

namespace CpldUI.CheckManager.GraphCheck
{

    /// <summary>
    /// GraphicalConfigCableInfo.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicalConfigCableInfo
    {
        public CableInfo CableInfo { get; set; }

        private readonly bool _flagIsAdd;
        public GraphicalConfigCableInfo(UserInfo user)
        {
            InitializeComponent();
            CableInfo = new CableInfo {CreateUser = user.UserName};
            _flagIsAdd = true;
            GpBoxName.Header = "添加产品";

        }

        public GraphicalConfigCableInfo(CableInfo cable, UserInfo user)
        {
            InitializeComponent();
            CableInfo = cable;
            CableInfo.CreateUser = user.UserName;
            TbProductName.Text = CableInfo.CableName;
            TbRemark.Text = CableInfo.Remark;
            _flagIsAdd = false;
            GpBoxName.Header = "修改产品";
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbProductName.Text.Trim()))
            {
                InfoBox.ErrorMsg("请输入产品名称");
                return;
            }
            bool isProductNameExist;
            if (!CacheGraph.IsCableNameExist(CableInfo, TbProductName.Text.Trim(), out isProductNameExist))
                return;

            if (isProductNameExist)
            {
                InfoBox.ErrorMsg("产品已存在");
                return;
            }

            CableInfo.CableName = TbProductName.Text.Trim();
            CableInfo.Remark = TbRemark.Text.Trim();
            CableInfo.ModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (_flagIsAdd)
            {
                CacheGraph.AddCable(CableInfo);
            }
            CacheGraph.CacheAddCable();
            
            InfoBox.InfoMsg("保存成功！");
            Close();
        }
    }
}
