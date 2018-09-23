// Summary: The DeploymentController controls the players actions during the deployment phase.

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

namespace BattleShips
{
    static class DeploymentController
    {
        private const int SHIPS_TOP = 98;
        private const int SHIPS_LEFT = 20;
        private const int SHIPS_HEIGHT = 90;
        private const int SHIPS_WIDTH = 300;
        private const int TOP_BUTTONS_TOP = 72;

        private static bool help_screen = false;

        private const int TOP_BUTTONS_HEIGHT = 46;
        private const int PLAY_BUTTON_LEFT = 693;

        private const int PLAY_BUTTON_WIDTH = 80;
        private const int UP_DOWN_BUTTON_LEFT = 410;

        private const int LEFT_RIGHT_BUTTON_LEFT = 350;
        private const int RANDOM_BUTTON_LEFT = 547;

        private const int HELP_BUTTON_LEFT = 485;

        private const int RANDOM_BUTTON_WIDTH = 51;

        private const int DIR_BUTTONS_WIDTH = 47;

        private const int TEXT_OFFSET = 5;
        private static Direction _currentDirection = Direction.UpDown;

        private static ShipName _selectedShip = ShipName.Tug;


        private const int CLEAR_BUTTON_HEIGHT = 46;
        private const int CLEAR_BUTTON_WIDTH = 56;
        private const int CLEAR_BUTTON_LEFT = 620;
        
        // Summary: Handles user input for the Deployment phase of the game.
        /* 
          Remarks: Involves selecting the ships, deloying ships, changing the direction
          of the ships to add, randomising deployment, and then ending deployment
          Isuru: Updated Keycodes
        */
        public static void HandleDeploymentInput()
        {
            //Shows menu when esc key pressed
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                GameController.AddNewState(GameState.ViewingGameMenu);
            }

            //Moves ships direction to vertical when down or up key pressed

            if (SwinGame.KeyTyped(KeyCode.vk_UP) || SwinGame.KeyTyped(KeyCode.vk_DOWN))
            {
                _currentDirection = Direction.UpDown;
            }

            //Moves ships direction to horizontal when left or right key pressed

            if (SwinGame.KeyTyped(KeyCode.vk_LEFT) || SwinGame.KeyTyped(KeyCode.vk_RIGHT))
            {
                _currentDirection = Direction.LeftRight;
            }

            //Randomises ship deploymeny when R key pressed
            if (SwinGame.KeyTyped(KeyCode.vk_r))
            {
                GameController.HumanPlayer.RandomizeDeployment();
            }

            //Selects ship if ship placed at cursor location, deploys ship otherwise.
            //Also checks for button presses onscreen.
            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                ShipName selected = default(ShipName);
                selected = GetShipMouseIsOver();
                if (selected != ShipName.None)
                {
                    _selectedShip = selected;
                }
                else
                {
                    DoDeployClick();
                }

