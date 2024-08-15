using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApplication.Common;

namespace BankApplication.DataAccess
{
    public class BankApplicationExternalBankDbRepo
    {
        public bool InsertIntoExternalBank(string accNo,double amount,string bank)
        {

            SqlConnection connection = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            connection.ConnectionString = conStr;


            string sqlInsert = $"insert into {bank} values (@accNo,@amount)";

            SqlCommand cmd = new SqlCommand();
            SqlParameter p1 = new SqlParameter();

            p1.ParameterName = "@accNo";
            p1.Value =accNo;
            cmd.Parameters.Add(p1);

            cmd.Parameters.AddWithValue("@amount", amount);

            cmd.CommandText = sqlInsert;
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection;
            try
            {
                connection.Open();//open as late as possible
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();//close connection as soon as possible
            }
        }
    }
}
