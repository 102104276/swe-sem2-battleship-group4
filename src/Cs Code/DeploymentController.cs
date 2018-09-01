using SwinGameSDK;
using System;
using System.Collections.Generic;

/*
Summary: The DeploymentController controls the players actions
during the deployment phase.
*/

namespace BattleShips
{
    class DeploymentController
    {

        private const int SHIPS_TOP = 98;

        private const int SHIPS_LEFT = 20;

        private const int SHIPS_HEIGHT = 90;

        private const int SHIPS_WIDTH = 300;

        private const int TOP_BUTTONS_TOP = 72;

        private const int TOP_BUTTONS_HEIGHT = 46;

        private const int PLAY_BUTTON_LEFT = 693;

        private const int PLAY_BUTTON_WIDTH = 80;

        private const int UP_DOWN_BUTTON_LEFT = 410;

        private const int LEFT_RIGHT_BUTTON_LEFT = 350;

        private const int RANDOM_BUTTON_LEFT = 547;

        private const int RANDOM_BUTTON_WIDTH = 51;

        private const int DIR_BUTTONS_WIDTH = 47;

        private const int TEXT_OFFset = 5;

        private Direction _currentDirection = Direction.UpDown;

        private ShipName _selectedShip = ShipName.Tug;

        /*
        Summary
        Handles user input for the Deployment phase of the game.

        Remarks
        Involves selecting the ships, deloying ships, changing the direction
        of the ships to add, randomising deployment, end then ending
        deployment
        */

        public static void HandleDeploymentInput()
        {
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                GameController.AddNewState(GameState.ViewingGameMenu);
            }

            if ((SwinGame.KeyTyped(KeyCode.vk_UP) || SwinGame.KeyTyped(KeyCode.vk_DOWN)))
            {
                _currentDirection = Direction.UpDown;
            }

            if ((SwinGame.KeyTyped(KeyCode.vk_LEFT) || SwinGame.KeyTyped(KeyCode.vk_RIGHT)))
            {
                _currentDirection = Direction.LeftRight;
            }

            if (SwinGame.KeyTyped(KeyCode.vk_r))
            {
                HumanPlayer.RandomizeDeployment();
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                ShipName selected;
                selected = getShipMouseIsOver();
                if ((selected != ShipName.None))
                {
                    _selectedShip = selected;
                }
                else
                {
                    DeploymentController.DoDeployClick();
                }

                if ((HumanPlayer.ReadyToDeploy && UtilityFunctions.IsMouseInRectangle(PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP, PLAY_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT)))
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
                    HumanPlayer.RandomizeDeployment();
                }
            }
        }

        /*
        Summary:
        The user has clicked somewhere on the screen, check if its is a deployment and deploy
        the current ship if that is the case.

        Remarks:
        if the click is in the grid it deploys to the selected location
        with the indicated direction
        */

        private static void DoDeployClick()
        {
            Point2D mouse;
            mouse = SwinGame.MousePosition();
            // Calculate the row/col clicked
            int row;
            int col;
            row = Convert.ToInt32(Math.Floor((mouse.Y / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP))));
            col = Convert.ToInt32(Math.Floor(((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP))));
            if (((row >= 0) && (row < HumanPlayer.PlayerGrid.Height)))
            {
                if (((col >= 0) && (col < HumanPlayer.PlayerGrid.Width)))
                {
                    // if in the area try to deploy
                    try
                    {
                        HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        

                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        Message = ex.Message;
                    }
                }
            }
        }

        /*
        Summary
        Draws the deployment screen showing the field and the ships
        that the player can deploy.
        */

        public static void DrawDeployment()
        {
            UtilityFunctions.DrawField(HumanPlayer.PlayerGrid, HumanPlayer, true);
            // Draw the Left/Right and Up/Down buttons
            if ((_currentDirection == Direction.LeftRight))
            {
                SwinGame.DrawBitmap(GameResources.GameImage("LeftRightButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                // SwinGame.DrawText("U/D", Color.Gray, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                // SwinGame.DrawText("L/R", Color.White, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }
            else
            {
                SwinGame.DrawBitmap(GameResources.GameImage("UpDownButton"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                // SwinGame.DrawText("U/D", Color.White, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                // SwinGame.DrawText("L/R", Color.Gray, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }

            // DrawShips
            foreach (ShipName sn in enum.getValues(typeof(ShipName))
            {
                int i;
                i = (Int(sn) - 1);
                if ((i >= 0))
                {
                    if ((sn == _selectedShip))
                    {
                        SwinGame.DrawBitmap(GameResources.GameImage("SelectedShip"), SHIPS_LEFT, (SHIPS_TOP + (i * SHIPS_HEIGHT)));
                        //     SwinGame.FillRectangle(Color.LightBlue, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                        // else
                        //     SwinGame.FillRectangle(Color.Gray, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    }

                    // SwinGame.DrawRectangle(Color.Black, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    // SwinGame.DrawText(sn.Tostring(), Color.Black, GameFont("Courier"), SHIPS_LEFT + TEXT_OFFset, SHIPS_TOP + i * SHIPS_HEIGHT)
                }

            }

            if (HumanPlayer.ReadyToDeploy)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("PlayButton"), PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP);
                // SwinGame.FillRectangle(Color.LightBlue, PLAY_BUTTON_LEFT, PLAY_BUTTON_TOP, PLAY_BUTTON_WIDTH, PLAY_BUTTON_HEIGHT)
                // SwinGame.DrawText("PLAY", Color.Black, GameFont("Courier"), PLAY_BUTTON_LEFT + TEXT_OFFset, PLAY_BUTTON_TOP)
            }

            SwinGame.DrawBitmap(GameResources.GameImage("RandomButton"), RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP);
            DrawMessage();
        }

        /*
        Summary
        gets the ship that the mouse is currently over in the selection panel.

        returns:
        The ship selected or none
        */

        private static ShipName getShipMouseIsOver()
        {
            foreach (ShipName sn in enum.getValues(typeof(ShipName)))
            {
                int i;
                i = (int(sn) - 1);
    
                if (IsMouseInRectangle(SHIPS_LEFT, (SHIPS_TOP + (i * SHIPS_HEIGHT)), SHIPS_WIDTH, SHIPS_HEIGHT))
                {
                    return sn;
                }
            }

            return ShipName.None;
        }
    }

}