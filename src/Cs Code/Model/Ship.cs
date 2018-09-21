/*
   Summary: A Ship has all the details about itself. For example the shipname,
   size, number of hits taken and the location. Its able to add tiles,
   remove, hits taken and if its deployed and destroyed. 
   Remarks: Deployment information is supplied to allow ships to be drawn.
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public class Ship
    {
        private ShipName _shipName;
        private int _sizeOfShip;
        private int _hitsTaken = 0;
        private List<Tile> _tiles;
        private int _row;
        private int _col;

        private Direction _direction;
        // Summary: The type of ship
        // Value: The type of ship
        // Returns: The type of ship
        public string Name
        {
            get
            {
                if (_shipName == ShipName.AircraftCarrier)
                {
                    return "Aircraft Carrier";
                }

                return _shipName.ToString();
            }
        }

        // Summary: The number of cells that this ship occupies.
        // Value: The number of hits the ship can take
        // Returns: The number of hits the ship can take
        public int Size
        {
            get { return _sizeOfShip; }
        }

        // Summary: The number of hits that the ship has taken.
        // Value: The number of hits the ship has taken.
        // Returns: The number of hits the ship has taken
        // Remarks: When this equals Size the ship is sunk
        public int Hits
        {
            get { return _hitsTaken; }
        }

        // Summary: The row location of the ship
        // Value: The topmost location of the ship
        // Returns: the row of the ship
        public int Row
        {
            get { return _row; }
        }

        // Summary: The column location of the ship 
        // Value: The leftmost location of the ship
        // Returns: the column of the ship
        public int Column
        {
            get { return _col; }
        }

        // Summary: The direction of the ship
        // Value: The direction of the ship, UpDown or LeftRight 
        // Returns: the direction of the ship
        public Direction Direction
        {
            get { return _direction; }
        }

        // Summary: The constructor, prepares name, tiles and size
        public Ship(ShipName ship)
        {
            _shipName = ship;
            _tiles = new List<Tile>();

            //gets the ship size from the enumarator
            _sizeOfShip = (int)_shipName;
        }

        // Summary: Add tile adds the ship tile
        // Parameter: tile - one of the tiles the ship is on
        public void AddTile(Tile tile)
        {
            _tiles.Add(tile);
        }

        // Summary: Remove clears the tile back to a sea tile
        public void Remove()
        {
            foreach (Tile tile in _tiles)
            {
                tile.ClearShip();
            }
            _tiles.Clear();
        }

        public void Hit()
        {
            _hitsTaken = _hitsTaken + 1;
        }

        // Summary: IsDeployed returns if the ships is deployed, if its deplyed it has more than 0 tiles
        public bool IsDeployed
        {
            get { return _tiles.Count > 0; }
        }

        // Summary: Checks if the ship is destroyed by comparing checking if it has been hit a number of times equal to its size
        public bool IsDestroyed
        {
            get { return Hits == Size; }
        }

        // Summary: Record that the ship is now deployed.
        // Parameter: direction
        // Parameter: row
        // Parameter: col
        internal void Deployed(Direction direction, int row, int col)
        {
            _row = row;
            _col = col;
            _direction = direction;
        }
    }
}