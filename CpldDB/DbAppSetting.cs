using System;
using System.Data;
using System.IO;
using CpldBase;
using CpldLog;

namespace CpldDB
{
    public class DbAppSetting:DbCommon
    {
        public static bool LoadCpldSetting(out string cpldAddress,out string bartendPath)
        {
            cpldAddress = "127.0.0.1";
            bartendPath = "";
            const string querySql = "SELECT cpld_host,bartend_path FROM machine_setting";
            DataTable dt;
            if(!QueryData(querySql, out dt))
                return false;
            cpldAddress = dt.Rows[0][0].ToString();
            bartendPath = dt.Rows[0][1].ToString();
            return true;
        }
      
        public static bool UpdateSetting(string cpldAddress, string bartendPath)
        {
            var querySql = "UPDATE machine_setting SET cpld_host = '" + cpldAddress + "', bartend_path = '" +
                           bartendPath + "'";
            return NonQueryData(querySql);
        }

        public static bool ImportSqlFile(string varFileName)
        {
            const string sql = "DROP DATABASE IF EXISTS tds_data; CREATE DATABASE tds_data";
            if (!NonQueryData(sql))
            {                       
                return false;
            }

            using (var reader = new StreamReader(varFileName, System.Text.Encoding.GetEncoding("utf-8")))
            {
                try
                {
                    var line = "";
                    while (true)
                    {
                        if (line.EndsWith(";"))
                            line = "";

                        var l = reader.ReadLine();

                        if (l == null) break;
                        l = l.TrimEnd();
                        if (l == "") continue;
                        if (l.StartsWith("--")) continue;
                        line += l;
                        if (!line.EndsWith(";")) continue;
                        if (line.StartsWith("/*!"))
                        {
                            continue;
                        }
                        NonQueryData(line);
                    }
                }
                catch (Exception)
                {
                    InfoBox.ErrorMsg("导入失败");
                    LogControl.LogError("导入数据失败");
                    return false;
                }
            }
            return true;
        }
    }
}
