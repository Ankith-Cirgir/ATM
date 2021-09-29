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

        static void TransactionHistory(int cid)
        {
            Console.Clear();

            string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();

                string Transactions = String.Format($"SELECT * from `transaction` WHERE CID_FROM='{cid}' OR CID_TO='{cid}'");
                MySqlCommand commandAmount = new MySqlCommand(Transactions, cnn);
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
                    Console.ReadLine();

                }
            }
        }
        static void Withdraw(int amount,int cid) {

            string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();

                int currentA;

                string currentAmount = String.Format($"SELECT Cbal from `customer` WHERE CID={cid}");
                MySqlCommand commandAmount = new MySqlCommand(currentAmount, cnn);
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


                    string withdraw = String.Format($"UPDATE `customer` SET Cbal={finalAmount}");
                    MySqlCommand commandWithdraw = new MySqlCommand(withdraw, cnn);
                    using (MySqlDataReader dr = commandWithdraw.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                    string withdrawTransaction = String.Format($"INSERT INTO `transaction` (`TID`, `CID_FROM`, `CID_TO`, `Camount`) VALUES(NULL, NULL, '{cid}', '{-amount}')");
                    MySqlCommand commandWithdrawTransaction = new MySqlCommand(withdrawTransaction, cnn);
                    using (MySqlDataReader dr = commandWithdrawTransaction.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }
                }
            }

            
        }

        static void Deposit(int amount,int cid)
        {
            Console.Clear();

            string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
            using (MySqlConnection cnn = new MySqlConnection(connetionString)){
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
                    string currentAmount = String.Format($"SELECT Cbal from `customer` WHERE CID={cid}");
                    MySqlCommand commandAmount = new MySqlCommand(currentAmount, cnn);
                    using (MySqlDataReader dr = commandAmount.ExecuteReader())
                    {
                        dr.Read();
                        currentA = dr.GetInt32(0);

                    }

                    int finalAmount = currentA + amount;

                    string deposit = String.Format($"UPDATE `customer` SET Cbal={finalAmount}");
                    MySqlCommand commandDeposit = new MySqlCommand(deposit, cnn);
                    using (MySqlDataReader dr = commandDeposit.ExecuteReader())
                    {
                        //Console.WriteLine(dr.Read());
                    }

                    string depositTransaction = String.Format($"INSERT INTO `transaction` (`TID`, `CID_FROM`, `CID_TO`, `Camount`) VALUES(NULL, NULL, '{cid}', '+{amount}')");
                    MySqlCommand commandTransaction = new MySqlCommand(depositTransaction, cnn);
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
            string CreateTable_customer = String.Format("CREATE TABLE `test`.`customer` ( `CID` INT NOT NULL AUTO_INCREMENT , `Cname` TEXT NOT NULL , `Cpass` TEXT NOT NULL , `Cbal` INT NOT NULL DEFAULT '0' , PRIMARY KEY (`CID`)) ENGINE = InnoDB;");
            MySqlCommand commandCustomer = new MySqlCommand(CreateTable_customer, cnn);
            using (MySqlDataReader dr = commandCustomer.ExecuteReader())
            {
                //Console.WriteLine(dr.Read());
            }
            string CreateTable_transcation = String.Format("CREATE TABLE `test`.`transaction` ( `TID` INT NOT NULL AUTO_INCREMENT , `CID_FROM` INT ,`CID_TO` INT NOT NULL , `Camount` INT NOT NULL, PRIMARY KEY (`TID`)) ENGINE = InnoDB;");
            MySqlCommand commandTranscation = new MySqlCommand(CreateTable_transcation, cnn);
            using (MySqlDataReader dr = commandTranscation.ExecuteReader())
            {
                //Console.WriteLine(dr.Read());
            }
        }

        static void Createaccount(){
            try
            {
                Console.Clear();
                string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
                using (MySqlConnection cnn = new MySqlConnection(connetionString))
                {
                    cnn.Open();
                    //Console.WriteLine("Connection Open  !");
                    Console.Write("Enter your NAME: ");
                    String name = Console.ReadLine();
                    Console.Write("Create a Password: ");
                    String pass = Console.ReadLine();
                    string insert = String.Format($"INSERT INTO `customer` (`CID`, `Cname`, `Cpass`, `Cbal`) VALUES(NULL,'{name}','{pass}','0')");
                    MySqlCommand command = new MySqlCommand(insert, cnn);
                    using (MySqlDataReader dr = command.ExecuteReader()) {
                        //Console.WriteLine(dr.Read());
                    }
                    Console.Clear();
                    Console.WriteLine("Account Created Succesfully!!\n\n\n");
                    /*
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        Console.WriteLine(dr.Read());
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open connection: Reason:" + ex.Message);
            }

        }

        static void Login(String uname, String password){
            
            string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {

                try
                {
                    cnn.Open();
                    int bal, CID;

                    string getUsername = String.Format($"SELECT Cbal,CID from `customer` WHERE Cname='{uname}' AND Cpass='{password}'");
                    MySqlCommand commandUsername = new MySqlCommand(getUsername, cnn);
                    using (MySqlDataReader dr = commandUsername.ExecuteReader())
                    {
                        dr.Read();
                        bal = dr.GetInt32(0);
                        CID = dr.GetInt32(1);
                        Console.WriteLine($"NAME: {uname}\nCID: {CID}\nCurrent account balance= {bal}");
                        Console.Write("\n\n\n1)Withdraw\n2)Transaction History\n3)Logout\nChoose an option: ");
                        int x = Convert.ToInt32(Console.ReadLine());
                        if (x == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter amount to withdraw: ");
                            int amount = Convert.ToInt32(Console.ReadLine());
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
            string strCheckTable = String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer'");
            MySqlCommand command = new MySqlCommand(strCheckTable, cnn);
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
            string connetionString = @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
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
                Console.WriteLine("Choose your Option:\n1)Create an Account\n2)Login\n3)Deposit\n4)EXIT");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Createaccount();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Enter your Details\nUsername: ");
                        String uname = Console.ReadLine();
                        Console.Write("Password: ");
                        String password = Console.ReadLine();
                        Console.Clear();
                        Login(uname, password);
                        break;
                    case 3:
                        Console.Write("Enter the CID of the user: ");
                        int cid = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter amount to be deposited: ");
                        int amount = Convert.ToInt32(Console.ReadLine());
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