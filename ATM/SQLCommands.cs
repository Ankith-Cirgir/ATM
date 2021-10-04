using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class SQLCommands
    {

        public static string CreateCustomerTable()
        {
            return String.Format("CREATE TABLE `test`.`customer` ( `CID` INT NOT NULL AUTO_INCREMENT , `Cname` TEXT NOT NULL , `Cpass` TEXT NOT NULL , `Cbal` INT NOT NULL DEFAULT '0' , PRIMARY KEY (`CID`)) ENGINE = InnoDB;");
        }

        public static string CreateTransactionTable()
        {
            return String.Format("CREATE TABLE `test`.`transaction` ( `TID` INT NOT NULL AUTO_INCREMENT , `CID_FROM` INT ,`CID_TO` INT NOT NULL , `Camount` INT NOT NULL, PRIMARY KEY (`TID`)) ENGINE = InnoDB;");
        }


        public static string CreateAccount(string name, string pass)
        {
            return String.Format($"INSERT INTO `customer` (`CID`, `Cname`, `Cpass`, `Cbal`) VALUES(NULL,'{name}','{pass}','0')");
        }

        public static string ConnectionString()
        {
            return @"Server=localhost;Database=test;User ID=root;Password=;SSL Mode=None";
        }

        public static string CheckTable()
        {
            return String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer'");
        }

        public static string GetTransactionDetails(int CID)
        {
            return String.Format($"SELECT * from `transaction` WHERE CID_FROM='{CID}' OR CID_TO='{CID}'");
        }

        public static string GetBal(int CID) 
        { 
            return String.Format($"SELECT Cbal from `customer` WHERE CID={CID}");
        }

        public static string SetBal(int finalAmount,int CID)
        {
            return String.Format($"UPDATE `customer` SET Cbal={finalAmount} WHERE CID={CID}");
        }

        public static string SetTransactionDetails(int CID,int amount)
        {
            return String.Format($"INSERT INTO `transaction` (`TID`, `CID_FROM`, `CID_TO`, `Camount`) VALUES(NULL, NULL, '{CID}', '+{amount}')");
        }

        public static string GetUserDetails(string uname,string password) 
        {
            return String.Format($"SELECT Cbal,CID from `customer` WHERE Cname='{uname}' AND Cpass='{password}'");
        }
    }
}
