// Summary: The AIEasyPlayer is a type of AIPlayer will fire at random

using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;

namespace BattleShips
{
    public class AIEasyPlayer : AIPlayer
    {
        /*
          Summary: Private enumarator for AI states. currently there are two states,
          the AI can be searching for a ship, or if it has found a ship it will
          target the same ship
        */
        private enum AIStates
        {
            Searching,
            TargetingShip
        }

        private AIStates _currentState = AIStates.Searching;

        private Stack<Location> _targets = new Stack<Location>();
        public AIEasyPlayer(BattleShipsGame controller) : base(controller)
        {
        }

        /*
          Summary: GenerateCoordinates should generate random shooting coordinates
          only when it has not found a ship, or has destroyed a ship and 
          needs new shooting coordinates
          Row: the generated row
          Column: the generated column
        */
        protected override void GenerateCoords(ref int row, ref int column)
        {
            do
            {
                //check which state the AI is in and uppon that choose which coordinate generation
                //method will be used.

                switch (_currentState)
                {
                    case AIStates.Searching:
                        SearchCoords(ref row, ref column);
                        break;
                    default:
                        throw new ApplicationException("AI has gone in an invalid state");
                }
            } while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea));
            //while inside the grid and not a sea tile do the search
        }

         //Summary: SearchCoords will randomly generate shots within the grid as long as its not hit that tile already
         //Row: the generated row
         //Column: the generated column
        private void SearchCoords(ref int row, ref int column)
        {
            row = _random.Next(0, EnemyGrid.Height);
            column = _random.Next(0, EnemyGrid.Width);
        }

        /*
          Summary: ProcessShot will be called uppon when a ship is found.
          It will create a stack with targets it will try to hit. These targets
          will be around the tile that has been hit.
          Row: the row it needs to process
          Col: the column it needs to process
          Result: the result of the last shot (should be hit)
        */
        protected override void ProcessShot(int row, int col, AttackResult result)
        {
            if (result.Value == ResultOfAttack.Hit)
            {
                _currentState = AIStates.Searching;
            }
            else if (result.Value == ResultOfAttack.ShotAlready)
            {
                throw new ApplicationException("Error in AI");
            }
        }

        //Summary: AddTarget will add the targets it will shoot onto a stack
        //row: the row of the targets location
        //column: the column of the targets location
        private void AddTarget(int row, int column)
        {

            if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid[row, column] == TileView.Sea)
            {
                _targets.Push(new Location(row, column));
            }
        }
    }
}