                if (help_screen == false)
                {
                    if (GameController.HumanPlayer.ReadyToDeploy & UtilityFunctions.IsMouseInRectangle(PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP, PLAY_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))
                    {
                        GameController.EndDeployment();
                    }
                    else if (UtilityFunctions.IsMouseInRectangle(UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                    {
                        _currentDirection = Direction.LeftRight;
                    }
                    else if (UtilityFunctions.IsMouseInRectangle(LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                    {
                        _currentDirection = Direction.LeftRight;
                    }
                    else if (UtilityFunctions.IsMouseInRectangle(RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP, RANDOM_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))
                    {
                        GameController.HumanPlayer.RandomizeDeployment();
                    }
                    else if (UtilityFunctions.IsMouseInRectangle(CLEAR_BUTTON_LEFT, TOP_BUTTONS_TOP, CLEAR_BUTTON_WIDTH, CLEAR_BUTTON_HEIGHT))
                    {
                        //clears board
                        GameController.HumanPlayer.PlayerGrid.ClearBoard();
                    }
                    else if (UtilityFunctions.IsMouseInRectangle(HELP_BUTTON_LEFT, TOP_BUTTONS_TOP, CLEAR_BUTTON_WIDTH, CLEAR_BUTTON_HEIGHT))
                    {
                        if (help_screen)
                        {
                            help_screen = false;
                        }
                        else
                        {
                            help_screen = true;
                        }
                    }
                }
                else
                {
                    help_screen = false;
                }
            }
        }

        /*
          Summary:
          The user has clicked somewhere on the screen, check if its is a deployment and deploy
          the current ship if that is the case.

          Remarks:
          If the click is in the grid it deploys to the selected location
          with the indicated direction
        */
        private static void DoDeployClick()
        {
            Point2D mouse = default(Point2D);

            mouse = SwinGame.MousePosition();

            //Calculate the row/col clicked
            int _row = 0;
            int _col = 0;
            _row = Convert.ToInt32(Math.Floor((mouse.Y) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
            _col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

            if (_row >= 0 & _row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (_col >= 0 & _col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(_row, _col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        UtilityFunctions.PlaySFX("Error");
                        UtilityFunctions.Message = ex.Message;
                    }
                }
            }
        }

        // Summary: Draws the deployment screen showing the field and the ships that the player can deploy.
        public static void DrawDeployment()
        {

            UtilityFunctions.DrawField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer, true);

            //Draw the Left/Right and Up/Down buttons
            if (_currentDirection == Direction.LeftRight)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("LeftRightButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.DrawText("U/D", Color.Gray, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.White, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }
            else
            {
                SwinGame.DrawBitmap(GameResources.GameImage("UpDownButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.DrawText("U/D", Color.White, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.Gray, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }

            //displays current game difficulty
            SwinGame.DrawText("Difficulty: " + GameController.AIDifficulty, Color.White, 530, 570);

            //clear button
            SwinGame.DrawBitmap(GameResources.GameImage("Clear"), 620, TOP_BUTTONS_TOP);


            //draw help button
            SwinGame.DrawBitmap(GameResources.GameImage("Help"), HELP_BUTTON_LEFT, TOP_BUTTONS_TOP);
            

            //DrawShips
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i = 0;
                i = ((int)sn) - 1;
                if (i >= 0)
                {
                    if (sn == _selectedShip)
                    {
                        SwinGame.DrawBitmap(GameResources.GameImage("SelectedShip"), SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT);
                        //    SwinGame.FillRectangle(Color.LightBlue, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                        //Else
                        //    SwinGame.FillRectangle(Color.Gray, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    }

                    //SwinGame.DrawRectangle(Color.Black, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    //SwinGame.DrawText(sn.ToString(), Color.Black, GameFont("Courier"), SHIPS_LEFT + TEXT_OFFSET, SHIPS_TOP + i * SHIPS_HEIGHT)

                }

            }

            if (GameController.HumanPlayer.ReadyToDeploy)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("PlayButton"), PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.FillRectangle(Color.LightBlue, PLAY_BUTTON_LEFT, PLAY_BUTTON_TOP, PLAY_BUTTON_WIDTH, PLAY_BUTTON_HEIGHT)
                //SwinGame.DrawText("PLAY", Color.Black, GameFont("Courier"), PLAY_BUTTON_LEFT + TEXT_OFFSET, PLAY_BUTTON_TOP)
            }

            SwinGame.DrawBitmap(GameResources.GameImage("RandomButton"), RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP);
            UtilityFunctions.DrawMessage();

            if (help_screen)
            {
                UtilityFunctions.DrawHelp(new string[] { "Up Arrow: chagen direction of the ships to vertical", "Right Arrow: change direction of ships to horizontal", "Random button: to change position of ships randomly", "Play button : start the game", "Esc key : to esc this mode" });
            }
        }

        // Summary: Gets the ship that the mouse is currently over in the selection panel.
        // Returns: The ship selected or none
        private static ShipName GetShipMouseIsOver()
        {
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i = 0;
                i = ((int)sn) - 1;

                if (UtilityFunctions.IsMouseInRectangle(SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT))
                {
                    return sn;
                }
            }
            return ShipName.None;
        }
    }
}
