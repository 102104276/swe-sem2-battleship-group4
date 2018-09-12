// Summary: The battle phase is handled by the DiscoveryController.

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

namespace BattleShips
{
    static class DiscoveryController
    {

        // Summary: Handles input during the discovery phase of the game.
        // Remarks: Escape opens the game menu. Clicking the mouse will attack a location.
        public static void HandleDiscoveryInput()
        {
            if (SwinGame.KeyTyped(KeyCode.EscapeKey))
            {
                GameController.AddNewState(GameState.ViewingGameMenu);
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                DoAttack();
            }
        }

        // Summary: Attack the location that the mouse if over.
        private static void DoAttack()
        {
            Point2D mouse = default(Point2D);

            mouse = SwinGame.MousePosition();

            // Calculate the row/col clicked
            int _row = 0;
            int _col = 0;
            _row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
            _col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

            // Makes sure attack is within the grid then attacks
            if (_row >= 0 & _row < GameController.HumanPlayer.EnemyGrid.Height)
            {
                if (_col >= 0 & _col < GameController.HumanPlayer.EnemyGrid.Width)
                {
                    GameController.Attack(_row, _col);
                }
            }
        }

        // Summary: Draws the game during the attack phase.
        // Remarks: Isuru -  Updated keycodes
        public static void DrawDiscovery()
        {
            const int SCORES_LEFT = 172;
            const int SHOTS_TOP = 157;
            const int HITS_TOP = 206;
            const int SPLASH_TOP = 256;


            //sets parameter to true if the player presses key combination of Shift + C
            if ((SwinGame.KeyDown(KeyCode.LeftShiftKey) | SwinGame.KeyDown(KeyCode.RightShiftKey)) & SwinGame.KeyDown(KeyCode.CKey))
            {
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
            }
            else
            {
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
            }

            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
            UtilityFunctions.DrawMessage();

            SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
        }

    }
}
