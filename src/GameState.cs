/*
Summary:
The GameStates represent the state of the Battleships game play.
This is used to control the actions and view displayed to
the player.
*/
public enum GameState {

    /*
    Summary: The player is viewing the main menu.
    ViewingMainMenu,


    Summary: The player is viewing the game menu
    ViewingGameMenu,

    Summary: The player is looking at the high scores
    ViewingHighScores,

    Summary: The player is altering the game settings
    AlteringSettings,

    Summary: Players are deploying their ships
    Deploying,

    Summary: Players are attempting to locate each others ships
    Discovering,

    Summary
    One player has won, showing the victory screen
    EndingGame,

    Summary
    The player has quit. Show ending credits and terminate the game
    Quitting,
}
