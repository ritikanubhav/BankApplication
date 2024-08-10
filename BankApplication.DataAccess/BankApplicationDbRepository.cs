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
                throw e;
            }
            finally
            {
                connection.Close();//close connection as soon as possible
            }
        }
        public IAccount GetAccountByAccNo(string accountNo)
        {
            IAccount account=null;

            SqlConnection conn = new SqlConnection();

            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = conStr;

            string sqlSelect = $"select * from accounts where AccNo=@accNo";

            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.AddWithValue("@accNo", accountNo);
            cmd.CommandText = sqlSelect;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["accType"].ToString() == "SAVINGS")
                        account = new Saving();
                    else
                        account = new Current();

                    account.AccNo = reader[0].ToString();
                    account.Name = reader[1].ToString();
                    account.Pin= reader[2].ToString();
                    account.Active= Convert.ToBoolean(reader[3]);
                    account.DateOfOpening=Convert.ToDateTime(reader[4]);
                    account.Balance = (double)reader[5];
                    account.PrivilegeType= Enum.Parse<PrivilegeType>(reader[6].ToString());
                }
                if (account != null)
                {
                    //creating policy for the account using policyfactory instance
                    PolicyFactory policyFactory = PolicyFactory.Instance;
                    IPolicy policy = policyFactory.CreatePolicy(account.GetAccType(), account.PrivilegeType.ToString());

                    //assigning policy to account
                    account.Policy = policy;
                }
                else
                {
                    throw new AccountDoesNotExistException($"Account does not exist with account no:{accountNo}");
                }
                return account;
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
                throw e;
            }
            finally
            {
                conn.Close();//close connection as soon as possible
            }
        }

        public bool FundTransfer(string fromAccNo, string toAccNo, double amount)
        {
            //create db connection
            SqlConnection connection = new SqlConnection();

            //prepare connection string
            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            connection.ConnectionString = conStr;

            //prepare withdraw sql update and execute it
            string withdraw = $"update accounts set Balance=Balance -@withdrawalAmount where AccNo=@fromAccNo";
            
            SqlCommand cmd1 = new SqlCommand(withdraw, connection);
            cmd1.Parameters.AddWithValue("@fromAccNo",fromAccNo);
            cmd1.Parameters.AddWithValue("@withdrawalAmount", amount);

            //prepare deposit sql update and execute it
            string deposit = $"update accounts set Balance=Balance +@depositAmount where AccNo=@toAccNo";
            
            SqlCommand cmd2 = new SqlCommand(deposit, connection);
            cmd2.Parameters.AddWithValue("@toAccNo", toAccNo);
            cmd2.Parameters.AddWithValue("@depositAmount", amount);

            //Creating Transaction: grouped all commmands
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();
            cmd1.Transaction = trans;
            cmd2.Transaction = trans;

            try
            {
                cmd1.ExecuteNonQuery();//withdraw
                Console.WriteLine($"From {fromAccNo} amount {amount} debited");

                cmd2.ExecuteNonQuery();//deposit
                Console.WriteLine($"To {toAccNo} amount {amount} credited");

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                
                trans.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
    
}
