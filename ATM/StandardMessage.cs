using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class StandardMessage
    {
        public static void WelcomeMessage() {

            Console.Clear();
            Console.WriteLine("Choose your Option:\n1)Create an Account\n2)Login\n3)Deposit\n4)EXIT");
        }

        public static void LoginMessage(int CID,string uname,int bal)
        {
            Console.Clear();
            Console.WriteLine($"NAME: {uname}\nCID: {CID}\nCurrent account balance= {bal}");
            Console.Write("\n\n\n1)Withdraw\n2)Transaction History\n3)Logout\nChoose an option: ");
        }

        public static void WithdrawPage()
        {
            Console.Clear();
            Console.WriteLine("Enter amount to withdraw: ");
        }

        public static string CreateAccountName()
        {
            Console.Clear();
            Console.Write("Enter your NAME: ");
            return InputMethod.StringInput();
        }

        public static string CreateAccountPass()
        {
            Console.Write("Create a Password: ");
            return InputMethod.StringInput();
        }

        public static void CreateAccountSuccesfully()
        {
            Console.Clear();
            Console.WriteLine("Account Created Succesfully!!\n\n\n");
        } 

        public static int GetCID()
        {
            Console.Clear();
            Console.Write("Enter the CID of the user: ");
            return InputMethod.IntInput();
        }

        public static int GetDepositAmount()
        {
            Console.Write("Enter amount to be deposited: ");
            return InputMethod.IntInput();
        }

        public static string GetUsername()
        {
            Console.Clear();
            Console.Write("Enter your Details\nUsername: ");
            return InputMethod.StringInput();
        }

        public static string GetPassword()
        {
            Console.Write("Password: ");
            return InputMethod.StringInput();
            Console.Clear();
        }
    }
}