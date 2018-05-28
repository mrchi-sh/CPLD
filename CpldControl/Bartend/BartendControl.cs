using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data;
using CpldDB;

namespace CpldControl.Bartend
{
    public class BartendControl
    {
        private static readonly string RootPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        private static void WriteBartendTxtFile(string itemName, DataTable dt)
        {
            var btwDbFile = RootPath + "\\Btw\\BtwStandard\\" + itemName + ".txt";
            try
            {
                if (!File.Exists(btwDbFile))
                {
                    var fs = File.Create(btwDbFile);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
                var writer = new StreamWriter(btwDbFile, false, Encoding.Default);
                var barFieldName = "";
                var barFieldValue = "";
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    barFieldName += "\"" + dt.Rows[i]["bar_field_name"] + "\",";
                    barFieldValue += "\"" + dt.Rows[i]["bar_field_value"] + "\",";
                }
                if(!barFieldName.Equals(""))
                    barFieldName = barFieldName.Remove(barFieldName.Length - 1);
                if(!barFieldValue.Equals(""))
                    barFieldValue = barFieldValue.Remove(barFieldValue.Length - 1);

                writer.WriteLine(barFieldName);
                writer.WriteLine(barFieldValue);
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
            
        }

        public static bool LoadBartendDb(string itemName)
        {
            DataTable dt;
            return LoadBartendDb(itemName, out dt);
        }

        public static bool LoadBartendDb(string itemName, out DataTable dt)
        {
            dt = new DataTable();

            if (!DbBar.LoadBarInfo(itemName, out dt))
                return false;
            WriteBartendTxtFile(itemName, dt);
            return true;
        }

        public static bool UpdateBartendDb(string itemName,DataTable dt)
        {
            if(!DbBar.UpdateBarInfo(dt))
                return false;
            WriteBartendTxtFile(itemName, dt);
            return true;
        }
        
        public static void OpenBartendFile(string btwFileName)
        {
            try
            {
                var btwFile = RootPath + "\\Btw\\BtwStandard\\" + btwFileName + ".btw";
                Process.Start(btwFile);
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
        }

        public static bool CreateBtwFile(string btwFileName)
        {
            try
            {
                var btwFile = RootPath + "\\Btw\\BtwStandard\\" + btwFileName + ".btw";
                if (File.Exists(btwFile)) return true;
                var fs = File.Create(btwFile);
                fs.Flush();
                fs.Close();
                fs.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
                return false;
            }
        }

        public static void Print(string btwFile, string bartendPath)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = bartendPath,
                    Arguments = " /f=\"" + btwFile + "\" /p /x"
                }
            };
            process.Start();
            var killMyProcess = Process.GetProcessesByName("bartend.exe");
            foreach (var p in killMyProcess)
            {
                p.Kill();
            }
        }    

        public static void PrintBar(string btwFileName)
        {
          
            var bartendPath = CpldBase.BartendParams.BartendPath;
            var btwFile = RootPath + "\\Btw\\BtwStandard\\" + btwFileName + ".btw";
            try
            {
                if (!File.Exists(btwFile))
                {
                    return;
                }
                if (!File.Exists(bartendPath))
                {
                    return;
                }
               
                Print(btwFile, bartendPath);
              
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
        }

        public static void DelBartendFiles(string itemName)
        {

            var btwFile = RootPath + "\\Btw\\BtwStandard\\" + itemName + ".btw";
            var btwDbFile = RootPath + "\\Btw\\BtwStandard\\" + itemName + ".txt";

            try
            {
                if (File.Exists(btwDbFile))
                {
                    File.Delete(btwDbFile);
                }

                if( File.Exists(btwFile))
                {
                    File.Delete(btwFile);
                }
            }
            catch (Exception ex)
            {
                CpldLog.LogControl.LogError(ex);
            }
        }
    }
}
