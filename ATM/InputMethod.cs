using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class InputMethod
    {
        public static int IntInput()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public static string StringInput()
        {
            return Console.ReadLine();
        }
    }
}