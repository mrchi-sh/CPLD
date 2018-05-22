using System;
using System.Data;
using MySql.Data.MySqlClient;
using CpldBase;
using CpldLog;

namespace CpldDB
{
    public class DbCommon
    {
        private static MySqlConnection _conn = null;

        public static void InitDbConn()
        {
            var mySqlStr = "Database=tds_data;Data Source=" + Params.DbHost + ";User Id=root;Password=asdfghjkl;"+
                              "pooling=false;CharSet=utf8;port=" + Params.DbPort + "; Connection Timeout = 5000";
            _conn = new MySqlConnection(mySqlStr);
        }

        //synchronized database operation 

        protected static bool QueryDataSync(string querySql, string lockTableName, out DataTable oDt)
        {
            var lockTable = "LOCK TABLE '" + lockTableName + "' READ; LOCK TABLE '" + lockTableName + "' WRITE;";
            const string unlockTable = ";UNLOCK TABLES";
            var querySqlLock = lockTable + querySql + unlockTable;

            return QueryData(querySqlLock, out oDt);            
        }

        protected static bool QueryData(string querySql, out DataTable dt)
        {
            var cmd = new MySqlCommand(querySql, _conn);
            dt = new DataTable();       
            try
            {
                _conn.Open();
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                return true;
            }
            catch (Exception ex)
            {
                InfoBox.ErrorMsg("数据库操作错误，请检查数据库连接或联系管理员");
                LogControl.LogError("db operation error", ex);               
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected static bool NonQueryData(string querySql)
        {
            var cmd = new MySqlCommand(querySql, _conn);

            try
            {
                _conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                InfoBox.ErrorMsg("数据库操作错误，请检查数据库连接或联系管理员");
                LogControl.LogError("db operation error", ex);
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }

        protected static bool NonQueryData(string querySql, DataTable dt)
        {
            var dataAdapter = new MySqlDataAdapter();
            try
            {
                _conn.Open();
                dataAdapter.SelectCommand = new MySqlCommand(querySql, _conn);
                var mySqlCommandBuilder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dt);
                return true;
            }
            catch (Exception ex)
            {
                InfoBox.ErrorMsg("数据库操作错误，请检查数据库连接或联系管理员");
                LogControl.LogError("db operation error", ex);
                return false;
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}
