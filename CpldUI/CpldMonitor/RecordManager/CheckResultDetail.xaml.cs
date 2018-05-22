using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
using CpldBase;
using CpldDB;

namespace CpldUI.RecordManager
{
    /// <summary>
    /// CheckResultDetail.xaml 的交互逻辑
    /// </summary>
    public partial class CheckResultDetail : Window
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OfReadwrite = 2;
        public const int OfShareDenyNone = 0x40;
        public readonly IntPtr HfileError = new IntPtr(-1);

        private readonly CheckItemExt _cable;
        
        public string CheckDateBegin
        {
            set
            {
                DatePickerBegin.Text = value;
            }
        }

        public string CheckDateEnd
        {
            set
            {
                DatePickerEnd.Text = value;
            }
        }
        
        private bool WriteExcelToFile(string excelPath)
        {
            var resultDetailList = GridCheckResult.ItemsSource as List<ResultDetail>;

            if (File.Exists(excelPath))
            {
                InfoBox.ErrorMsg("文件已存在");
                return false;
            }

            var xlApp = new Excel.Application();
            var workbooks = xlApp.Workbooks;
            var workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            var worksheet = (Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

            if (resultDetailList != null)
            {
                var workRange = worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[resultDetailList.Count+1, GridCheckResult.Columns.Count]];
                workRange.NumberFormatLocal = "@";
            }

            for (var i = 0; i < GridCheckResult.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = GridCheckResult.Columns[i].Header;
            }

            if (resultDetailList != null)
                for (var rowIndex = 0; rowIndex < resultDetailList.Count; rowIndex++)
                {
                    worksheet.Cells[rowIndex + 2, 1] = resultDetailList[rowIndex].CheckDate;
                    worksheet.Cells[rowIndex + 2, 2] = resultDetailList[rowIndex].ItemName;
                    worksheet.Cells[rowIndex + 2, 3] = resultDetailList[rowIndex].Username;
                    worksheet.Cells[rowIndex + 2, 4] = resultDetailList[rowIndex].BarContent;
                    worksheet.Cells[rowIndex + 2, 5] = resultDetailList[rowIndex].State;
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

        private void LoadResultDetails(string checkDateBegin, string checkDateEnd, string checkResult)
        {
            List<ResultDetail> resultDetailList;
            if (checkResult.Equals("全部"))
                checkResult = "";
            DbCheckResult.LoadResultDetails(_cable.ItemName, checkDateBegin, checkDateEnd, checkResult, out resultDetailList);
            GridCheckResult.ItemsSource = resultDetailList;
        }

        public CheckResultDetail(CheckItemExt cable)
        {
            _cable = cable;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var checkTimeBegin = Convert.ToDateTime(DatePickerBegin.SelectedDate).ToString("yy-MM-dd ") + " 00:00:00";
            var checkTimeEnd = Convert.ToDateTime(DatePickerEnd.SelectedDate).ToString("yy-MM-dd ") + " 23:59:59";

            var checkResult = CbCheckResult.Text;

            DatePickerBegin.Text = checkTimeBegin;
            DatePickerEnd.Text = checkTimeEnd;

            LoadResultDetails(checkTimeBegin, checkTimeEnd, checkResult);       
        }

        private void btnFilt_Click(object sender, RoutedEventArgs e)
        {
            var checkTimeBegin = Convert.ToDateTime(DatePickerBegin.SelectedDate).ToString("yy-MM-dd ") + " 00:00:00";
            var checkTimeEnd = Convert.ToDateTime(DatePickerEnd.SelectedDate).ToString("yy-MM-dd ") + " 23:59:59";
            var checkResult = CbCheckResult.Text;

            DatePickerBegin.Text = checkTimeBegin;
            DatePickerEnd.Text = checkTimeEnd;

            LoadResultDetails(checkTimeBegin, checkTimeEnd, checkResult);       
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
            InfoBox.InfoMsg(WriteExcelToFile(sfdSaveFile.FileName)?"保存成功":"保存失败");
            wb.Dispatcher.Invoke((Action)(() => wb.Close()));
        }
    }
}
