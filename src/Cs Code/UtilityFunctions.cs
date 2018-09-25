/*
  Summary: This Class includes a number of utility methods for
  drawing and interacting with the Mouse.
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

/*
Summary: This includes a number of utility methods for
drawing and interacting with the Mouse.
*/

namespace BattleShips
{
    static class UtilityFunctions
    {
        public const int FIELD_TOP = 122;
        public const int FIELD_LEFT = 349;
        public const int FIELD_WIDTH = 418;

        public const int FIELD_HEIGHT = 418;

        public const int MESSAGE_TOP = 548;
        public const int CELL_WIDTH = 40;
        public const int CELL_HEIGHT = 40;

        public const int CELL_GAP = 2;

        public const int SHIP_GAP = 3;
        private static readonly Color SMALL_SEA = SwinGame.RGBAColor(6, 60, 94, 255);
        private static readonly Color SMALL_SHIP = Color.Gray;
        private static readonly Color SMALL_MISS = SwinGame.RGBAColor(1, 147, 220, 255);

        private static readonly Color SMALL_HIT = SwinGame.RGBAColor(169, 24, 37, 255);
        private static readonly Color LARGE_SEA = SwinGame.RGBAColor(6, 60, 94, 255);
        private static readonly Color LARGE_SHIP = Color.Gray;
        private static readonly Color LARGE_MISS = SwinGame.RGBAColor(1, 147, 220, 255);

        private static readonly Color LARGE_HIT = SwinGame.RGBAColor(252, 2, 3, 255);
        private static readonly Color OUTLINE_COLOR = SwinGame.RGBAColor(5, 55, 88, 255);
        private static readonly Color SHIP_FILL_COLOR = Color.Gray;
        private static readonly Color SHIP_OUTLINE_COLOR = Color.White;

        private static readonly Color MESSAGE_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);
        public const int ANIMATION_CELLS = 7;

        public const int FRAMES_PER_CELL = 8;

        private static bool sfx_active = true;


        public static bool SFX_ACTIVE
        {
            get
            {
                return sfx_active;
            }
        }

        /*
          Summary: Determines if the mouse is in a given rectangle.
          Parameter: X - the x location to check
          Parameter: Y - the y location to check
          Paramenter: W - the width to check
          Parameter: H - the height to check
          Returns: true if the mouse is in the area checked
        */

        public static bool IsMouseInRectangle(int x, int y, int w, int h)
        {
            Point2D mouse = default(Point2D);
            bool result = false;

            mouse = SwinGame.MousePosition();

            //if the mouse is inline with the button horizontally
            if (mouse.X >= x & mouse.X <= x + w)
            {
                //Check vertical position
                if (mouse.Y >= y & mouse.Y <= y + h)
                {
                    result = true;
                }
            }

            return result;
        }

        /*
          Summary: Draws a large field using the grid and the indicated player's ships.
          Parameter: grid - The grid to draw
          Parameter: thePlayer - the players ships to show
          Parameter: showShips - indicates if the ships should be shown.
        */

        public static void DrawField(ISeaGrid grid, Player thePlayer, bool showShips, bool showonlydestroyed)
        {
            DrawCustomField(grid, thePlayer, false, showShips, FIELD_LEFT, FIELD_TOP, FIELD_WIDTH, FIELD_HEIGHT, CELL_WIDTH, CELL_HEIGHT,
            CELL_GAP, showonlydestroyed);
        }

        /*
          Summary: Draws a small field, showing the attacks made and the locations of the player's ships
          Parameyer: grid - the grid to show
          Parameter: thePlayer - the player to show the ships of
        */

        public static void DrawSmallField(ISeaGrid grid, Player thePlayer)
        {
            const int SMALL_FIELD_LEFT = 39;
            const int SMALL_FIELD_TOP = 373;
            const int SMALL_FIELD_WIDTH = 166;
            const int SMALL_FIELD_HEIGHT = 166;
            const int SMALL_FIELD_CELL_WIDTH = 13;
            const int SMALL_FIELD_CELL_HEIGHT = 13;
            const int SMALL_FIELD_CELL_GAP = 4;

            DrawCustomField(grid, thePlayer, true, true, SMALL_FIELD_LEFT, SMALL_FIELD_TOP, SMALL_FIELD_WIDTH, SMALL_FIELD_HEIGHT, SMALL_FIELD_CELL_WIDTH, SMALL_FIELD_CELL_HEIGHT, SMALL_FIELD_CELL_GAP, false);
        }

