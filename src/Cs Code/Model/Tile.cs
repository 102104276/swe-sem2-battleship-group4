
/*
  Summary: Tile knows its location on the grid, if it is a ship and if it has been
  shot before
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public class Tile
    {
        //the row value of the tile
        private readonly int _rowValue;
        //the column value of the tile
        private readonly int _columnValue;
        //the ship the tile belongs to
        private Ship _ship = null;
        //the tile has been shot at
        private bool _shot = false;

        /*
          Summary: Has the tile been shot?
          Value: indicate if the tile has been shot
          Returns: true if the tile was shot
        */
        public bool Shot
        {
            get { return _shot; }
            set { _shot = value; }
        }

        /*
          Summary: The row of the tile in the grid
          Value: the row index of the tile in the grid
          Returns: the row index of the tile
        */
        public int Row
        {
            get { return _rowValue; }
        }

        /*
          Summary: The column of the tile in the grid
          Value: the column of the tile in the grid
          Returns: the column of the tile in the grid
        */
        public int Column
        {
            get { return _columnValue; }
        }
 
        // Summary: Ship allows for a tile to check if there is ship and add a ship to a tile
        public Ship Ship
        {
            get { return _ship; }
            set
            {
                if (_ship == null)
                {
                    _ship = value;
                    if (value != null)
                    {
                        _ship.AddTile(this);
                    }
                }
                else
                {
                    throw new InvalidOperationException("There is already a ship at [" + Row + ", " + Column + "]");
                }
            }
        }

        /*
          Summary: The tile constructor will know where it is on the grid, and is its a ship
          Row: the row on the grid
          Col: The col on the grid
          Ship: What ship it is
        */
        public Tile(int row, int col, Ship ship)
        {
            _rowValue = row;
            _columnValue = col;
            _ship = ship;
        }

        // Summary: Clearship will remove the ship from the tile
        public void ClearShip()
        {
            _ship = null;
        }

        // Summary: View is able to tell the grid what the tile is
        public TileView View
        {
            get
            {
                //if there is no ship in the tile

                if (_ship == null)
                {
                    //and the tile has been hit

                    if (_shot)
                    {
                        return TileView.Miss;
                    }
                    else
                    {
                        //and the tile hasn't been hit
                        return TileView.Sea;
                    }
                }
                else
                {
                    //if there is a ship and it has been hit
                    if ((_shot))
                    {
                        return TileView.Hit;
                    }
                    else
                    {
                        //if there is a ship and it hasn't been hit
                        return TileView.Ship;
                    }
                }
            }
        }

        /*
          Summary: Shoot allows a tile to be shot at, and if the tile has been hit before
          it will give an error
        */
        internal void Shoot()
        {
            if ((false == Shot))
            {
                Shot = true;
                if (_ship != null)
                {
                    _ship.Hit();
                }
            }
            else
            {
                throw new ApplicationException("You have already shot this square");
            }
        }
    }
}
