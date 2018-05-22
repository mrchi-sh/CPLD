using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using CpldBase;

namespace CpldDB
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserLevel { get; set; }
        public string UserLevelName { get; set; }
        public string UserAuthen { get; set; }
    }

    public class DbUser:DbCommon
    {
        private static string EncryptString(string cleartext)
        {
            const string sKey = "lzhnaihe";
            var data = Encoding.UTF8.GetBytes(cleartext);
            var des = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(sKey),
                IV = Encoding.ASCII.GetBytes(sKey)
            };
            var desEncrypt = des.CreateEncryptor();
            var result = desEncrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }

        public static bool UserLogin(string username, string password, out bool isVerified,out UserInfo user)
        {
            var encPassword = EncryptString(password);
            var querySql = "SELECT user_level, user_authen FROM user_account WHERE username = '" + username + "' AND password = '" + encPassword + "'";
            DataTable dt;
            isVerified = false;
            user = new UserInfo { UserName = username,UserLevel = -1, UserAuthen = "" };

            if (!QueryData(querySql, out dt))
                return false;

            if (dt.Rows.Count == 0) return true;
            isVerified = true;
            user.UserLevel = Convert.ToInt32(dt.Rows[0][0]);
            user.UserAuthen = dt.Rows[0][1].ToString();

            return true;
        }

        private static bool IsUsernameExist(string username, out bool isExist)
        {
            var querySql = "SELECT username FROM user_account WHERE username = '" + username + "'";
            DataTable dt;

            isExist = false;
            if (!QueryData(querySql, out dt))
                return false;
            isExist = dt.Rows.Count != 0;
            return true;
        }

        public static bool AddUser(string username, string password, int userLevel, string userAuthen)
        {
            var isExist = false;
            string querySql = null;

            if (!IsUsernameExist(username, out isExist))
                return false;

            if (isExist)
            {
                InfoBox.InfoMsg("用户名已存在");
                return false;
            }

            password = EncryptString(password);
            querySql = "INSERT INTO user_account (username, password, user_level, user_authen) VALUES ('" + username + "', '" + password + "', " + userLevel + ",'"+userAuthen+"')";

            return NonQueryData(querySql);  
        }

        public static bool EditUser(string username, string password, int userLevel, string userAuthen)
        {
            password = EncryptString(password);
            var querySql = "UPDATE user_account Set password = '" + password + "', user_level = " + userLevel + ", user_authen = '"+userAuthen+"' WHERE username = '" + username + "'";

            return NonQueryData(querySql);
        }

        public static bool LoadAllUser(out List<UserInfo> userInfoList)
        {
            const string querySql = "SELECT username, user_level FROM user_account  ORDER BY Id DESC";
            DataTable dt;
            userInfoList = new List<UserInfo>();

            if (!QueryData(querySql, out dt))
                return false;

            if (dt.Rows.Count == 0)
                return false;
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string username = dt.Rows[i][0].ToString();
                    int userlevel = Convert.ToInt32(dt.Rows[i][1]);
                    string tmpUserlevel = "";

                    switch (userlevel)
                    {
                        case 0:
                            tmpUserlevel = "管理员";
                            break;
                        case 1:
                            tmpUserlevel = "普通员工";
                            break;     
                        case 2:
                            tmpUserlevel = "班长";
                            break;
                    }

                    userInfoList.Add(new UserInfo()
                    {
                        Id = i+1,
                        UserName = username,
                        UserLevel = userlevel,
                        UserLevelName = tmpUserlevel
                    });
                }
                return true;
            }
        }

        public static bool GetUserAuthen(string username, out string userAuthen)
        {
            var querySql = "SELECT user_authen FROM user_account WHERE username = '" + username + "'";
            userAuthen = "";
            DataTable dt;

            if (!QueryData(querySql, out dt))
                return false;

            userAuthen = dt.Rows[0][0].ToString();
            return true;
        }

        public static bool SetUserAuthen(string username, string userAuthen)
        {
            string querySql = "UPDATE user_account Set user_authen = '" + userAuthen + "' WHERE username = '" + username + "'";
            return NonQueryData(querySql);
        }

        public static bool DelUser(string username)
        {
            string querySql = "DELETE FROM user_account WHERE username = '"+username+"'";
            return NonQueryData(querySql);
        }
    }
}
