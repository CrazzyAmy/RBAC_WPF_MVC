using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DataModel;


namespace DataAccessLibrary.Repository
{
    public class MemberRepository
    {
        public string DbConnStr { get; } = ConfigurationManager.ConnectionStrings["WebDB"].ConnectionString;
        //public string DbConnStr { get; } = "Server = localhost; Initial Catalog = WebDB; Persist Security Info=False;User ID = sa ; Password = 1234; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout = 30;";
        //public string DbConnStr { get; } = "Server = localhost; Initial Catalog = WebDB; Persist Security Info=False;User ID = "+ Environment.GetEnvironmentVariable("SQL_AC") + " ; Password = "+Environment.GetEnvironmentVariable("SQL_PW")+"; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout = 30;";
        //ConfigurationManager.ConnectionStrings["WebDB"].ConnectionString;
        public readonly string _dbConnString;
        public MemberRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public void Registrator(string acc, string pw)
        {
            var str = @"insert into [WebDB].[dbo].[LOGINAP]([account], [password], [group_authority], [startdate]) " +
                    "values(@acc, @pw, 1, " + "@sdate" + ") ";
            string result;
            try
            {
                var enc = SHA256Encrypt(pw);
                string[] temp = new string[] { acc, enc };
                GetSqlCommand_NoQuery(str, temp);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string IdentifyLogin(string acc, string pw)
        {
            var str = @"Select [account], [password], [group_authority], [startdate]  " +
                "FROM [WebDB].[dbo].[LOGINAP] " +
                "Where [account]=@acc AND [password] = @pw ";
            string result;
            try
            {
                string[] temp = new string[] { acc, pw };
                List<MemberData> temp_memberdata_list = GetSqlCommand_twoparas(str, temp);
                if (temp_memberdata_list != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failed";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string IdentifyAccExitOrNot(string acc, string password)
        {
            var str = @"Select [account], [password], [group_authority], [startdate]  " +
                "FROM [WebDB].[dbo].[LOGINAP] " +
                "Where [account]=@acc ";
            string result;
            try
            {
                string[] temp = new string[] { acc };
                List<MemberData> temp_memberdata_list = GetSqlCommand_onepara(str, temp);
                if (temp_memberdata_list.Count > 0)
                {
                    result = "Had";
                }
                else
                {
                    result = "Haven't";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public List<MemberData> GetSqlCommand_onepara(string sql, string[] temp, params SqlParameter[] cmdParms)
        {
            try
            {
                List<MemberData> memberlist = new List<MemberData>();
                //SqlCommand cmd = new SqlCommand();
                using (SqlConnection dbConn = new SqlConnection(_dbConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@acc", temp[0]);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 1200;
                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            memberlist.Add(new MemberData()
                            {
                                Account = dr["account"].ToString(),
                                Pword = dr["password"].ToString(),
                                AuthorityLevel = int.Parse(dr["group_authority"].ToString()),
                                StartDate = Convert.ToDateTime((dr["startdate"].ToString()))
                            });
                        }
                        dr.Close();

                        if (cmdParms != null)
                        {
                            foreach (SqlParameter parm in cmdParms)
                            {
                                if ((parm.Direction) == System.Data.ParameterDirection.InputOutput ||
                                    parm.Direction == System.Data.ParameterDirection.Input && (parm.Value == null))
                                {
                                    parm.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parm);
                            }
                        }
                    }
                }
                return memberlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MemberData> GetSqlCommand_twoparas(string sql, string[] temp, params SqlParameter[] cmdParms)
        {
            try
            {
                List<MemberData> memberlist = new List<MemberData>();
                //SqlCommand cmd = new SqlCommand();
                using (SqlConnection dbConn = new SqlConnection(_dbConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@acc", temp[0]);
                        cmd.Parameters.AddWithValue("@pw", temp[1]);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 1200;
                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            memberlist.Add(new MemberData()
                            {
                                Account = dr["account"].ToString(),
                                Pword = dr["password"].ToString(),
                                AuthorityLevel = int.Parse(dr["group_authority"].ToString()),
                                StartDate = Convert.ToDateTime((dr["startdate"].ToString()))
                            });
                        }
                        dr.Close();

                        if (cmdParms != null)
                        {
                            foreach (SqlParameter parm in cmdParms)
                            {
                                if ((parm.Direction) == System.Data.ParameterDirection.InputOutput ||
                                    parm.Direction == System.Data.ParameterDirection.Input && (parm.Value == null))
                                {
                                    parm.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parm);
                            }
                        }
                    }
                }
                return memberlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MemberData> GetSqlCommand_threeparas(string sql, SqlConnection conn, string[] temp, params SqlParameter[] cmdParms)
        {
            try
            {
                List<MemberData> memberlist = new List<MemberData>();
                //SqlCommand cmd = new SqlCommand();
                using (var dbConn = new SqlConnection(_dbConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@acc", temp[0]);
                        cmd.Parameters.AddWithValue("@pw", temp[1]);
                        cmd.Parameters.AddWithValue("@sdate", DateTime.Today);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 1200;
                        cmd.Connection.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            memberlist.Add(new MemberData()
                            {
                                Account = dr["account"].ToString(),
                                Pword = dr["password"].ToString(),
                                AuthorityLevel = int.Parse(dr["group_authority"].ToString()),
                                StartDate = Convert.ToDateTime((dr["startdate"].ToString()))
                            });
                        }
                        dr.Close();

                        if (cmdParms != null)
                        {
                            foreach (SqlParameter parm in cmdParms)
                            {
                                if ((parm.Direction) == System.Data.ParameterDirection.InputOutput ||
                                    parm.Direction == System.Data.ParameterDirection.Input && (parm.Value == null))
                                {
                                    parm.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parm);
                            }
                        }
                    }
                }
                return memberlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetSqlCommand_NoQuery(string sql, string[] temp, params SqlParameter[] cmdParms)
        {
            try
            {
                List<MemberData> memberlist = new List<MemberData>();
                using (var dbConn = new SqlConnection(_dbConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = dbConn;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@acc", temp[0]);
                        cmd.Parameters.AddWithValue("@pw", temp[1]);
                        cmd.Parameters.AddWithValue("@sdate", (DateTime.Today).ToString("yyyy/MM/dd"));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 1200;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (cmdParms != null)
                        {
                            foreach (SqlParameter parm in cmdParms)
                            {
                                if ((parm.Direction) == System.Data.ParameterDirection.InputOutput ||
                                    parm.Direction == System.Data.ParameterDirection.Input && (parm.Value == null))
                                {
                                    parm.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parm);
                            }
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        // <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="strIN">要加密的string字符串</param>
        /// <returns>SHA256加密之后的密文</returns>
        public static string SHA256Encrypt(string strIN)
        {
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));

            StringBuilder rst = new StringBuilder();
            for (int i = 0; i < tmpByte.Length; i++)
            {
                rst.Append(tmpByte[i].ToString("x2"));
            }
            sha256.Clear();
            return rst.ToString();
        }

        /// <summary>
        /// 获取要加密的string字符串字节数组
        /// </summary>
        /// <param name="strKey">待加密字符串</param>
        /// <returns>加密数组</returns>
        private static byte[] GetKeyByteArray(string strKey)
        {
            UTF8Encoding Asc = new UTF8Encoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = Asc.GetBytes(strKey);
            return tmpByte;
        }
    }
}
