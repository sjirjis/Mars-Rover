using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover
{
    class Rover
    {
        int roverX;
        int roverY;
        string roverOrientation;

        //const for new rover
        public Rover(int x = 0, int y = 0, string orientation = "N")
        {
            roverX = x;
            roverY = y;
            roverOrientation = orientation.ToUpper();
        }

        //user input validation
        public bool validateCoordsLength(string[] arr)
        {
            var valCoord = (arr.Length == 3) ? true : false;
            return valCoord;
        }

        //user input validation
        public bool validateCoordsAsWholeNumbers(string[] arr)
        {
            //allWhole to loop over input to validate both input coords are whole numbers
            bool allWhole = false;
            try
            {
                //only looking at first two entries for whole numbers as third will be char
                for (int i = 0; i <= 1; i++)
                {
                    allWhole = int.Parse(arr[i]) >= 0;
                }
            }
            catch
            {
                return false;
            }

            return allWhole;

        }

        //user input validation
        public bool validateOrientation(string orientation)
        {
            //standardize the users input for eval
            orientation = orientation.ToUpper();

            while (orientation == "N" || orientation == "S" || orientation == "E" || orientation == "W")
            {
                return true;
            }

            return false;
        }

        //user input validation
        public bool validateInstructions(string instr)
        {
            //standardize the users input for later eval
            instr = instr.ToUpper();

            //convert to array so we can use .All to eval entire string 
            char[] instrArr = instr.ToCharArray();

            if (instr.Length == 0)
            {
                return false;
            }

            //every element in the string must be L or R or M and nothing else
            bool validInstr = instrArr.All(element => element == 'L' || element == 'R' || element == 'M') ? true : false;
            return validInstr;
        }

        //check to see if rover will fall off plateau
        public bool isRoverWithinBound(string[] plateauCoords, string[] roverStartCoords, string roverInstructions = "")
        {
            //get and set initial values individually
            int plateauX = int.Parse(plateauCoords[0]);
            int plateauY = int.Parse(plateauCoords[1]);

            int roverX;
            int roverY;

            //this method is used to check both the roverStartCoords and finalCoords
            //if roverInstructions have been passed, then we check against that
            //otherwise check against roverStartCoords
            if (roverInstructions.Length > 0)
            {
                string[] roverCoords = evaluateInstructionOutcome(roverStartCoords, roverInstructions);

                roverX = int.Parse(roverCoords[0]);
                roverY = int.Parse(roverCoords[1]);
            }
            else
            {
                roverX = int.Parse(roverStartCoords[0]);
                roverY = int.Parse(roverStartCoords[1]);
            }

            //check that rover's coords are larger than 0 and are smaller or equal to plateau size
            return (
                roverX <= plateauX &&
                roverX > 0 &&
                roverY <= plateauY &&
                roverY > 0)
                ? true : false;
        }

        //object to store coords of parked rovers
        private class ParkedRover
        {
            public int XCoordinate { get; set; }
            public int YCoordinate { get; set; }
        }

        //list to store parked rover objects
        private static List<ParkedRover> parkedRovers = new List<ParkedRover>();

        //check if rover insstructions are proceed that it will/won't crash into a parked rover
        public bool willRoverCrash(string[] roverStartCoords, string roverInstructions = "")
        {
            string[] roverCoords = evaluateInstructionOutcome(roverStartCoords, roverInstructions);

            roverX = int.Parse(roverCoords[0]);
            roverY = int.Parse(roverCoords[1]);

            //loop through all objects in list. if any one returns true, meaning crash, break out and return true. else it's safe to park the rover
            foreach (ParkedRover parkedRover in parkedRovers)
            {
                if (parkedRover.XCoordinate == roverX && parkedRover.YCoordinate == roverY) return true;
            }
            return false;
        }

        //this will go through the instructions but not commit them (meaning not deploy the rover)  
        //in order to evaluate what would happen if it was deployed
        private string[] evaluateInstructionOutcome(string[] roverStartCoords, string roverInstructions)
        {
            int roverX = int.Parse(roverStartCoords[0]);
            int roverY = int.Parse(roverStartCoords[1]);

            string roverOrientation = roverStartCoords[2].ToUpper();
            int orientationInt = 0;

            //converting the orienation to an int for easier mathmatical processing to determine orientation after receiving instructions
            //this method reduces the amount of conditionals needed to check for
            //example below
            switch (roverOrientation)
            {
                case "N": orientationInt = 1; break;
                case "W": orientationInt = 2; break;
                case "S": orientationInt = 3; break;
                case "E": orientationInt = 4; break;
            }

            roverInstructions = roverInstructions.ToUpper();
            char[] roverInstructionsArr = roverInstructions.ToCharArray();

            string[] finalCoords;

            //process each instruction individually
            for (int i = 0; i < roverInstructionsArr.Length; i++)
            {
                switch (roverInstructionsArr[i])
                {
                    case 'L':
                        //if orientation is East (4) and instruction is Left, we reset to North (1)
                        if (orientationInt == 4)
                        {
                            orientationInt = orientationInt - 3;
                        }
                        //otherwise add 1
                        //example: having orientation be an int, we don't need to have an "else if" for North, West or South, for each of the three instruction options
                        //we can take it from wherever it is and add 1
                        else
                        {
                            orientationInt++;
                        }
                        break;

                    case 'R':
                        //if the orientation is North (4) and the instruction is to turn Right, we reset to East (4)
                        if (orientationInt == 1)
                        {
                            orientationInt = orientationInt + 3;
                        }
                        //otherwise minus 1
                        else
                        {
                            orientationInt--;
                        }
                        break;

                    case 'M':
                        //the orientation of the rover will dictate the value of the coordinates
                        switch (orientationInt)
                        {
                            case 1: roverY++; break; //If you have moved and your orientation is North... y + 1
                            case 2: roverX--; break; //West(2)  = x - 1
                            case 3: roverY--; break; //South(3) = y - 1    
                            case 4: roverX++; break; //East(4)  = x + 1                
                        }
                        break;
                }

                //loop through all objects in list to see if a rover will hit a parked rover during this instruction.
                //f any one returns true, meaning crash, break out and return true. else it's safe to move the rover
                foreach (ParkedRover parkedRover in parkedRovers)
                {
                    if (parkedRover.XCoordinate == roverX && parkedRover.YCoordinate == roverY)
                    {
                        finalCoords = new string[] { roverX.ToString(), roverY.ToString(), roverOrientation };
                        return finalCoords;
                    }
                }
            }

            //find the final orientation in String format
            switch (orientationInt)
            {
                case 1: roverOrientation = "N"; break;
                case 2: roverOrientation = "W"; break;
                case 3: roverOrientation = "S"; break;
                case 4: roverOrientation = "E"; break;
            }

            finalCoords = new string[] { roverX.ToString(), roverY.ToString(), roverOrientation };
            return finalCoords;
        }

        //all clear, the rover made it without falling or crashing so let's move and park it
        public string commitRoverDeploy(string[] roverStartCoords, string roverInstructions)
        {
            string[] roverCoords = (evaluateInstructionOutcome(roverStartCoords, roverInstructions));

            int roverX = int.Parse(roverCoords[0]);
            int roverY = int.Parse(roverCoords[1]);
            string roverOrientation = roverCoords[2];

            //let's store the coords so the next rover can check against this array and return the outcome
            ParkedRover newParkedRover = new ParkedRover();
            newParkedRover.XCoordinate = roverX;
            newParkedRover.YCoordinate = roverY;
            parkedRovers.Add(newParkedRover);

            string finalCoords = "\r\nFinal coordinates: " + roverX + ' ' + roverY + ' ' + roverOrientation;
            return finalCoords;
        }
    }
}
