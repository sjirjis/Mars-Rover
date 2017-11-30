using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Mars Rover Command Center.");
            Console.WriteLine("Please enter the upper right coordinates of the plateau, separated by a space, greater than 0.\r\nExample: 5 5");

            //instantiate new plateau object
            var plateau = new plateau();

            //prompt user for plateauCoords
            string[] plateauCoords = Console.ReadLine().Trim().Split(' ');

            bool isUserInputValid = false;

            //both of the next two functions must pass. if either one fails, run through both checks again
            while (!(isUserInputValid))
            {
                if (!(plateau.validateCoordsLength(plateauCoords)))
                {
                    plateauCoords = PromptUser.throwErrGetInputArr("\r\nPlease enter only two coordinates, separated by a space");
                }
                else if (!(plateau.validateCoordsAsWholeNumbers(plateauCoords)))
                {
                    plateauCoords = PromptUser.throwErrGetInputArr("\r\nPlease enter Whole Numbers only");
                }
                else
                {
                    //we made it. both checks passed so let's break out of the while loop
                    isUserInputValid = true;
                }
            }

            //appState user to allow user to deploy new rover or exit
            string appState = "1";
            while (appState == "1")
            {
                //all clear to move forward
                Console.WriteLine("\r\nGreat! Enter the rovers position and location.\r\nA rover's position and location is represented by a combination of x and y coordinates\r\nand a letter representing one of the four cardinal compass points. \r\nExample: 1 2 N");

                //instantiate new rover object
                var rover = new Rover();

                //prompt user for roverStartCoords
                string[] roverStartCoords = Console.ReadLine().Trim().Split(' ');

                //reset for more evaluating
                isUserInputValid = false;

                while (!(isUserInputValid))
                {
                    if (!(rover.validateCoordsLength(roverStartCoords)))
                    {
                        roverStartCoords = PromptUser.throwErrGetInputArr("\r\nPlease enter two coordinates and a cardinal compass point");
                    }
                    else if (!(rover.validateCoordsAsWholeNumbers(roverStartCoords)))
                    {
                        roverStartCoords = PromptUser.throwErrGetInputArr("\r\nPlease enter Whole Numbers for the coordinates");
                    }
                    else if (!(rover.validateOrientation(roverStartCoords[2])))
                    {
                        roverStartCoords = PromptUser.throwErrGetInputArr("\r\nPlease enter a cardinal compass point: N S E or W");
                    }
                    else if (!(rover.isRoverWithinBound(plateauCoords, roverStartCoords)))
                    {
                        roverStartCoords = PromptUser.throwErrGetInputArr("\r\nInvalid rover instructions: The rover would fall off the plateau!\r\nPlease enter the rovers position and location\r\nExample: 1 2 N");
                    }
                    else if (rover.willRoverCrash(roverStartCoords))
                    {
                        roverStartCoords = PromptUser.throwErrGetInputArr("\r\nInvalid instructions: The rover would bump into a parked rover!\r\nPlease give the rover new instructions");
                    }
                    else
                    {
                        //all checks passed, move on
                        isUserInputValid = true;
                    }
                }

                Console.WriteLine("\r\nExcellent! Lastly, give the rover instructions of 'L', 'R', or 'M'.\r\n'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its current spot.\r\n'M' means move forward one grid point, and maintain the same heading\r\nExample: LMLMLMLMM");

                string roverInstructions = Console.ReadLine().Trim();

                //reset for more evaluating
                isUserInputValid = false;

                while (!(isUserInputValid))
                {
                    if (!(rover.validateInstructions(roverInstructions)))
                    {
                        roverInstructions = PromptUser.throwErrGetInputStr("\r\nPlease use only 'L', 'R', or 'M'");
                    }
                    //if the input is valid, let's see what would happen if we deployed the Rover

                    //check for crashing into a parked rover
                    else if (rover.willRoverCrash(roverStartCoords, roverInstructions))
                    {
                        roverInstructions = PromptUser.throwErrGetInputStr("\r\nInvalid instructions: The rover would bump into a parked rover!\r\nPlease give the rover new instructions");
                    }
                    //check if rover will fall off the plateau
                    else if (!(rover.isRoverWithinBound(plateauCoords, roverStartCoords, roverInstructions)))
                    {
                        roverInstructions = PromptUser.throwErrGetInputStr("\r\nInvalid rover instructions: The rover would fall off the plateau!\r\nPlease give the rover new instructions");
                    }
                    else
                    {
                        //checks passed, deploy and park the rover
                        Console.WriteLine(rover.commitRoverDeploy(roverStartCoords, roverInstructions));
                        isUserInputValid = true;
                    }
                }

                //play again?
                Console.WriteLine("\r\nWould you like to deploy another rover? Enter 1 for Yes, 0 for No.");
                appState = Console.ReadLine().Trim();

                //reset for more evaluating
                isUserInputValid = false;

                while (!(isUserInputValid))
                {
                    //if user input is invalid ask them again
                    if (appState == "0")
                    {
                        Console.WriteLine("\r\nThe Command Center thanks you! Press any key to exit");
                        Console.ReadLine();
                        isUserInputValid = true;
                    }
                    //if the input is valid and they want to exit
                    else if (appState == "1")
                    {
                        Console.WriteLine("\r\n");
                        isUserInputValid = true;
                    }
                    else
                    {
                        appState = PromptUser.throwErrGetInputStr("\r\nPlease enter 1 for Yes or 0 for No");
                    }
                }
            }
        }
    }
}
