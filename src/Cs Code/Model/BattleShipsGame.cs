/*
  Summary: The BattleShipsGame controls a big part of the game. It will add the two players
  to the game and make sure that both players ships are all deployed before starting the game.
  It also allows players to shoot and swap turns between player. It will also check if players
  are destroyed.
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public class BattleShipsGame
    {
        /*
          Summary: The attack delegate type is used to send notifications of the end of an
          attack by a player or the AI.
          Parameter: sender - the game sending the notification
          Parameter: result - the result of the attack
        */
        public delegate void AttackCompletedHandler(object sender, AttackResult result);

        //Summary: The AttackCompleted event is raised when an attack has completed.
        //Remarks: This is used by the UI to play sound effects etc.
        public event AttackCompletedHandler AttackCompleted;

        private Player[] _players = new Player[3];

        private int _playerIndex = 0;

        /*
          Summary: The current player.
          Value: The current player
          Returns: The current player
          Remarks: This value will switch between the two players as they have their attacks
        */
        public Player Player
        {
            get { return _players[_playerIndex]; }
        }

        /*
          Summary: AddDeployedPlayer adds both players and will make sure
          that the AI player deploys all ships
          P: Player object (ADD)
        */
        public void AddDeployedPlayer(Player p)
        {
            if (_players[0] == null)
            {
                _players[0] = p;
            }
            else if (_players[1] == null)
            {
                _players[1] = p;
                CompleteDeployment();
            }
            else
            {
                throw new ApplicationException("You cannot add another player, the game already has two players.");
            }
        }

        /*
          Summary: Assigns each player the other's grid as the enemy grid. This allows each player
          to examine the details visable on the other's sea grid.
        */

        private void CompleteDeployment()
        {
            _players[0].Enemy = new SeaGridAdapter(_players[1].PlayerGrid);
            _players[1].Enemy = new SeaGridAdapter(_players[0].PlayerGrid);
        }

        /*
          Summary: Shoot will swap between players and check if a player has been killed.
          It also allows the current player to hit on the enemygrid.
          Parameter: row - the row fired upon>
          Parameter: col - the column fired upon
          Returns: The result of the attack
        */
        public AttackResult Shoot(int row, int col)
        {
            AttackResult newAttack = default(AttackResult);
            int otherPlayer = (_playerIndex + 1) % 2;

            newAttack = Player.Shoot(row, col);

            //Will exit the game when all players ships are destroyed
            if (_players[otherPlayer].IsDestroyed)
            {
                newAttack = new AttackResult(ResultOfAttack.GameOver, newAttack.Ship, newAttack.Text, row, col);
            }

            if (AttackCompleted != null)
            {
                AttackCompleted(this, newAttack);
            }

            //change player if the last hit was a miss
            if (newAttack.Value == ResultOfAttack.Miss)
            {
                _playerIndex = otherPlayer;
            }

            return newAttack;
        }
    }
}