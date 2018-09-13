
/*
Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
all ships are deployed and if all ships are detroyed. A Player can also attach.
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
namespace BattleShips
{
	public class Player : IEnumerable<Ship>
	{

		protected static Random _random = new Random();
		private Dictionary<ShipName, Ship> _ships = new Dictionary<ShipName, Ship>();
		private SeaGrid _playerGrid;
		private ISeaGrid _enemyGrid;

		protected BattleShipsGame _game;
		private int _shots;
		private int _hits;

		private int _misses;
		// Returns the game that the player is part of.
		// Value: The game
		// Returns: The game that the player is playing
		public BattleShipsGame Game
		{
			get { return _game; }
			set { _game = value; }
		}

		// Sets the grid of the enemy player
		// Value: The enemy's sea grid
		public ISeaGrid Enemy
		{
			set { _enemyGrid = value; }
		}

		// Constructor for the Player
		// Parameter: controller - The game that the player is playing in
		public Player(BattleShipsGame controller)
		{
			_game = controller;
			_playerGrid = new SeaGrid(_ships);

			// for each ship, add the ship's name so the seagrid knows about them
			foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
			{
				if (name != ShipName.None)
				{
					_ships.Add(name, new Ship(name));
				}
			}

			RandomizeDeployment();
		}

		// The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
		// Value: The grid to use.
		// Returns: The enemy's grid.
		public ISeaGrid EnemyGrid
		{
			get { return _enemyGrid; }
			set { _enemyGrid = value; }
		}

		// The PlayerGrid is just a normal SeaGrid where the players ships can be deployed and seen
		// Returns: The PlayerGrid.
		public SeaGrid PlayerGrid
		{
			get { return _playerGrid; }
		}

		// Returns: true if all ships are deployed
		public bool ReadyToDeploy
		{
			get { return _playerGrid.AllDeployed; }
		}

		// Checks if all ships on your grid are destroyed!
		// Returns: True is all ships on the player grid are destroyed.
		public bool IsDestroyed
		{
			// Check if all ships are destroyed... -1 for the none ship
			get { return _playerGrid.ShipsKilled == Enum.GetValues(typeof(ShipName)).Length - 1; }
		}

		// Returns the Player's ship with the given name.
		// Parameter: Name - the name of the ship to return
		// Returns: The ship with the indicated name
		// Note: The none ship returns nothing/null
		public Ship Ship(ShipName name)
		{

			if (name == ShipName.None)
				return null;

			return _ships[name];

		}

		/// The number of shots the player has made
		/// Value: Shots taken
		/// Returns: The number of shots taken
		public int Shots
		{
			get { return _shots; }
		}

		// Fairly self explanatory.
		public int Hits
		{
			get { return _hits; }
		}

		// Total number of shots that missed
		// Value: Miss count
		// Returns: The number of shots that have missed ships
		public int Missed
		{
			get { return _misses; }
		}

		// Returns the score of the player.
		/*
		Returns: 0 if all of the player's ships are destroyed. Returns the player's
		score otherwise, as per the calculation below.
		*/
		public int Score
		{
			get
			{
				if (IsDestroyed)
				{
					return 0;
				}
				else
				{
					return (Hits * 12) - Shots - (PlayerGrid.ShipsKilled * 20);
				}
			}
		}

		// Makes it possible to enumerate over the ships the player has.
		// Returns: A Ship enumerator
		public IEnumerator<Ship> GetShipEnumerator()
		{
			Ship[] result = new Ship[_ships.Values.Count + 1];
			_ships.Values.CopyTo(result, 0);
			List<Ship> lst = new List<Ship>();
			lst.AddRange(result);

			return lst.GetEnumerator();
		}
		IEnumerator<Ship> IEnumerable<Ship>.GetEnumerator()
		{
			return GetShipEnumerator();
		}

		// Makes it possible to enumerate over the ships the player has.
		// Returns: A Ship enumerator
		public IEnumerator GetEnumerator()
		{
			Ship[] result = new Ship[_ships.Values.Count + 1];
			_ships.Values.CopyTo(result, 0);
			List<Ship> lst = new List<Ship>();
			lst.AddRange(result);

			return lst.GetEnumerator();
		}

		// Virtual Attack allows the player to shoot
		public virtual AttackResult Attack()
		{
			// human does nothing here...
			return null;
		}

		// Shoot at a given row/column
		// Parameter: row - The row to attack
		// Parameter: col - The column to attack
		// Returns: The result of the attack
		internal AttackResult Shoot(int row, int col)
		{
			_shots += 1;
			AttackResult result = default(AttackResult);
			result = EnemyGrid.HitTile(row, col);

			switch (result.Value)
			{
				case ResultOfAttack.Destroyed:
				case ResultOfAttack.Hit:
					_hits += 1;
					break;
				case ResultOfAttack.Miss:
					_misses += 1;
					break;
			}

			return result;
		}

		// Randomises the deployment of ships on the player's grid.
		public virtual void RandomizeDeployment()
		{
			bool placementSuccessful = false;
			Direction heading = default(Direction);

			// for each ship to deploy in shipist

			foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName)))
			{
				if (shipToPlace == ShipName.None)
					continue;

				placementSuccessful = false;

				// generate random position until the ship can be placed
				do
				{
					int dir = _random.Next(2);
					int x = _random.Next(0, 11);
					int y = _random.Next(0, 11);


					if (dir == 0)
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
					catch
					{
						placementSuccessful = false;
					}
				} while (!placementSuccessful);
			}
		}
	}
}