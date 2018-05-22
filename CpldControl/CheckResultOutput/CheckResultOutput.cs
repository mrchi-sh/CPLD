using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CpldControl.CheckResultOutput
{
    public class CheckResultOutput
    {
        private static readonly string RootPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public static bool ResultToTxt(string username, string itemName, string barcode, string state, string shortCircuit, string openCircuit)
        {
            var resultRootPath = RootPath + "\\checkResult";
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            const string checkMethod = "标准检测";

            if (!Directory.Exists(resultRootPath)) return false;

            var resultPath = resultRootPath + "\\" + currentDate; 
            if(!Directory.Exists(resultPath))
                Directory.CreateDirectory(resultPath);
            var txtFilePath = resultPath +"\\"+ currentDate + "_" + checkMethod + "_" + itemName + "_" + username + ".txt";
            if (!File.Exists(txtFilePath))
                File.Create(txtFilePath).Close();

            //append content
            var resultContent = "";
            if (state.Equals("OK"))
                resultContent = DateTime.Now.ToString("HH:mm:ss") + "," + barcode.Replace("\t", "") + ":" + state;
            else
                resultContent = DateTime.Now.ToString("HH:mm:ss") + "," + barcode.Replace("\t", "") + ":" + state +", 短路：" + shortCircuit + ", 断路：" + openCircuit;

            var sw = new StreamWriter(txtFilePath,true);
            sw.WriteLine(resultContent);
            sw.Close();
                              
            return true;

        }
    }
}
