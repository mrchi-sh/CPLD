using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
using CpldBase;
using CpldDB;

namespace CpldUI.RecordManager
{
    /// <summary>
    /// CheckResultManager.xaml 的交互逻辑
    /// </summary>
    public partial class CheckResultManager : Window
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OfReadwrite = 2;
        public const int OfShareDenyNone = 0x40;
        public readonly IntPtr HfileError = new IntPtr(-1);

        public CheckResultManager()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbItemName.Focus();
        }

        private void ResetDate()
        {            
            datePickerBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yy-MM-dd ") + " 00:00:00");
            datePickerEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yy-MM-dd ") + " 23:59:59");
        }

        private void FiltData()
        {
            var itemName = tbItemName.Text.Trim();
            var checkDateBegin = Convert.ToDateTime(datePickerBegin.SelectedDate).ToString("yy-MM-dd ") + " 00:00:00";
            var checkDateEnd = Convert.ToDateTime(datePickerEnd.SelectedDate).ToString("yy-MM-dd ") + " 23:59:59";
            
            List<CheckItemExt> checkItemList;
            DbCheckResult.LoadCheckItemList(itemName, checkDateBegin, checkDateEnd, out checkItemList);

            GridCheckResult.ItemsSource = checkItemList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResetDate();
            FiltData();
        }

        private void btnFilt_Click(object sender, RoutedEventArgs e)
        {
            FiltData();
        }

        private void gridCheckResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridCheckResult.SelectedItems.Count <= 0) return;
            var selectItem = (CheckItemExt) GridCheckResult.SelectedItem;
            var tmpHandler = new CheckResultDetail(selectItem)
            {
                CheckDateBegin = datePickerBegin.Text,
                CheckDateEnd = datePickerEnd.Text
            };

            tmpHandler.ShowDialog();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var sfdSaveFile = new Microsoft.Win32.SaveFileDialog
            {
                RestoreDirectory = true,
                DefaultExt = "xlsx",
                Filter = "Excel文件(*.xlsx)|*.xlsx",
                FileName = "*.xlsx"
            };
            //设置保存文件的格式
            if (sfdSaveFile.ShowDialog() != true)
            {
                return;
            }

            if (File.Exists(sfdSaveFile.FileName))
            {
                var vHandle = _lopen(sfdSaveFile.FileName, OfReadwrite | OfShareDenyNone);
                if (vHandle == HfileError)
                {
                    InfoBox.ErrorMsg("文件已打开，请关闭后导出！");
                    CloseHandle(vHandle);
                    return;
                }

                CloseHandle(vHandle);
                File.Delete(sfdSaveFile.FileName);
            }

            WaitBox wb = null;
            var t = new Thread(() =>
            {
                wb = new WaitBox();
                wb.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            InfoBox.InfoMsg(WriteExcelToFile(sfdSaveFile.FileName)?"导出成功":"导出失败");
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
        }

        private void tbItemName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbItemName.Text.Equals(""))
                FiltData();
        }
        
        private bool WriteExcelToFile(string excelPath)
        {
            var resultDetailList = GridCheckResult.ItemsSource as List<CheckItemExt>;

            if (File.Exists(excelPath))
            {
                InfoBox.InfoMsg("文件已存在!");
                return false;
            }
            var xlApp = new Excel.Application();
            var workbooks = xlApp.Workbooks;
            var workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            var worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

            if (resultDetailList != null)
            {
                var workRange = worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[resultDetailList.Count + 1, GridCheckResult.Columns.Count]];
                workRange.NumberFormatLocal = "@";
            }

            for (var i = 0; i < GridCheckResult.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = GridCheckResult.Columns[i].Header;
            }

            if (resultDetailList != null)
                for (var rowIndex = 0; rowIndex < resultDetailList.Count; rowIndex++)
                {
                    worksheet.Cells[rowIndex + 2, 1] = resultDetailList[rowIndex].CheckMethod;
                    worksheet.Cells[rowIndex + 2, 2] = resultDetailList[rowIndex].PartNo;
                    worksheet.Cells[rowIndex + 2, 3] = resultDetailList[rowIndex].ItemName;
                    worksheet.Cells[rowIndex + 2, 4] = resultDetailList[rowIndex].OkTime;
                    worksheet.Cells[rowIndex + 2, 5] = resultDetailList[rowIndex].NgTime;
                }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。

            try
            {
                object misValue = System.Reflection.Missing.Value;
                workbook.SaveAs(excelPath,
                    misValue,
                    misValue,
                    misValue,
                    misValue,
                    misValue,
                    Excel.XlSaveAsAccessMode.xlExclusive,
                    misValue,
                    misValue,
                    misValue,
                    misValue,
                    misValue);

                return true;
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
                return false;
            }
            finally
            {
                workbook.Close(true);
                xlApp.Quit();
                //Kill(xlApp);
                GC.Collect();//强行销毁                
            }
        }
        
    }
}
