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
        public void Update(IAccount account)
        {
            string dbProvider = ConfigurationManager.ConnectionStrings["default"].ProviderName;

            DbProviderFactories.RegisterFactory(dbProvider, SqlClientFactory.Instance);
            DbProviderFactory factory = DbProviderFactories.GetFactory(dbProvider);

            IDbConnection conn = factory.CreateConnection();
            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = conStr;

            string sqlUpdate = $"update contacts set ";

            IDbCommand cmd = conn.CreateCommand();

            IDbDataParameter p1 = cmd.CreateParameter();
            p1.ParameterName = "@name";
            p1.Value = account.Name;
            cmd.Parameters.Add(p1);

            IDbDataParameter p2 = cmd.CreateParameter();
            p1.ParameterName = "@mobile";
            p1.Value = account.Mobile;
            cmd.Parameters.Add(p2);

            IDbDataParameter p3 = cmd.CreateParameter();
            p1.ParameterName = "@email";
            p1.Value = c.Email;
            cmd.Parameters.Add(p3);

            IDbDataParameter p4 = cmd.CreateParameter();
            p1.ParameterName = "@location";
            p1.Value = c.Location;
            cmd.Parameters.Add(p4);

            IDbDataParameter p5 = cmd.CreateParameter();
            p1.ParameterName = "@location";
            p1.Value = c.Id;
            cmd.Parameters.Add(p5);

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
