Using SwinGameSDK;
// '' <summary>
// '' The GameController is responsible for controlling the game,
// '' managing user input, and displaying the current state of the
// '' game.
// '' </summary>
Public Class GameController {
    
    Private BattleShipsGame _theGame;
    
    Private Player _human;
    
    Private AIPlayer _ai;
    
    Private Stack<GameState> _state = New Stack<GameState>();
    
    Private AIOption _aiSetting;
    
    // '' <summary>
    // '' Returns the current state of the game, indicating which screen is
    // '' currently being used
    // '' </summary>
    // '' <value>The current state</value>
    // '' <returns>The current state</returns>
    Public GameState CurrentState {
        Get {
            Return _state.Peek();
        }
    }
    
    Public Player HumanPlayer {
        Get {
            Return _human;
        }
    }
    
    Public Player ComputerPlayer {
        Get {
            Return _ai;
        }
    }
    
    GameController() {
        // bottom state will be quitting. If player exits main menu then the game Is over
        _state.Push(GameState.Quitting);
        // at the start the player Is viewing the main menu
        _state.Push(GameState.ViewingMainMenu);
    }
    
    // '' <summary>
    // '' Starts a new game.
    // '' </summary>
    // '' <remarks>
    // '' Creates an AI player based upon the _aiSetting.
    // '' </remarks>
    Public Static void StartGame() {
        If (_theGame) {
            IsNot;
            null;
            GameController.EndGame();
            // Create the game
            _theGame = New BattleShipsGame();
            // create the players
            switch (_aiSetting) {
                Case AIOption.Medium : 
                    _ai = New AIMediumPlayer(_theGame);
                    break;
                Case AIOption.Hard : 
                    _ai = New AIHardPlayer(_theGame);
                    break;
                Default: 
                    _ai = New AIHardPlayer(_theGame);
                    break;
            }
            _human = New Player(_theGame);
            // AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
            _ai.PlayerGrid.Changed += New System.EventHandler(this.GridChanged);
            _theGame.AttackCompleted += New System.EventHandler(this.AttackCompleted);
            GameController.AddNewState(GameState.Deploying);
        }
        
        // '' <summary>
        // '' Stops listening to the old game once a new game is started
        // '' </summary>
    }
    
    Static void EndGame() {
        // RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
        _ai.PlayerGrid.Changed;
        New System.EventHandler(this.GridChanged);
        _theGame.AttackCompleted;
        New System.EventHandler(this.AttackCompleted);
    }
    
    // '' <summary>
    // '' Listens to the game grids for any changes and redraws the screen
    // '' when the grids change
    // '' </summary>
    // '' <param name="sender">the grid that changed</param>
    // '' <param name="args">not used</param>
    Private Static void GridChanged(Object sender, EventArgs args) {
        GameController.DrawScreen();
        SwinGame.RefreshScreen();
    }
    
    Private Static void PlayHitSequence(int row, int column, bool showAnimation) {
        If (showAnimation) {
            AddExplosion(row, column);
        }
        
        Audio.PlaySoundEffect(GameSound("Hit"));
        DrawAnimationSequence();
    }
    
    Private Static void PlayMissSequence(int row, int column, bool showAnimation) {
        If (showAnimation) {
            AddSplash(row, column);
        }
        
        Audio.PlaySoundEffect(GameSound("Miss"));
        DrawAnimationSequence();
    }
    
    // '' <summary>
    // '' Listens for attacks to be completed.
    // '' </summary>
    // '' <param name="sender">the game</param>
    // '' <param name="result">the result of the attack</param>
    // '' <remarks>
    // '' Displays a message, plays sound and redraws the screen
    // '' </remarks>
    Private Static void AttackCompleted(Object sender, AttackResult result) {
        bool isHuman;
        isHuman = (_theGame.Player == HumanPlayer);
        If (isHuman) {
            Message = ("You " + result.ToString());
        }
        Else {
            Message = ("The AI " + result.ToString());
        }
        
        switch (result.Value) {
            Case ResultOfAttack.Destroyed : 
                GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                Audio.PlaySoundEffect(GameSound("Sink"));
                break;
            Case ResultOfAttack.GameOver : 
                GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                Audio.PlaySoundEffect(GameSound("Sink"));
                While (Audio.SoundEffectPlaying(GameSound("Sink"))) {
                    SwinGame.Delay(10);
                    SwinGame.RefreshScreen();
                }
                
                If (HumanPlayer.IsDestroyed) {
                    Audio.PlaySoundEffect(GameSound("Lose"));
                }
                Else {
                    Audio.PlaySoundEffect(GameSound("Winner"));
                }
                
                break;
            Case ResultOfAttack.Hit : 
                GameController.PlayHitSequence(result.Row, result.Column, isHuman);
                break;
            Case ResultOfAttack.Miss : 
                GameController.PlayMissSequence(result.Row, result.Column, isHuman);
                break;
            Case ResultOfAttack.ShotAlready : 
                Audio.PlaySoundEffect(GameSound("Error"));
                break;
        }
    }
    
    // '' <summary>
    // '' Completes the deployment phase of the game and
    // '' switches to the battle mode (Discovering state)
    // '' </summary>
    // '' <remarks>
    // '' This adds the players to the game before switching
    // '' state.
    // '' </remarks>
    Public Static void EndDeployment() {
        // deploy the players
        _theGame.AddDeployedPlayer(_human);
        _theGame.AddDeployedPlayer(_ai);
        GameController.SwitchState(GameState.Discovering);
    }
    
    // '' <summary>
    // '' Gets the player to attack the indicated row and column.
    // '' </summary>
    // '' <param name="row">the row to attack</param>
    // '' <param name="col">the column to attack</param>
    // '' <remarks>
    // '' Checks the attack result once the attack is complete
    // '' </remarks>
    Public Static void Attack(int row, int col) {
        AttackResult result;
        result = _theGame.Shoot(row, col);
        GameController.CheckAttackResult(result);
    }
    
    // '' <summary>
    // '' Gets the AI to attack.
    // '' </summary>
    // '' <remarks>
    // '' Checks the attack result once the attack is complete.
    // '' </remarks>
    Private Static void AIAttack() {
        AttackResult result;
        result = _theGame.Player.Attack();
        GameController.CheckAttackResult(result);
    }
    
    // '' <summary>
    // '' Checks the results of the attack and switches to
    // '' Ending the Game if the result was game over.
    // '' </summary>
    // '' <param name="result">the result of the last
    // '' attack</param>
    // '' <remarks>Gets the AI to attack if the result switched
    // '' to the AI player.</remarks>
    Private Static void CheckAttackResult(AttackResult result) {
        switch (result.Value) {
            Case ResultOfAttack.Miss
    If ((_theGame.Player == ComputerPlayer)) {
                    GameController.AIAttack();
                }
                
                break;
            Case ResultOfAttack.GameOver : 
                GameController.SwitchState(GameState.EndingGame);
                break;
        }
    }
    
    // '' <summary>
    // '' Handles the user SwinGame.
    // '' </summary>
    // '' <remarks>
    // '' Reads key and mouse input and converts these into
    // '' actions for the game to perform. The actions
    // '' performed depend upon the state of the game.
    // '' </remarks>
    Public Static void HandleUserInput() {
        // Read incoming input events
        SwinGame.ProcessEvents();
        switch (CurrentState) {
            Case GameState.ViewingMainMenu : 
                HandleMainMenuInput();
                break;
            Case GameState.ViewingGameMenu : 
                HandleGameMenuInput();
                break;
            Case GameState.AlteringSettings : 
                HandleSetupMenuInput();
                break;
            Case GameState.Deploying : 
                HandleDeploymentInput();
                break;
            Case GameState.Discovering : 
                HandleDiscoveryInput();
                break;
            Case GameState.EndingGame : 
                HandleEndOfGameInput();
                break;
            Case GameState.ViewingHighScores : 
                HandleHighScoreInput();
                break;
        }
        UpdateAnimations();
    }
    
    // '' <summary>
    // '' Draws the current state of the game to the screen.
    // '' </summary>
    // '' <remarks>
    // '' What is drawn depends upon the state of the game.
    // '' </remarks>
    Public Static void DrawScreen() {
        DrawBackground();
        switch (CurrentState) {
            Case GameState.ViewingMainMenu : 
                DrawMainMenu();
                break;
            Case GameState.ViewingGameMenu : 
                DrawGameMenu();
                break;
            Case GameState.AlteringSettings : 
                DrawSettings();
                break;
            Case GameState.Deploying : 
                DrawDeployment();
                break;
            Case GameState.Discovering : 
                DrawDiscovery();
                break;
            Case GameState.EndingGame : 
                DrawEndOfGame();
                break;
            Case GameState.ViewingHighScores : 
                DrawHighScores();
                break;
        }
        DrawAnimations();
        SwinGame.RefreshScreen();
    }
    
    // '' <summary>
    // '' Move the game to a new state. The current state is maintained
    // '' so that it can be returned to.
    // '' </summary>
    // '' <param name="state">the new game state</param>
    Public Static void AddNewState(GameState state) {
        _state.Push(state);
        Message = "";
    }
    
    // '' <summary>
    // '' End the current state and add in the new state.
    // '' </summary>
    // '' <param name="newState">the new state of the game</param>
    Public Static void SwitchState(GameState newState) {
        GameController.EndCurrentState();
        GameController.AddNewState(newState);
    }
    
    // '' <summary>
    // '' Ends the current state, returning to the prior state
    // '' </summary>
    Public Static void EndCurrentState() {
        _state.Pop();
    }
    
    // '' <summary>
    // '' Sets the difficulty for the next level of the game.
    // '' </summary>
    // '' <param name="setting">the new difficulty level</param>
    Public Static void SetDifficulty(AIOption setting) {
        _aiSetting = setting;
    }
}