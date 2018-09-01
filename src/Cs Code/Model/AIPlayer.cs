// Summary: The AIPlayer is a type of player. It can readomly deploy ships, it also has the functionality to generate coordinates and shoot at tiles

using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace BattleShips
{
    public abstract class AIPlayer : Player
    {
        //Summary: Location can store the location of the last hit made by an AI Player. The use of which determines the difficulty.
        protected class Location
        {
            private int _Row;
            private int _Column;

            //Summary: The row of the shot
            //Value: The row of the shot
            //Returns: The row of the shot
            public int Row
            {
                get
                {
                    return _Row;
                }
                set
                {
                    _Row = value;
                }
            }

            //Summary: The column of the shot
            //Value: The column of the shot
            //Returns: The column of the shot
            public int Column
            {
                get
                {
                    return _Column;
                }
                set
                {
                    _Column = value;
                }
            }

            // Summary: Sets the last hit made to the local variables
            //Row: the row of the location
            //Column: the column of the location
            public Location(int row, int column)
            {
                _Column = column;
                _Row = row;
            }

            //Summary: Check if two locations are equal
            //This: location 1
            //Other: location 2
            //Returns>true if location 1 and location 2 are at the same spot

            public static bool operator ==(Location @this, Location other)
            {
                return @this != null && other != null && @this.Row == other.Row && @this.Column == other.Column;
            }

            //Summary: Check if two locations are not equal
            //This: location 1
            //Other: location 2
            //Returns: true if location 1 and location 2 are not at the same spot

            public static bool operator !=(Location @this, Location other)
            {
                return @this == null || other == null || @this.Row != other.Row || @this.Column != other.Column;
            }
        }


        public AIPlayer(BattleShipsGame game) : base(game)
        {
        }

        //Summary: Generate a valid row, column to shoot at
        //Row: output the row for the next shot
        //Column: output the column for the next show

        protected abstract void GenerateCoords(ref int row, ref int column);

        //Summary: The last shot had the following result. Child classes can use this to prepare for the next shot.
        //Result: The result of the shot
        //Row: the row shot
        //Row: the row shot
        //Col: the column shot

        protected abstract void ProcessShot(int row, int col, AttackResult result);

        //Summary: The AI takes its attacks until its go is over.
        //Returns: The result of the last attack
    
    public override AttackResult Attack()
        {
            AttackResult result;
            int row = 0;
            int column = 0;

            do
            {
                Delay();
                GenerateCoords(ref row, ref column);
                result = _game.Shoot(row, column);
                ProcessShot(row, column, result);
            }
            while (result.Value != ResultOfAttack.Miss && result.Value != ResultOfAttack.GameOver && !SwinGame.WindowCloseRequested)// generate coordinates for shot// take shot
    ;

            return result;
        }

        //Summary: Wait a short period to simulate the think time

        private void Delay()
        {
            int i;
            for (i = 0; i <= 150; i++)
            {
                // Dont delay if window is closed
                if (SwinGame.WindowCloseRequested)
                    return;

                SwinGame.Delay(5);
                SwinGame.ProcessEvents();
                SwinGame.RefreshScreen();
            }
        }
    }
}