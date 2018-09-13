
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;

/*
Summary:
The AIMediumPlayer is a type of AIPlayer where it will try and destroy a ship
if it has found a ship
*/

namespace BattleShips
{
    public class AIMediumPlayer : AIPlayer
    {
        /*
        Summary:
        Private enumarator for AI states.currently there are two states,
        the AI can be searching for a ship, or if it has found a ship it will
        target the same ship
        */
         
        private enum AIStates
        {
            Searching,
            TargetingShip
        }

        private AIStates _CurrentState = AIStates.Searching;

        private Stack<Location> _Targets = new Stack<Location>();
        public AIMediumPlayer(BattleShipsGame controller) : base(controller)
        {
        }

        /*
         Summary:
         GenerateCoordinates should generate random shooting coordinates
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
                switch (_CurrentState)
                {
                    case AIStates.Searching:
                        SearchCoords(ref row, ref column);
                        break;
                    case AIStates.TargetingShip:
                        TargetCoords(ref row, ref column);
                        break;
                    default:
                        throw new ApplicationException("AI has gone in an imvalid state");
                }
            } while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea));
            //while inside the grid and not a sea tile do the search
        }
        /*
         Summary:
         TargetCoords is used when a ship has been hit and it will try and destroy
         this ship

         Row: row generated around the hit tile
         Column: column generated around the hit tile
         */
         
        private void TargetCoords(ref int row, ref int column)
        {
            Location l = _Targets.Pop();

            if ((_Targets.Count == 0))
                _CurrentState = AIStates.Searching;
            row = l.Row;
            column = l.Column;
        }

        /*
        Summary:
        SearchCoords will randomly generate shots within the grid as long as its not hit that tile already
        
        Row: the generated row
        Column: the generated column
        */

        private void SearchCoords(ref int row, ref int column)
        {
            row = _random.Next(0, EnemyGrid.Height);
            column = _random.Next(0, EnemyGrid.Width);
        }

        /*
        Summary:
        ProcessShot will be called uppon when a ship is found.
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
                _CurrentState = AIStates.TargetingShip;
                AddTarget(row - 1, col);
                AddTarget(row, col - 1);
                AddTarget(row + 1, col);
                AddTarget(row, col + 1);
            }
            else if (result.Value == ResultOfAttack.ShotAlready)
            {
                throw new ApplicationException("Error in AI");
            }
        }

        /*
        Summary:
        AddTarget will add the targets it will shoot onto a stack

        Row: the row of the targets location
        Column: the column of the targets location
        */

        private void AddTarget(int row, int column)
        {

            if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid[row, column] == TileView.Sea)
            {
                _Targets.Push(new Location(row, column));
            }
        }
    }
}
 