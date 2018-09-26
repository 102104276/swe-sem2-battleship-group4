// Summary:  AttackResult gives the result after a shot has been made.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
namespace BattleShips
{
	public class AttackResult
	{
		private ResultOfAttack _value;
		private Ship _ship;
		private string _text;
		private int _row;
		private int _column;

		// Summary: The result of the attack
		// Returns: The result of the attack
		public ResultOfAttack Value {
			get { return _value; }
		}

		// Summary: The ship, if any, involved in this result
		// Returns: The ship, if any, involved in this result
		public Ship Ship {
			get { return _ship; }
		}
        
		// Summary: A textual description of the result.
		// Value: A textual description of the result.
		// Returns: A textual description of the result.
		public string Text {
			get { return _text; }
		}

		// Summary: The row where the attack occurred
		public int Row {
			get { return _row; }
		}


		// Summary: The column where the attack occurred
		public int Column {
			get { return _column; }
		}

        /*
		   Summary: Set the _value to the PossibleAttack value
		   Parameter: value - Either hit, miss, destroyed, shotalready
		   Parameter: text - Text to display
		   Parameter: row - Row to display
		   Parameter: column - Column to display
        */
		public AttackResult(ResultOfAttack value, string text, int row, int column)
		{
			_value = value;
			_text = text;
			_ship = null;
			_row = row;
			_column = column;
		}

        /*
		  Set the _value to the PossibleAttack value, and the _ship to the ship
		  Parameter: value - Either hit, miss, destroyed, shotalready
		  Parameter: ship - The ship information
		  Parameter: text - Text to display
		  Parameter: row - Row to display
		  Parameter: column - Column to display
        */
		public AttackResult(ResultOfAttack value, Ship ship, string text, int row, int column) : this(value, text, row, column)
		{
			_ship = ship;
		}

		// Returns: The textual information about the attack
		public override string ToString()
		{
			if (_ship == null)
			{
				return Text;
			}

			return Text + " " + _ship.Name;
		}
	}
}