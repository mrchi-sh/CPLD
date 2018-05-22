using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CpldBase;
using CpldDB;
using CpldLog;

namespace CpldUI.Check.Bar
{
    /// <summary>
    /// BarMgr.xaml 的交互逻辑
    /// </summary>
    /// 
    internal enum DbOperateType
    {
        Insert,
        Update
    }

    public partial class BarMgr : Window
    {
        private List<BarInfo> _barInfoList;
        private readonly CableInfo _cable;
     
        public BarMgr(CableInfo cable)
        {
            _cable = cable;
            InitializeComponent();
        }

        private void btnBarEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CpldControl.Bartend.BartendControl.LoadBartendDb(_cable.CableName))
            {
                InfoBox.ErrorMsg("读取Bartend数据库文件失败");
                return;
            }
            //create btw file
            if (!CpldControl.Bartend.BartendControl.CreateBtwFile(_cable.CableName))
            {
                InfoBox.ErrorMsg("创建Bartend文件失败");
                return;
            }

            CpldControl.Bartend.BartendControl.OpenBartendFile(_cable.CableName);
        }

        private void OperateDb(DbOperateType operateType, int index)
        {
            var barFieldName = TbBarFieldName.Text.Trim();
            if (barFieldName.Equals(""))
                InfoBox.ErrorMsg("条码字段名不能为空,请首先配置条形码确定字段内容");
            else
            {
                if (_barInfoList != null)
                {
                    if (operateType == DbOperateType.Insert)
                    {
                        if (_barInfoList.Any(tmp => tmp.BarFieldName.Equals(barFieldName)))
                        {
                            InfoBox.InfoMsg("条码字段名已存在");
                            return;
                        }
                    }
                    else
                    {
                        var tmpIndex = 0;
                        foreach (var tmp in _barInfoList)
                        {
                            if (tmp.BarFieldName.Equals(barFieldName) && tmpIndex != index)
                            {
                                InfoBox.InfoMsg("条码字段名已存在");
                                return;
                            }
                            tmpIndex++;
                        }
                    }

                    var fieldType = BarFieldType.AutoIncrement;
                    var fieldValue = "";
                    var iTmp = 0;

                    if (int.TryParse(TbBarValueLen.Text, out iTmp) == false)
                    {
                        InfoBox.ErrorMsg("数值长度必须为数字");
                        return;
                    }

                    if(iTmp <= 0)
                    {
                        InfoBox.ErrorMsg("数值长度必须大于0");
                        return;
                    }

                    switch (CbBar.SelectedIndex)
                    {
                        case 0:
                            fieldType = BarFieldType.AutoIncrement;
                            fieldValue = "0".PadLeft(iTmp, '0');
                            break;
                        case 1:
                            fieldType = BarFieldType.Constant;
                            fieldValue = TbBarFieldContent.Text.Trim();
                            break;
                        case 2:
                            fieldType = BarFieldType.Month;
                            fieldValue = "";
                            break;
                        case 3:
                            fieldType = BarFieldType.Day;
                            fieldValue = "";
                            break;
                        case 4:
                            fieldType = BarFieldType.Year;
                            fieldValue = "";
                            break;
                        default:break;
                    }
                    switch (operateType)
                    {
                        case DbOperateType.Insert:
                            if (ChkEditable.IsChecked != null && !DbBar.AddBarInfo(_cable.CableName, barFieldName, fieldType, fieldValue, (bool)ChkEditable.IsChecked))
                            {
                                InfoBox.ErrorMsg("添加条码字段失败");
                                return;
                            }

                            break;
                        case DbOperateType.Update:
                            var id = _barInfoList[index].Id;

                            if (ChkEditable.IsChecked != null && !DbBar.UpdateBarInfo(id, _cable.CableName, barFieldName, fieldType, fieldValue, (bool)ChkEditable.IsChecked))
                            {
                                InfoBox.ErrorMsg("修改条码字段失败");
                                return;
                            }

                            break;
                        default:
                            break;
                    }

                    if (!CpldControl.Bartend.BartendControl.LoadBartendDb(_cable.CableName))
                    {
                        InfoBox.ErrorMsg("读取Bartend数据库文件失败");
                        return;
                    }
                }
            }

            LoadBar();
        }

        private void barcodeAdd_Click(object sender, RoutedEventArgs e)
        {
            OperateDb(DbOperateType.Insert, -1);           
        }

        private void LoadBar()
        {
            if (!DbBar.LoadBarInfo(_cable.CableName, out _barInfoList))
                InfoBox.InfoMsg("加载失败");
            else
            {
                GridBarSelect.ItemsSource = _barInfoList;
            }
        }

        private void DelBar()
        {
            if (GridBarSelect.SelectedItems.Count > 0)
            {
                var mbr = MessageBox.Show("确定要删除选中的字段吗？", "提示信息", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (mbr != MessageBoxResult.OK) return;
                var barInfo = (BarInfo) GridBarSelect.SelectedItem;

                if (!DbBar.DelBarInfo(_cable.CableName, barInfo.BarFieldName))
                {
                    InfoBox.InfoMsg("删除失败");
                    return;
                }

                if (!CpldControl.Bartend.BartendControl.LoadBartendDb(_cable.CableName))
                {
                    InfoBox.ErrorMsg("读取Bartend数据库文件失败");
                    return;
                }

                LoadBar();
            }
            else
            {
                InfoBox.InfoMsg("请选择需要删除的字段");
            }

        }

        private void barcodeDel_Click(object sender, RoutedEventArgs e)
        {
            DelBar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbBarValueLen.IsEnabled = true;
            TbBarFieldContent.IsEnabled = false;
            LoadBar();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            TbBarFieldName.Focus();
        }

        private void tbBarFieldContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TbBarValueLen.Text = TbBarFieldContent.Text.Trim().Length.ToString();
        }

        private void gridBarSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = GridBarSelect.SelectedIndex;

            if (index == -1)
                return;

            ChkEditable.IsChecked = (_barInfoList[index].IsEditable == "是");  

            switch (_barInfoList[index].BarFieldType)
            {
                case "常量":
                    TbBarValueLen.IsEnabled = false;
                    TbBarFieldContent.IsEnabled = true;
                    TbBarFieldName.IsEnabled = true;
                    TbBarFieldName.Text = _barInfoList[index].BarFieldName;
                    TbBarFieldContent.Text = _barInfoList[index].BarFieldValue;
                    CbBar.Text = "常量";
                    TbBarValueLen.Text = "0";
                    break;
                case "自增数值":
                    TbBarValueLen.IsEnabled = true;
                    TbBarFieldContent.IsEnabled = false;
                    TbBarFieldContent.Text = "";
                    TbBarFieldName.IsEnabled = true;
                    TbBarFieldName.Text = _barInfoList[index].BarFieldName;
                    CbBar.Text = "自增数值";
                    TbBarValueLen.Text = _barInfoList[index].BarFieldValue.Length.ToString();

                    //tbBarValueLen.SelectAll();
                    //tbBarValueLen.Focus();
                    break;
                case "日期":
                    TbBarValueLen.IsEnabled = false;
                    TbBarValueLen.Text = "2";
                    TbBarFieldContent.Text = "";
                    TbBarFieldContent.IsEnabled = false;
                    TbBarFieldName.IsEnabled = false;
                    TbBarFieldName.Text = _barInfoList[index].BarFieldName;
                    CbBar.Text = _barInfoList[index].BarFieldName;
                    break;
                    default:break;
            }
        }

        private void cbBar_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                TbBarFieldContent.Text = "";
                ChkEditable.IsChecked = false;
                ChkEditable.IsEnabled = true;

                switch (CbBar.SelectedIndex)
                {
                    case 0:
                        TbBarValueLen.IsEnabled = true;
                        TbBarFieldContent.IsEnabled = false;
                        TbBarFieldContent.Text = "";
                        TbBarFieldName.IsEnabled = true;
                        TbBarFieldName.Text = "";
                        TbBarValueLen.Text = "";
                        break;
                    case 1:
                        TbBarValueLen.IsEnabled = false;
                        TbBarFieldContent.IsEnabled = true;
                        TbBarFieldName.IsEnabled = true;
                        TbBarFieldName.Text = "";
                        TbBarValueLen.Text = "0";
                        break;
                    case 2:
                        TbBarValueLen.IsEnabled = false;
                        TbBarValueLen.Text = "2";
                        TbBarFieldContent.Text = "";
                        TbBarFieldContent.IsEnabled = false;
                        TbBarFieldName.IsEnabled = false;
                        TbBarFieldName.Text = "月";
                        ChkEditable.IsEnabled = false;
                        break;
                    case 3:
                        TbBarValueLen.IsEnabled = false;
                        TbBarValueLen.Text = "2";
                        TbBarFieldContent.Text = "";
                        TbBarFieldContent.IsEnabled = false;
                        TbBarFieldName.IsEnabled = false;
                        TbBarFieldName.Text = "日";
                        ChkEditable.IsEnabled = false;
                        break;
                    case 4:
                        TbBarValueLen.IsEnabled = false;
                        TbBarValueLen.Text = "2";
                        TbBarFieldContent.Text = "";
                        TbBarFieldContent.IsEnabled = false;
                        TbBarFieldName.IsEnabled = false;
                        TbBarFieldName.Text = "年";
                        ChkEditable.IsEnabled = false;
                        break;
                    default: break;
                }
                
            }
            catch (Exception exception)
            {
                LogControl.LogError(exception);
            }
        }

        private void barcodeEd_Click(object sender, RoutedEventArgs e)
        {
            var index = GridBarSelect.SelectedIndex;
            OperateDb(DbOperateType.Update, index);
            GridBarSelect.SelectedIndex = index;
        }
            
    }
}
