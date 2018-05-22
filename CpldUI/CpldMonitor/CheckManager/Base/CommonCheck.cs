using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CpldBase;
using CpldDB;
using CpldUI.Check.GraphCheck;
using CpldUI.CheckManager.GraphCheck;

namespace CpldUI.CheckManager.Base
{
    internal class CommonCheck
    {
        private static bool IsSameDay(string itemName)
        {
            return true;
        }


        internal static void UpdateBarList(string itemName, object o, DataTable dt)
        {
            Window scHandler = (GraphicalCableCheck) o;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var tb = scHandler.FindName("tbName_" + dt.Rows[i]["bar_field_name"]) as TextBox;

                if (Convert.ToBoolean(dt.Rows[i]["is_editable"])) //editable condition
                {
                    if ((BarFieldType) dt.Rows[i]["bar_field_type"] == BarFieldType.AutoIncrement)
                    {
                        var tmp = 0;
                        if (tb != null && int.TryParse(tb.Text, out tmp) == false)
                        {
                            InfoBox.ErrorMsg(dt.Rows[i]["bar_field_name"] + "的内容应为数字");
                            return;
                        }

                        var tmpLen = dt.Rows[i]["bar_field_value"].ToString().Length;
                        dt.Rows[i]["bar_field_value"] = tb.Text.Trim().PadLeft(tmpLen, '0');
                    }
                    else if ((BarFieldType) dt.Rows[i]["bar_field_type"] == BarFieldType.Constant)
                    {
                        dt.Rows[i]["bar_field_value"] = tb.Text.Trim();
                    }
                }
                else //not editable condition
                {
                    if ((BarFieldType) dt.Rows[i]["bar_field_type"] == BarFieldType.AutoIncrement)
                    {
                        if (!IsSameDay(itemName)) //TIANHAI return serial to zero
                        {
                            var tmpLen = dt.Rows[i]["bar_field_value"].ToString().Length;
                            if (tb == null) continue;
                            tb.Text = "1".PadLeft(tmpLen, '0');
                            dt.Rows[i]["bar_field_value"] = tb.Text;
                        }
                        else
                        {
                            var tmpLen = dt.Rows[i]["bar_field_value"].ToString().Length;
                            if (tb == null) continue;
                            tb.Text = (Convert.ToInt32(tb.Text) + 1).ToString().PadLeft(tmpLen, '0');
                            dt.Rows[i]["bar_field_value"] = tb.Text;
                        }
                    }
                    else if ((BarFieldType) dt.Rows[i]["bar_field_type"] == BarFieldType.Constant)
                        continue;

                    else //month,day,year
                    {
                        if (tb != null) dt.Rows[i]["bar_field_value"] = tb.Text;
                    }

                }
            }

            CpldControl.Bartend.BartendControl.UpdateBartendDb(itemName, dt);
        }

        internal static void AddCheckResult(object o, DataTable dt, string itemName, string state,
            string shortCircuitInfo, string openCircuitInfo, string username)
        {
            var barName = "";
            var barContent = "";
            Window scHandler = (GraphicalCableCheck) o;

            if (state.Equals("OK"))
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var tmpBarName = dt.Rows[i]["bar_field_name"].ToString();
                    var tb = scHandler.FindName("tbName_" + tmpBarName) as TextBox;
                    if (tb == null) continue;
                    var tmpBarContent = tb.Text;

                    barName += tmpBarName + "\t";
                    barContent += tmpBarContent + "\t";
                }

                if (dt.Rows.Count != 0)
                {
                    barName = barName.Remove(barName.Length - 1);
                    barContent = barContent.Remove(barContent.Length - 1);
                }
            }
            else
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["bar_field_type"]) != 1) continue;
                    var tmpBarName = dt.Rows[i]["bar_field_name"].ToString();
                    var tb = scHandler.FindName("tbName_" + tmpBarName) as TextBox;
                    if (tb == null) continue;
                    var tmpBarContent = tb.Text;

                    barName += tmpBarName + "\t";
                    barContent += tmpBarContent + "\t";
                }

                if (barName.Length != 0)
                {
                    barName = barName.Remove(barName.Length - 1);
                    barContent = barContent.Remove(barContent.Length - 1);
                }
            }

            DbCheckResult.AddCheckResult(itemName,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                state,
                shortCircuitInfo,
                openCircuitInfo,
                username,
                barName,
                barContent);

            CpldControl.CheckResultOutput.CheckResultOutput.ResultToTxt(username, itemName, barContent, state,
                shortCircuitInfo, openCircuitInfo); //write to file
        }


        internal static void OrderCheckResult(List<string> resultList)
        {
            for (var i = 1; i < resultList.Count; i++)
            {
                for (var j = 0; j < resultList.Count - i; j++)
                {
                    var tmp = "";
                    if (resultList[j].Substring(0, 1).CompareTo(resultList[j + 1].Substring(0, 1)) > 0)
                    {
                        tmp = resultList[j + 1];
                        resultList[j + 1] = resultList[j];
                        resultList[j] = tmp;
                    }

                    if (resultList[j].Substring(0, 1).CompareTo(resultList[j + 1].Substring(0, 1)) != 0 ||
                        Convert.ToInt32(resultList[j].Remove(0, 1)) <= Convert.ToInt32(resultList[j + 1].Remove(0, 1)))
                        continue;
                    tmp = resultList[j + 1];
                    resultList[j + 1] = resultList[j];
                    resultList[j] = tmp;
                }
            }
        }
    }
}
