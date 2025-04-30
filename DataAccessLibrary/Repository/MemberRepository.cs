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
        public string DbConnStr { get; } =
            ConfigurationManager.ConnectionStrings["WebDB"].ConnectionString;
        private readonly string _dbConnString;
        public MemberRepository(string dbConnString) 
        {
            _dbConnString = dbConnString;
        }

        public string Registrator(string username, string password)
        {

        }

        public string IdentifyLogin(string username, string password)
        {

        }

        public string IdentifyAccExitOrNot(string acc, string password)
        {
            var str = "Select [account], [password], [group_authority], [startdate]  " +
                "FROM [WebDB].[dbo].[LOGINAP] " +
                "Where [account]=@acc "  ;
            string result;
            try
            {
                SqlConnection conn = new SqlConnection(DbConnStr);
                conn.Open();
                string[] temp = new string[] {acc};
                List<MemberData> temp_memberdata_list = GetSqlCommand(str, conn, temp);
                if(temp_memberdata_list != null)
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

        private static List<MemberData> GetSqlCommand(string sql, SqlConnection conn, string[] temp, params SqlParameter[] cmdParms)
        {
            try
            {
                List<MemberData> memberlist = new List<MemberData>();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@username", temp[0]);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 1200;
                cmd.Connection.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    memberlist.Add(new MemberData()
                    {
                        Account = dr["account"].ToString(),
                        Pword = dr["password"].ToString(),
                        AuthorityLevel = (int)dr["group_authority"],
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
                return memberlist;
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
