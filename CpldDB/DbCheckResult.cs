using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Data;
using System.Dynamic;
using System.Collections;
using System.ComponentModel;

namespace CpldDB
{
    public abstract class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return;
            item = value;
            OnPropertyChanged(propertyName);
        }
    }
    public class BindCheckResult : BindableObject
    {
        private int _id;
        private string _checkResult = string.Empty;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id,value);}
        }
        public string CheckResult 
        {
            get { return _checkResult; }
            set
            {
                SetProperty(ref _checkResult,value);
            }
        }
    }
    

    public class CheckItemExt
    {
        public string ItemName { get; set; }
        public string CheckMethod { get; set; }
        public int OkTime { get; set; }
        public int NgTime { get; set; }
        public string PartNo { get; set; }
    }

    public class ResultDetail:DynamicObject
    {
        public string ItemName { get; set; }
        public string State { get; set; }
        public string ShortCircuit { get; set; }
        public string OpenCircuit { get; set; }
        public string Username { get; set; }
        public string CheckDate { get; set; }
        public string BarContent { get; set; }
     
    }

    public class DbCheckResult:DbCommon
    {
        public static bool CheckDbPartitionState()
        {
            var querySql = "SELECT PARTITION_DESCRIPTION FROM information_schema.partitions where table_name = 'check_result'";
            
            DataTable dt;
            if(!QueryData(querySql, out dt))
                return false;
            var currentYear = (Convert.ToInt32(DateTime.Now.ToString("yyyy"))+1).ToString() ;
           
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (!currentYear.Equals(dt.Rows[i][0])) continue;
                return true;
            }
           

            querySql = "ALTER TABLE check_result drop partition p9999; "+                    
                       "ALTER TABLE check_result add partition (partition p"+currentYear+" values less than ("+ currentYear +"));" +
                       "ALTER TABLE check_result add partition ( partition p9999 values less than (MAXVALUE))";
            return NonQueryData(querySql);
        }

        public static bool LoadCheckItemList(string itemName, string checkDateBegin, string checkDateEnd, out List<CheckItemExt> checkItemList)
        {
            var querySql = "SELECT item_name, check_type, state, id  FROM check_result WHERE item_name LIKE '%" +
                           itemName + "%'" +
                           " AND check_date BETWEEN '" + checkDateBegin + "' AND  '" + checkDateEnd + "'";
                            
            DataTable dt;
            var ht = new Hashtable();
            checkItemList = new List<CheckItemExt>();

            if (!QueryData(querySql, out dt))
                return false;

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var key = dt.Rows[i][0] + "\t" + dt.Rows[i][1];
                if (!ht.ContainsKey(key))
                {
                    var okTime = 0;
                    var ngTime = 0;
                 
                    if (dt.Rows[i][2].ToString().Equals("OK"))
                        okTime++;
                    else
                        ngTime++;

                    ht.Add(key, new CheckItemExt
                    {
                        ItemName = dt.Rows[i][0].ToString(),
                        CheckMethod = "标准检测",
                        OkTime = okTime,
                        NgTime = ngTime,
                    });
                }
                else
                {
                    if (dt.Rows[i][2].ToString().Equals("OK"))
                        ((CheckItemExt) ht[key]).OkTime++;
                    else
                    {
                        ((CheckItemExt) ht[key]).NgTime++;
                    }
                }
            }

            checkItemList.AddRange(ht.Values.Cast<CheckItemExt>());
            return true;
        }
        
        public static bool AddCheckResult(string itemName, string checkDate, string state, string shortCircuitInfo, 
                                          string openCircuitInfo, string username, string barName, string barContent)
        {
            var querySql = "INSERT INTO check_result (item_name, check_date, state, short_circuit_info, open_circuit_info, username,  bar_name, bar_content) " +
                              "VALUES ('" + itemName + "', '" 
                           + checkDate + "', '" 
                           + state + "', '" 
                           + shortCircuitInfo + "', '" 
                           + openCircuitInfo + "','" 
                           + username + "', '" 
                           + barName + "', '" 
                           + barContent + "')";

            return NonQueryData(querySql);
        }

      
        public static bool LoadResultDetails(string itemName, string checkDateBegin, string checkDateEnd, string checkResult, out List<ResultDetail> resultDetailList)
        {
            var querySql =
                "SELECT item_name, check_date, state, short_circuit_info, open_circuit_info, username, bar_content FROM check_result" +
                " WHERE item_name = '" + itemName + "' AND state LIKE '%" + checkResult +
                "%' AND check_date BETWEEN '" + checkDateBegin + "' AND  '" + checkDateEnd +
                "' ORDER BY check_date DESC";
            DataTable dt;

            resultDetailList = new List<ResultDetail>();

            if (!QueryData(querySql, out dt))
                return false;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                resultDetailList.Add(new ResultDetail()
                {
                    ItemName = dt.Rows[i][0].ToString(),
                    CheckDate = dt.Rows[i][1].ToString(),
                    State = dt.Rows[i][2].ToString(),
                    ShortCircuit = dt.Rows[i][3].ToString(),
                    OpenCircuit = dt.Rows[i][4].ToString(),
                    Username = dt.Rows[i][5].ToString(),
                    BarContent = dt.Rows[i][6].ToString().Replace("\t","")
                        
                });
            }
            return true;
        }
    }
}
