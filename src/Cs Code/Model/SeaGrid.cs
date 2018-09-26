/*
  Summary: The SeaGrid is the grid upon which the ships are deployed.
  Remarks: The grid is viewable via the ISeaGrid interface as a read only
  grid. This can be used in conjuncture with the SeaGridAdapter to
  mask the position of the ships.
*/


using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public class SeaGrid : ISeaGrid
    {

        private const int _WIDTH = 10;

        private const int _HEIGHT = 10;
        private Tile[,] _gameTiles;
        private Dictionary<ShipName, Ship> _ships;

        private int _shipsKilled = 0;

        // Summary: The sea grid has changed and should be redrawn.
        public event EventHandler Changed;

        /*
          Summary: The width of the sea grid.
          Value: The width of the sea grid.
          Returns: The width of the sea grid.
        */
        public int Width
        {
            get
            {
                return _WIDTH;
            }
        }

        /*
          Summary: The height of the sea grid.
          Value: The height of the sea grid.
          Returns: The height of the sea grid.
        */
        public int Height
        {
            get { return _HEIGHT; }
        }

        // Summary: ShipsKilled returns the number of ships killed

        public int ShipsKilled
        {
            get { return _shipsKilled; }
        }

        /*
          Summary: Show the tile view
          x: x coordinate of the tile
          y: y coordiante of the tile
        */
        public TileView this[int x, int y]
        {
            get { return _gameTiles[x, y].View; }
        }

        // Summary: AllDeployed checks if all the ships are deployed
        public bool AllDeployed
        {
            get
            {
                foreach (Ship s in _ships.Values)
                {
                    if (!s.IsDeployed)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        // Summary: SeaGrid constructor, a seagrid has a number of tiles stored in an array
        public SeaGrid(Dictionary<ShipName, Ship> ships)
        {
            _gameTiles = new Tile[Width, Height];
            //fill array with empty Tiles
            int i = 0;
            for (i = 0; i <= Width - 1; i++)
            {
                for (int j = 0; j <= Height - 1; j++)
                {
                    _gameTiles[i, j] = new Tile(i, j, null);
                }
            }

            _ships = ships;
        }

        /*
          Summary: MoveShips allows for ships to be placed on the seagrid
          Parameter: row - the row selected
          Parameter: col - the column selected
          Parameter: ship - the ship selected
          Parameter: direction - the direction the ship is going
        */
        public void MoveShip(int row, int col, ShipName ship, Direction direction)
        {
            Ship newShip = _ships[ship];
            newShip.Remove();
            AddShip(row, col, direction, newShip);
        }

        public void ClearBoard()
        {
            foreach (KeyValuePair<ShipName, Ship> ship in _ships)
            {
                ship.Value.Remove();
            }
        }

        /*
          Summary: AddShip add a ship to the SeaGrid
          Parameter: row - row coordinate
          Parameter: col - col coordinate
          Parameter: direction - direction of ship
          Parameter: newShip - the ship
        */
        private void AddShip(int row, int col, Direction direction, Ship newShip)
        {
            try
            {
                int size = newShip.Size;
                int currentRow = row;
                int currentCol = col;
                int dRow = 0;
                int dCol = 0;

                if (direction == Direction.LeftRight)
                {
                    dRow = 0;
                    dCol = 1;
                }
                else
                {
                    dRow = 1;
                    dCol = 0;
                }

                //place ship's tiles in array and into ship object
                int i = 0;
                for (i = 0; i <= size - 1; i++)
                {
                    if (currentRow < 0 || currentRow >= Width || currentCol < 0 || currentCol >= Height)
                    {
                        throw new InvalidOperationException("Ship can't fit on the board");
                    }

                    _gameTiles[currentRow, currentCol].Ship = newShip;
                    currentCol += dCol;
                    currentRow += dRow;
                }
                newShip.Deployed(direction, row, col);
            }
            catch (Exception e)
            {
                newShip.Remove();
                //if fails remove the ship
                throw new ApplicationException(e.Message);
            }
            finally
            {
                if (Changed != null)
                {
                    Changed(this, EventArgs.Empty);
                }
            }
        }

        /*
          Summary: HitTile hits a tile at a row/col, and whatever tile has been hit, a
          result will be displayed.
          Parameter: row - the row at which is being shot
          Parameter: col - the cloumn at which is being shot
          Returns: An attackresult (hit, miss, sunk, shotalready)
        */
        public AttackResult HitTile(int row, int col)
        {
            try
            {
                //tile is already hit
                if (_gameTiles[row, col].Shot)
                {
                    return new AttackResult(ResultOfAttack.ShotAlready, "have already attacked [" + col + "," + row + "]!", row, col);
                }

                _gameTiles[row, col].Shoot();

                //there is no ship on the tile
                if (_gameTiles[row, col].Ship == null)
                {
                    return new AttackResult(ResultOfAttack.Miss, "missed", row, col);
                }

                //all ship's tiles have been destroyed
                if (_gameTiles[row, col].Ship.IsDestroyed)
                {
                    _gameTiles[row, col].Shot = true;
                    _shipsKilled += 1;
                    return new AttackResult(ResultOfAttack.Destroyed, _gameTiles[row, col].Ship, "destroyed the enemy's", row, col);
                }
                
                //else hit but not destroyed
                return new AttackResult(ResultOfAttack.Hit, "hit something!", row, col);
            }
            finally
            {
                if (Changed != null)
                {
                    Changed(this, EventArgs.Empty);
                }
            }
        }
    }

}