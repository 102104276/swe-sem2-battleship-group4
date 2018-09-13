
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
/*
 Summary:
 The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
 presented it changes the view into a sea tile instead of a ship tile.
*/
namespace BattleShips
{
    public class SeaGridAdapter : ISeaGrid
    {


        private SeaGrid _MyGrid;
        /*
         Summary
         Create the SeaGridAdapter, with the grid, and it will allow it to be changed

         Parameter: grid: the grid that needs to be adapted
         */

        public SeaGridAdapter(SeaGrid grid)
        {
            _MyGrid = grid;
            _MyGrid.Changed += new EventHandler(MyGrid_Changed);
        }

        /*
         Summary
         MyGrid_Changed causes the grid to be redrawn by raising a changed event

         Parameter: sender - the object that caused the change
         Parameter: e - what needs to be redrawn
         */

        private void MyGrid_Changed(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        #region "ISeaGrid Members"

        /*
         Summary:
         Changes the discovery grid. Where there is a ship we will sea water

         Parameter: x - tile x coordinate
         Parameter: y - tile y coordinate
         Returns : a tile, either what it actually is, or if it was a ship then return a sea tile</returns>
         */

        public TileView this[int x, int y]
        {
            get
            {
                TileView result = _MyGrid[x, y];

                if (result == TileView.Ship)
                {
                    return TileView.Sea;
                }
                else
                {
                    return result;
                }
            }
        }

        //Summary:
        //Indicates that the grid has been changed

        public event EventHandler Changed;

        //Summary:
        //Get the width of a tile
        public int Width
        {
            get { return _MyGrid.Width; }
        }

        //Summary:
        //Get the height of the tile
        public int Height
        {
            get { return _MyGrid.Height; }
        }

        /*
        Summary:
        HitTile calls oppon _MyGrid to hit a tile at the row, col
        row:
        the row its hitting at

        col:
        the column its hitting at

        Returns:
        the result from hitting that tile
        */

        public AttackResult HitTile(int row, int col)
        {
            return _MyGrid.HitTile(row, col);
        }
        #endregion

    }
}