// '' <summary>
// '' A Ship has all the details about itself. For example the shipname,
// '' size, number of hits taken and the location. Its able to add tiles,
// '' remove, hits taken and if its deployed and destroyed.
// '' </summary>
// '' <remarks>
// '' Deployment information is supplied to allow ships to be drawn.
// '' </remarks>
Public Class Ship {
    
    Private ShipName _shipName;
    
    Private int _sizeOfShip;
    
    Private int _hitsTaken = 0;
    
    Private List<Tile> _tiles;
    
    Private int _row;
    
    Private int _col;
    
    Private Direction _direction;
    
    // '' <summary>
    // '' The type of ship
    // '' </summary>
    // '' <value>The type of ship</value>
    // '' <returns>The type of ship</returns>
    Public String Name {
        Get {
            If ((_shipName == ShipName.AircraftCarrier)) {
                Return "Aircraft Carrier";
            }
            
            Return _shipName.ToString();
        }
    }
    
    Public int Size {
        Get {
            Return _sizeOfShip;
        }
    }
    
    Public int Hits {
        Get {
            Return _hitsTaken;
        }
    }
    
    Public int Row {
        Get {
            Return _row;
        }
    }
    
    Public int Column {
        Get {
            Return _col;
        }
    }
    
    Public Direction Direction {
        Get {
            Return _direction;
        }
    }
    
    Public Ship(ShipName ship) {
        _shipName = ship;
        _tiles = New List<Tile>();
        // gets the ship size from the enumarator
        _sizeOfShip = _shipName;
    }
    
    // '' <summary>
    // '' Add tile adds the ship tile
    // '' </summary>
    // '' <param name="tile">one of the tiles the ship is on</param>
    Public void AddTile(Tile tile) {
        _tiles.Add(tile);
    }
    
    // '' <summary>
    // '' Remove clears the tile back to a sea tile
    // '' </summary>
    Public void Remove() {
        foreach (Tile tile in _tiles) {
            tile.ClearShip();
        }
        
        _tiles.Clear();
    }
    
    Public void Hit() {
        _hitsTaken = (_hitsTaken + 1);
    }
    
    // '' <summary>
    // '' IsDeployed returns if the ships is deployed, if its deplyed it has more than
    // '' 0 tiles
    // '' </summary>
    Public bool IsDeployed {
        Get {
            Return (_tiles.Count > 0);
        }
    }
    
    Public bool IsDestroyed {
        Get {
            Return;
        }
    }
    
    internal void Deployed(Direction direction, int row, int col) {
        _row = row;
        _col = col;
        _direction = direction;
    }
}