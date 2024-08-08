using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using BankApplication.Common;
namespace BankApplication.DataAccess
{
    public class BankApplicationDbRepository
    {
        public void Create(IAccount account)
        {

            SqlConnection connection = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            connection.ConnectionString = conStr;


            string sqlInsert = $"insert into accounts values (@accNo,@name,@pin,@active,@dtOfOpening,@balance,@privelegeType,@accType)";

            SqlCommand cmd = new SqlCommand();
            SqlParameter p1 = new SqlParameter();

            p1.ParameterName = "@accNo";
            p1.Value = account.AccNo;
            cmd.Parameters.Add(p1);

            cmd.Parameters.AddWithValue("@name", account.Name);
            cmd.Parameters.AddWithValue("@pin", account.Pin);
            cmd.Parameters.AddWithValue("@active", account.Active);
            cmd.Parameters.AddWithValue("@dtOfOpening", account.DateOfOpening);
            cmd.Parameters.AddWithValue("@balance", account.Balance);
            cmd.Parameters.AddWithValue("@privelegeType", account.PrivilegeType.ToString());
            cmd.Parameters.AddWithValue("@accType", account.GetAccType());

            cmd.CommandText = sqlInsert;
            cmd.Connection = connection;
            try
            {
                connection.Open();//open as late as possible
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();//close connection as soon as possible
            }
        }
        //public IAccount GetAccountByAccNo(string accountNo)
        //{

        //}
        public void Update(string accountNo,double newBalance)
        {
            SqlConnection conn = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = conStr;


            string sqlUpdate = $"update accounts set Balance=@balance where AccNo=@accNo";

            SqlCommand cmd = new SqlCommand();
            SqlParameter p1 = new SqlParameter();

            p1.ParameterName = "@balance";
            p1.Value = newBalance;
            cmd.Parameters.Add(p1);
            cmd.Parameters.AddWithValue("@accNo", accountNo);
            
            cmd.CommandText = sqlUpdate;
            cmd.Connection = conn;
            try
            {
                conn.Open();//open as late as possible
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();//close connection as soon as possible
            }
        }
    }
    
}
