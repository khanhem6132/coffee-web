using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Configuration;
using System.Data;

namespace CoffeeWeb.Models.Common
{
    public class StatisticalUser
    {
        public static string strConnect =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public static CoffeeWeb.Models.StatisticalViewModel StaProc()
        {
            using (var conn = new OracleConnection(strConnect))
            using (var cmd = new OracleCommand("COFFEEWEB.SP_STATISTICS", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add("o_today", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_yesterday", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_thisweek", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_lastweek", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_thismonth", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_lastmonth", OracleDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("o_alltime", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                int GetInt(string name)
                {
                    var v = cmd.Parameters[name].Value;
                    if (v == null || v == DBNull.Value) return 0;
                    if (v is OracleDecimal od) return od.IsNull ? 0 : od.ToInt32();
                    return Convert.ToInt32(v);
                }

                return new CoffeeWeb.Models.StatisticalViewModel
                {
                    Today = GetInt("o_today"),
                    Yesterday = GetInt("o_yesterday"),
                    ThisWeek = GetInt("o_thisweek"),
                    LastWeek = GetInt("o_lastweek"),
                    ThisMonth = GetInt("o_thismonth"),
                    LastMonth = GetInt("o_lastmonth"),
                    AllTime = GetInt("o_alltime")
                };
            }
        }
    }
}
