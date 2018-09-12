/*
    Summary:
    The AIPlayer is a type of player. It can randomly deploy ships, it also has the
    functionality to generate coordinates and shoot at tiles
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

public abstract class AIPlayer : Player
{

    /* 
      Summary: Location can store the location of the last hit made by an
      AI Player. The use of which determines the difficulty.
    */
    protected class Location
	{
		private int _row;

		private int _column;

        // Summary: The row of the shot
        // Value: The row of the shot
        //Returns: The row of the shot
        public int Row {
			get { return _row; }
			set { _row = value; }
		}

        // Summary: The column of the shot
        // Value: The column of the shot
        //Returns: The column of the shot
        public int Column {
			get { return _column; }
			set { _column = value; }
		}

        // Summary: Sets the last hit made to the local variables
        // Parameter: row - the row of the location
        // Parameter: column - the column of the location
        public Location(int row, int column)
		{
			_column = column;
			_row = row;
		}

        // Summary: Check if two locations are equal
        // Parameter: this - location 1
        // Parameter: other - location 2
        // Returns: true if location 1 and location 2 are at the same spot
        public static bool operator ==(Location @this, Location other)
		{
			return !ReferenceEquals(@this, null) && !ReferenceEquals(other, null) && @this.Row == other.Row && @this.Column == other.Column;
//			return @this != null && other != null && @this.Row == other.Row && @this.Column == other.Column;
		}

        // Summary: Check if two locations are not equal
        // Parameter: this - location 1
        // Parameter: other - location 2
        // Retuns: true if location 1 and location 2 are not at the same spot
        public static bool operator !=(Location @this, Location other)
		{
			return ReferenceEquals(@this, null) || ReferenceEquals(other, null) || @this.Row != other.Row || @this.Column != other.Column;
			//return @this == null || other == null || @this.Row != other.Row || @this.Column != other.Column;
		}
	}

    // Summary: empty method
    public AIPlayer(BattleShipsGame game) : base(game)
	{
	}

    // Summary: Generate a valid row, column to shoot at
    // Paramater: row - output the row for the next shot
    // Parameter: column - output the column for the next show
    protected abstract void GenerateCoords(ref int row, ref int column);

    /*
      Summary: The last shot had the following result. Child classes can use this
      to prepare for the next shot.
    */
    // Parameter: result - The result of the shot
    // Parameter: row - the row shot
    // Parameter: col - the column shot
    protected abstract void ProcessShot(int row, int col, AttackResult result);

    // Summary: The AI takes its attacks until its go is over.
    // Returns: The result of the last attack
    public override AttackResult Attack()
	{
		AttackResult result = default(AttackResult);
		int row = 0;
		int column = 0;

		//keep hitting until a miss
		do {
			Delay();

			GenerateCoords(ref row, ref column);
			//generate coordinates for shot
			result = _game.Shoot(row, column);
			//take shot
			ProcessShot(row, column, result);
		} while (result.Value != ResultOfAttack.Miss && result.Value != ResultOfAttack.GameOver && !SwinGame.WindowCloseRequested());

		return result;
	}

    // Summary: Wait a short period to simulate the think time
    private void Delay()
	{
		int i = 0;
		for (i = 0; i <= 150; i++) {
			//Dont delay if window is closed
			if (SwinGame.WindowCloseRequested())
				return;

			SwinGame.Delay(5);
			SwinGame.ProcessEvents();
			SwinGame.RefreshScreen();
		}
	}
}