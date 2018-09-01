using System;
using System.Collections.Generic;

/*
Player has its own _playerGrid, and can see an _enemyGrid, it can also check if
all ships are deployed and if all ships are detroyed. A Player can also attach.
*/
namespace BattleShips
{
    public class Player {
        
        protected static Random _random = new Random();
        
        private Dictionary<ShipName, Ship> _ships = new Dictionary<ShipName, Ship>();
        
        private SeaGrid _playerGrid = new SeaGrid(_ships);
        
        private ISeaGrid _enemyGrid;
        
        protected BattleShipsGame _game;
        
        private int _shots;
        
        private int _hits;
        
        private int _misses;
        
        // returns the game that the player is part of.
        // Value: The game
        // returns: The game that the player is playing
        public BattleShipsGame Game
    	{
            get
    		{
                return _game;
            }
            set
    		{
                _game = value;
            }
        }
        
        public ISeaGrid Enemy
    	{
            set
    		{
                _enemyGrid = value;
            }
        }

        

        public Player(BattleShipsGame controller)
    	{
            _game = controller;
            // for each ship add the ships name so the seagrid knows about them
            foreach(ShipName name in Enum.getValues(typeof(ShipName)))
    		{
                if((name != ShipName.None))
    			{
                    _ships.Add(name, new Ship(name));
                }
                
            }
            
            this.RandomizeDeployment();
        }

        // The enemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemy's ships
        public ISeaGrid EnemyGrid
    	{
            get
    		{
                return _enemyGrid;
            }
            set
    		{
                _enemyGrid = value;
            }
        }
        
        public SeaGrid PlayerGrid
    	{
            get
    		{
                return _playerGrid;
            }
        }
        
        public bool ReadyToDeploy
    	{
            get
    		{
                return _playerGrid.AllDeployed;
            }
        }
        
        public bool IsDestroyed
    	{
            get
    		{
                // Check if all ships are destroyed... -1 for the none ship
                return ;
            }
        }
        
        public Ship Ship
    	{
            get
    		{
                if((name == ShipName.None))
    			{
                    return null;
                }
                return _ships.Item[name];
            }
        }
        
        public int Shots
    	{
            get
    		{
                return _shots;
            }
        }
        
        public int Hits
    	{
            get
    		{
                return _hits;
            }
        }
        
        public int Missed
    	{
            get
    		{
                return _misses;
            }
        }
        
        public int Score
    	{
            get
    		{
                if(IsDestroyed)
    			{
                    return 0;
                }
                else
    			{
                    return ((Hits * 12) - (Shots - (PlayerGrid.ShipsKilled * 20)));
                }
            }
        }
        
        // A copy of the method below.
        public Ienumerator<Ship> getShipenumerator()
    	{
            Ship[,] result;
            _ships.Values.CopyTo(result, 0);
            List<Ship> lst = new List<Ship>();
            lst.AddRange(result);
            return lst.getenumerator();
        }
        
        /*
    	Makes it possible to enumerate over the ships the player
        has.
    	*/
        // returns: A Ship enumerator
        public Ienumerator getenumerator()
    	{
            Ship[,] result;
            _ships.Values.CopyTo(result, 0);
            List<Ship> lst = new List<Ship>();
            lst.AddRange(result);
            return lst.getenumerator();
        }
        
        // Virtual Attack allows the player to shoot
        public virtual AttackResult Attack()
    	{
            // human does nothing here...
            return null;
        }
        
        // Shoot at a given row/column
        // Parameter 'row': the row to attack
        // Parameter 'col': the column to attack
        // returns: the result of the attack
        internal AttackResult Shoot(int row, int col)
    	{
            _shots ++;
            AttackResult result;
            result = EnemyGrid.HitTile(row, col);
            switch (result.Value)
    		{
                case ResultOfAttack.Destroyed:
                case ResultOfAttack.Hit:
                    _hits ++;
                    break;
                case ResultOfAttack.Miss:
                    _misses ++;
                    break;
            }
            return result;
        }
        
        // Places ships in random positions on the grid.
        public virtual void RandomizeDeployment()
    	{
            bool placementSuccessful;
            Direction heading;
            // for each ship to deploy in shipist
            foreach(ShipName shipToPlace in enum.getValues(typeof(ShipName)))
    		{
                if((shipToPlace == ShipName.None))
    			{
                    // TODO: Continue for... Warning!!! not translated
                }
                
                placementSuccessful = false;
                for(; !placementSuccessful; )
    			{
                    int dir = _random.Next(2);
                    int x = _random.Next(0, 11);
                    int y = _random.Next(0, 11);
                    if((dir == 0))
    				{
                        heading = Direction.UpDown;
                    }
                    else
    				{
                        heading = Direction.LeftRight;
                    }
                    
                    // try to place ship, if position unplaceable, generate new coordinates
                    try
    				{
                        PlayerGrid.MoveShip(x, y, shipToPlace, heading);
                        placementSuccessful = true;
                    }
                    catch (System.Exception placementSuccessful)
    				{
                        false;
                    }
                    
                }
                
            }
            
        }
    }
}