        /*
          Summary: Draws the player's grid and ships.
          Parameter: grid - the grid to show
          Parameter: thePlayer - the player to show the ships of
          Parameter: small - true if the small grid is shown
          Parameter: showShips - true if ships are to be shown
          Parameter: left - the left side of the grid
          Parameter: top - the top of the grid
          Parameter: width - the width of the grid
          Parameter: height - the height of the grid
          Parameter: cellWidth - the width of each cell
          Parameter: cellHeight - the height of each cell
          Parameter: cellGap - the gap between the cells
        */

        private static void DrawCustomField(ISeaGrid grid, Player thePlayer, bool small, bool showShips, int left, int top, int width, int height, int cellWidth, int cellHeight, int cellGap, bool showonlydestroyed)
        {
            //SwinGame.FillRectangle(Color.Blue, left, top, width, height)

            int rowTop = 0;
            int colLeft = 0;

            //Draw the grid
            for (int row = 0; row <= 9; row++)
            {
                rowTop = top + (cellGap + cellHeight) * row;

                for (int col = 0; col <= 9; col++)
                {
                    colLeft = left + (cellGap + cellWidth) * col;

                    Color fillColor = default(Color);
                    bool draw = false;

                    draw = true;

                    switch (grid[row, col])
                    {
                        //case TileView.Ship:
                        //draw = false;
                        //break;

                        //If small Then fillColor = _SMALL_SHIP Else fillColor = _LARGE_SHIP
                        case TileView.Miss:
                            if (small)
                                fillColor = SMALL_MISS;
                            else
                                fillColor = LARGE_MISS;
                            break;
                        case TileView.Hit:
                            if (small)
                                fillColor = SMALL_HIT;
                            else
                                fillColor = LARGE_HIT;
                            break;
                        case TileView.Sea:
                        case TileView.Ship:
                            if (small)
                                fillColor = SMALL_SEA;
                            else
                                draw = false;
                            break;
                    }

                    if (draw)
                    {
                        SwinGame.FillRectangle(fillColor, colLeft, rowTop, cellWidth, cellHeight);
                        if (!small)
                        {
                            SwinGame.DrawRectangle(OUTLINE_COLOR, colLeft, rowTop, cellWidth, cellHeight);
                        }
                    }
                }
            }

            if (!showShips)
            {
                return;
            }
            UtilityFunctions.DrawShips(grid, thePlayer, small, showShips, rowTop, colLeft, left, top, width, height, cellWidth, cellHeight, cellGap, showonlydestroyed);

        }

        public static void DrawShips(ISeaGrid grid, Player thePlayer, bool small, bool showShips, int rowTop, int colLeft, int left, int top, int width, int height, int cellWidth, int cellHeight, int cellGap, bool showonlydestroyed)
        {

            int shipHeight = 0;
            int shipWidth = 0;
            string shipName = null;

            //Draw the ships
            foreach (Ship s in thePlayer)
            {
                if (s == null || !s.IsDeployed)
                    continue;
                rowTop = top + (cellGap + cellHeight) * s.Row + SHIP_GAP;
                colLeft = left + (cellGap + cellWidth) * s.Column + SHIP_GAP;

                if (s.Direction == Direction.LeftRight)
                {
                    shipName = "ShipLR" + s.Size;
                    shipHeight = cellHeight - (SHIP_GAP * 2);
                    shipWidth = (cellWidth + cellGap) * s.Size - (SHIP_GAP * 2) - cellGap;
                }
                else
                {
                    //Up down
                    shipName = "ShipUD" + s.Size;
                    shipHeight = (cellHeight + cellGap) * s.Size - (SHIP_GAP * 2) - cellGap;
                    shipWidth = cellWidth - (SHIP_GAP * 2);
                }

                if (showonlydestroyed)
                {
                    if (s.IsDestroyed)
                    {
                        if (!small)
                        {
                            SwinGame.DrawBitmap(GameResources.GameImage(shipName), colLeft, rowTop);
                        }
                        else
                        {
                            SwinGame.FillRectangle(SHIP_FILL_COLOR, colLeft, rowTop, shipWidth, shipHeight);
                            SwinGame.DrawRectangle(SHIP_OUTLINE_COLOR, colLeft, rowTop, shipWidth, shipHeight);
                        }

                    }
                }
                else
                {
                    if (!small)
                    {
                        SwinGame.DrawBitmap(GameResources.GameImage(shipName), colLeft, rowTop);
                    }
                    else
                    {
                        SwinGame.FillRectangle(SHIP_FILL_COLOR, colLeft, rowTop, shipWidth, shipHeight);
                        SwinGame.DrawRectangle(SHIP_OUTLINE_COLOR, colLeft, rowTop, shipWidth, shipHeight);
                    }
                }

            }
        }


