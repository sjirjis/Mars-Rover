using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover
{
    public class plateau
    {
        int maxX;
        int maxY;

        //plateau constructor
        public plateau(int x = 0, int y = 0)
        {
            maxX = x;
            maxY = y;
        }

        //user input validation
        public static bool validateCoordsLength(string[] arr)
        {
            var valCoord = (arr.Length == 2) ? true : false;
            return valCoord;
        }

        //user input validation
        public bool validateCoordsAsWholeNumbers(string[] arr)
        {
            bool allWhole = false;

            //try/catch because a failed string[] --> int[] will throw an unhandled exception
            try
            {
                //turn string array into int array so we can evaluate as int
                int[] myCoords = arr.Select(int.Parse).ToArray();

                //check that all elements of the array are whole numbers
                allWhole = myCoords.All(x => x > 0);

            }
            catch
            {
                return false;
            }

            return allWhole;

        }
    }

}
