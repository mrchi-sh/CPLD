using System;
using System.Collections.Generic;
using System.Data;

namespace CpldDB
{
    public enum BarFieldType
    {
        AutoIncrement,                                   //int value auto increase, step 1, 
        Constant,                                         //constant value        
        Month,
        Day,
        Year
    }

    public class BarInfo
    {
        public int Id{ get; set; }
        public string ItemName { get; set; }
        public string BarFieldName { get; set; }
        public string BarFieldType { get; set; }
        public string BarFieldValue { get; set; }
        public string IsEditable { get; set; }
    }

    public partial class DbBar:DbCommon
    {
        public static bool LoadBarInfo(string itemName, out DataTable dt)
        {
            var querySql = "SELECT id, bar_field_name, bar_field_type, bar_field_value, is_editable FROM bar_info WHERE " +
                              "item_name = '" + itemName + "' ORDER BY is_editable DESC";
            dt = null;

            return QueryData(querySql, out dt);
        }

        public static bool LoadBarInfo(string itemName, out List<BarInfo> barInfoList)
        {
            DataTable dt;
            barInfoList = new List<BarInfo>();

            if (!LoadBarInfo(itemName, out dt))
                return false;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var barFieldType = "";
                var isEditable = "";

                switch ((BarFieldType)dt.Rows[i]["bar_field_type"])
                {
                    case BarFieldType.AutoIncrement:
                        barFieldType = "自增数值";
                        break;
                    case BarFieldType.Constant:
                        barFieldType = "常量";
                        break;
                    case BarFieldType.Year:
                    case BarFieldType.Month:
                    case BarFieldType.Day:
                        barFieldType = "日期";
                        break;
                }

                isEditable = Convert.ToBoolean(dt.Rows[i]["is_editable"]) ? "是" : "否";

                barInfoList.Add(new BarInfo() 
                { 
                    Id = Convert.ToInt32(dt.Rows[i]["id"]),
                    ItemName = itemName,
                    BarFieldName = dt.Rows[i]["bar_field_name"].ToString(),
                    BarFieldType = barFieldType,
                    BarFieldValue = dt.Rows[i]["bar_field_value"].ToString(),
                    IsEditable = isEditable
                });
            }
            return true;
        }

        public static bool AddBarInfo(string itemName, string fieldName, BarFieldType fieldType, string fieldValue, bool isEditable)
        {
            var querySql = "INSERT INTO bar_info (item_name, bar_field_name, bar_field_type, bar_field_value, is_editable) VALUES " +
                              "('" + itemName + "','" + fieldName + "', '" + Convert.ToInt32(fieldType) + "', '" + fieldValue + "', " + isEditable + ")";
            return NonQueryData(querySql);
        }

        public static bool UpdateBarInfo(int id, string itemName,  string fieldName, BarFieldType fieldType, string fieldValue, bool isEditable)
        {
            var querySql = "UPDATE bar_info SET item_name = '" + itemName + "', bar_field_name = '" + fieldName + "'," +
                              "bar_field_type = '" + Convert.ToInt32(fieldType) + "', bar_field_value = '" + fieldValue + "', is_editable = " + isEditable + " WHERE id = " + id + "";
            return NonQueryData(querySql);               
        }



        public static bool DelBarInfo(string itemName, string fieldName)
        {
            var querySql = "DELETE FROM bar_info WHERE item_name = '" + itemName + "' AND bar_field_name = '" + fieldName + "'";
            return NonQueryData(querySql);
        }

        public static bool UpdateBarInfo(DataTable dt)
        {
            var querySql = "SELECT id, bar_field_name, bar_field_type, bar_field_value, is_editable FROM bar_info";
            return NonQueryData(querySql, dt);
        }
    }
}
