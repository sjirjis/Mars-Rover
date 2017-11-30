using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover
{
    class PromptUser
    {
        public static string[] throwErrGetInputArr(string promptMessage)
        {
            Console.WriteLine(promptMessage);
            return Console.ReadLine().Trim().Split(' ');
        }

        public static string throwErrGetInputStr(string promptMessage)
        {
            Console.WriteLine(promptMessage);
            return Console.ReadLine().Trim();
        }
    }
}
