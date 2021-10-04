using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Program
    {

        static void TransactionHistory(int CID)
        {
            Console.Clear();
            using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString()))
            {
                cnn.Open();
                MySqlCommand commandAmount = new MySqlCommand(SQLCommands.GetTransactionDetails(CID), cnn);
                using (MySqlDataReader dr = commandAmount.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        Console.WriteLine("TID    CID_FROM    CID_TO    Camount");
                        while (dr.Read())
                        {
                            int cid_from = dr.IsDBNull(1) ? 0 : dr.GetInt32(dr.GetInt32(1));
                            Console.WriteLine($"{dr.GetInt32(0)}      {cid_from}           {dr.GetInt32(2)}         {dr.GetInt32(3)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    Console.WriteLine("Press any key to logout...");
                    InputMethod.StringInput();

                }
            }
        }
        static void Withdraw(int amount,int CID) {

            using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString()))
            {
                cnn.Open();

                int currentA;
                MySqlCommand commandAmount = new MySqlCommand(SQLCommands.GetBal(CID), cnn);
                using (MySqlDataReader dr = commandAmount.ExecuteReader())
                {
                    dr.Read();
                    currentA = dr.GetInt32(0);

                }

                if (currentA - amount < 0)
                {
                    Console.WriteLine("Insufficient FUNDS");
                }
                else
                {
                    int finalAmount = currentA - amount;
                    MySqlCommand commandWithdraw = new MySqlCommand(SQLCommands.SetBal(finalAmount,CID), cnn);
                    using (MySqlDataReader dr = commandWithdraw.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                    MySqlCommand commandWithdrawTransaction = new MySqlCommand(SQLCommands.SetTransactionDetails(CID,-amount), cnn);
                    using (MySqlDataReader dr = commandWithdrawTransaction.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                }
            } 
        }

        static void Deposit(int amount,int CID)
        {
            Console.Clear();

            using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString())){
                try
                {
                    cnn.Open();

                    /* CODE TO HANDLE DEPOSITING MONEY INTO UNREGISTERED CID
                    string cidexis = String.Format($"SELECT * FROM `customers` WHERE CID={cid}");
                    MySqlCommand cidex = new MySqlCommand(cidexis, cnn);
                    using (MySqlDataReader dr = cidex.ExecuteReader()){
                        if (){

                        }
                    }
                    */
                    int currentA;
                    MySqlCommand commandAmount = new MySqlCommand(SQLCommands.GetBal(CID), cnn);
                    using (MySqlDataReader dr = commandAmount.ExecuteReader())
                    {
                        dr.Read();
                        currentA = dr.GetInt32(0);

                    }

                    int finalAmount = currentA + amount;
                    MySqlCommand commandDeposit = new MySqlCommand(SQLCommands.SetBal(finalAmount,CID), cnn);
                    using (MySqlDataReader dr = commandDeposit.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                    MySqlCommand commandTransaction = new MySqlCommand(SQLCommands.SetTransactionDetails(CID,amount), cnn);
                    using (MySqlDataReader dr = commandTransaction.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                }
                catch (Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void CreateTable(MySqlConnection cnn){
            MySqlCommand commandCustomer = new MySqlCommand(SQLCommands.CreateCustomerTable(), cnn);
            using (MySqlDataReader dr = commandCustomer.ExecuteReader())
            {
                //Console.WriteLine(dr.Read());
            }
            MySqlCommand commandTranscation = new MySqlCommand(SQLCommands.CreateTransactionTable(), cnn);
            using (MySqlDataReader dr = commandTranscation.ExecuteReader())
            {
                //Console.WriteLine(dr.Read());
            }
        }

        static void Createaccount(){

            try
            {

                Console.Clear();
                using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString()))
                {
                    cnn.Open();
                    //Console.WriteLine("Connection Open  !");

                    String name = StandardMessage.CreateAccountName();
                    String pass = StandardMessage.CreateAccountPass();


                    MySqlCommand command = new MySqlCommand(SQLCommands.CreateAccount(name, pass), cnn);

                    using (MySqlDataReader dr = command.ExecuteReader()) {
                        //Console.WriteLine(dr.Read());
                    }

                    StandardMessage.CreateAccountSuccesfully();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open connection: Reason:" + ex.Message);
            }
        }

        static void Login(String uname, String password){

            using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString()))
            {
                try
                {
                    cnn.Open();
                    int bal, CID;

                    MySqlCommand commandUsername = new MySqlCommand(SQLCommands.GetUserDetails(uname,password), cnn);
                    using (MySqlDataReader dr = commandUsername.ExecuteReader())
                    {
                        dr.Read();
                        bal = dr.GetInt32(0);
                        CID = dr.GetInt32(1);

                        StandardMessage.LoginMessage(CID,uname,bal);
                        
                        int x = InputMethod.IntInput();
                        if (x == 1)
                        {
                            StandardMessage.WithdrawPage();
                            int amount = InputMethod.IntInput();
                            Withdraw(amount, CID);
                        }
                        else if (x == 2) {
                            TransactionHistory(CID);
                        }
                        Console.Clear();
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static bool Checktable(MySqlConnection cnn) {
            MySqlCommand command = new MySqlCommand(SQLCommands.CheckTable(), cnn);
            using (MySqlDataReader dr = command.ExecuteReader())
            {
                if (dr.Read())
                {
                    Console.WriteLine("EXISTS");
                    return true;
                }
                else
                {
                    Console.WriteLine("dont exist");
                    //CreateTable(cnn);
                    return false;
                }
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            using (MySqlConnection cnn = new MySqlConnection(SQLCommands.ConnectionString()))
            {
                cnn.Open();
                if (Checktable(cnn)==false)
                {
                    CreateTable(cnn);
                    Console.WriteLine("TABLES CREATED");
                }
            }
            int exit = 1;
            while (exit == 1)
            {
                StandardMessage.WelcomeMessage();
                int choice = InputMethod.IntInput();
                switch (choice)
                {
                    case 1:
                        Createaccount();
                        break;
                    case 2:
                        String uname = StandardMessage.GetUsername();
                        String password = StandardMessage.GetPassword();
                        Login(uname, password);
                        break;
                    case 3:
                        int cid = StandardMessage.GetCID();
                        int amount = StandardMessage.GetDepositAmount();
                        Deposit(amount, cid);
                        break;
                    case 4:
                        exit = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}