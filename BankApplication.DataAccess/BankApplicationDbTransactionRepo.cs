using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            cmd.Parameters.AddWithValue("@accNo", trans.FromAccount);
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
        public Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>> GetAllTransactions()
        {
            Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>> TransDictionary = new Dictionary<string, Dictionary<TransactionTypes, List<Transaction>>>();
            

            SqlConnection conn = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = conStr;

            string sqlSelect = $"SELECT * FROM transactions ORDER BY AccNo, TransactionType";

            SqlCommand cmd = new SqlCommand();

            
            cmd.CommandText = sqlSelect;

            cmd.Connection = conn;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //get all values in a row of the table
                    int transId = reader.GetInt32(reader.GetOrdinal("transid"));
                    string fromAccount = reader.GetString(reader.GetOrdinal("accno"));
                    DateTime tranDate = reader.GetDateTime(reader.GetOrdinal("tansdate"));
                    double amount = reader.GetDouble(reader.GetOrdinal("amount"));
                    TransactionTypes transType = Enum.Parse<TransactionTypes>(reader.GetString(reader.GetOrdinal("transactiontype")));
                    

                    if (!TransDictionary.ContainsKey(fromAccount))
                    {
                        TransDictionary[fromAccount] = new Dictionary<TransactionTypes, List<Transaction>>();
                    }

                    if (!TransDictionary[fromAccount].ContainsKey(transType))
                    {
                        TransDictionary[fromAccount][transType] = new List<Transaction>();
                    }
                    Transaction transaction = new Transaction
                    {
                        TransID = transId,
                        FromAccount = fromAccount,
                        TranDate = tranDate,
                        Amount = amount
                    };
                    TransDictionary[fromAccount][(transType)].Add(transaction);
                }
                return TransDictionary;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
