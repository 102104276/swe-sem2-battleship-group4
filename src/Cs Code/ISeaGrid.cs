/* 
    Summary:
    The ISeaGrid defines the read only interface of a Grid. This
    allows each player to see and attack their opponents grid. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public interface ISeaGrid
{
    int Width { get; }

    int Height { get; }

    // Summary: Indicates that the grid has changed.

    event EventHandler Changed;

    // Summary: Provides access to the given row/column
    // Parameter: row - the row to access
    // Parameter: column - the column to access
    // Value: what the player can see at that location
    // Returns: what the player can see at that location

    TileView Item { get; }

    // Summary: Mark the indicated tile as shot.
    // Parameter: row - the row of the tile
    // Pararameter: col - the column of the tile
    // Returns: the result of the attack
    AttackResult HitTile(int row, int col);
}
