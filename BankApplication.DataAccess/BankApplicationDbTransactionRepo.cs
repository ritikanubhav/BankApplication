using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApplication.Common;

namespace BankApplication.DataAccess
{
    public class BankApplicationDbTransactionRepo
    {
        public bool InsertTransaction(Transaction trans,TransactionTypes transactionType)
        {

            SqlConnection connection = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            connection.ConnectionString = conStr;


            string sqlInsert = $"insert into transactions values (@transId,@transactionType,@accNo,@date,@amount)";

            SqlCommand cmd = new SqlCommand();
            SqlParameter p1 = new SqlParameter();

            p1.ParameterName = "@transId";
            p1.Value = trans.TransID;
            cmd.Parameters.Add(p1);

            cmd.Parameters.AddWithValue("@transactionType", transactionType.ToString());
            cmd.Parameters.AddWithValue("@accNo", trans.FromAccount.AccNo);
            cmd.Parameters.AddWithValue("@date", trans.TranDate);
            cmd.Parameters.AddWithValue("@amount", trans.Amount);
            
            cmd.CommandText = sqlInsert;
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