        private static string _message;
        /*
          Summary: The message to display
          Value: The message to display
          Returns: The message to display
        */

        public static string Message
        {
            get { return _message; }
            set { _message = value; }
        }


        public static void DrawHelp(string[] text)
        {
            int i;
            Graphics.DrawRectangle(Color.White, 50, 100, 700, 400);
            Graphics.FillRectangle(Color.Blue, 50, 100, 700, 400);
            i = 0;
            foreach (string phrase in text)
            {
                Text.DrawText(phrase, Color.White, 70, (120 + (i * 20)));
                i++;
            }


            
        }

        //Summary: Draws the background for the current state of the game

        public static void DrawMessage()
        {
            SwinGame.DrawText(Message, MESSAGE_COLOR, GameResources.GameFont("Courier"), FIELD_LEFT, MESSAGE_TOP);
        }

        /*
          Summary:
          Draws the background for the current state of the game
          Remarks:
          Isuru: Updated Draw frame rate function;
        */

        public static void DrawBackground()
        {
            switch (GameController.CurrentState)
            {
                case GameState.ViewingMainMenu:
                case GameState.ViewingGameMenu:
                case GameState.AlteringSettings:
                case GameState.ViewingHighScores:
                    SwinGame.DrawBitmap(GameResources.GameImage("Menu"), 0, 0);
                    break;
                case GameState.Discovering:
                case GameState.EndingGame:
                    SwinGame.DrawBitmap(GameResources.GameImage("Discovery"), 0, 0);
                    break;
                case GameState.Deploying:
                    SwinGame.DrawBitmap(GameResources.GameImage("Deploy"), 0, 0);
                    break;
                default:
                    SwinGame.ClearScreen();
                    break;
            }
            SwinGame.DrawFramerate(675, 585);
            //SwinGame.DrawFramerate(675, 585, GameResources.GameFont("CourierSmall"));
        }

        public static void AddExplosion(int row, int col)
        {
            AddAnimation(row, col, "Splash");
        }

        public static void AddSplash(int row, int col)
        {
            AddAnimation(row, col, "Splash");
        }


        private static List<Sprite> _animations = new List<Sprite>();
        private static void AddAnimation(int row, int col, string image)
        {
            Sprite s = default(Sprite);
            Bitmap imgObj = default(Bitmap);

            imgObj = GameResources.GameImage(image);
            imgObj.SetCellDetails(40, 40, 3, 3, 7);

            AnimationScript animation = default(AnimationScript);
            animation = SwinGame.LoadAnimationScript("splash.txt");

            s = SwinGame.CreateSprite(imgObj, animation);
            s.X = FIELD_LEFT + col * (CELL_WIDTH + CELL_GAP);
            s.Y = FIELD_TOP + row * (CELL_HEIGHT + CELL_GAP);

            s.StartAnimation("splash");

            _animations.Add(s);
        }

        public static void UpdateAnimations()
        {
            List<Sprite> ended = new List<Sprite>();

            foreach (Sprite s in _animations)
            {
                SwinGame.UpdateSprite(s);
                if (s.AnimationHasEnded)
                {
                    ended.Add(s);
                }
            }

            foreach (Sprite s in ended)
            {
                _animations.Remove(s);
                SwinGame.FreeSprite(s);
            }
        }

        public static void DrawAnimations()
        {

            foreach (Sprite s in _animations)
            {
                SwinGame.DrawSprite(s);
            }
        }

        public static void DrawAnimationSequence()
        {
            int i = 0;
            for (i = 1; i <= ANIMATION_CELLS * FRAMES_PER_CELL; i++)
            {
                UpdateAnimations();
                GameController.DrawScreen();
            }
        }

        public static void PlayMuisc()
        {
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
        }

        public static void StopMusic()
        {
            Audio.StopMusic();
        }

        public static void RemoveSFX()
        {
            sfx_active = false;          
        }

        public static void LoadSFX()
        {
            sfx_active = true;
        }

        public static void PlaySFX(string sfx_name)
        {
            if (sfx_active)
            {
                Audio.PlaySoundEffect(GameResources.GameSound(sfx_name));
            }
        }

    }
}