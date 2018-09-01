using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace BattleShips
{

    class GameLogic
    {

        public static void Main()
        {
            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);
            // Load Resources
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            // Game Loop
            for (; (((SwinGame.WindowCloseRequested() == true) || (CurrentState == GameState.Quitting)) == false);)
            {
                GameController.HandleUserInput();
                GameController.DrawScreen();
            }

            SwinGame.StopMusic();
            // Free Resources and Close Audio, to end the program.
            GameResources.FreeResources();
        }
    }
}
