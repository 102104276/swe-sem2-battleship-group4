/* 
    Summary: The ISeaGrid defines the read only interface of a Grid. This
    allows each player to see and attack their opponents grid. 
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public interface ISeaGrid
    {

        int Width { get; }

        int Height { get; }

        // Summary: Indicates that the grid has changed.
        event EventHandler Changed;

        /*
          Summary: Provides access to the given row/column
          Parameter: row - the row to access
          Parameter: column - the column to access
          Value: what the player can see at that location
          Returns: what the player can see at that location
        */
        TileView this[int row, int col] { get; }

        /*
          Summary: Mark the indicated tile as shot.
          Parameter: row - the row of the tile
          Parameter: col - the column of the tile
          Returns: the result of the attack
        */
        AttackResult HitTile(int row, int col);
    }
}
