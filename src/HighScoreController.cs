Using System.IO;
Using SwinGameSDK;
// '' <summary>
// '' Controls displaying and collecting high score data.
// '' </summary>
// '' <remarks>
// '' Data is saved to a file.
// '' </remarks>
Class HighScoreController {
    
    Private Const int NAME_WIDTH = 3;
    
    Private Const int SCORES_LEFT = 490;
    
    // '' <summary>
    // '' The score structure is used to keep the name and
    // '' score of the top players together.
    // '' </summary>
    Private struct Score {
        
        Public String Name;
        
        Public int Value;
        
        // '' <summary>
        // '' Allows scores to be compared to facilitate sorting
        // '' </summary>
        // '' <param name="obj">the object to compare to</param>
        // '' <returns>a value that indicates the sort order</returns>
        Public Static int CompareTo(Object obj) {
            If ((obj.GetType() == Score)) {
                Score other = ((Score)(obj));
                Return (other.Value - this.Value);
            }
            Else {
                Return 0;
            }
            
        }
    }
    
    Private List<Score> _Scores = New List<Score>();
    
    Private Static void LoadScores() {
        String filename;
        filename = SwinGame.PathToResource("highscores.txt");
        StreamReader input;
        input = New StreamReader(filename);
        // Read in the # of scores
        int numScores;
        numScores = Convert.ToInt32(input.ReadLine());
        _Scores.Clear();
        int i;
        For (i = 1; (i <= numScores); i++) {
            Score s;
            String line;
            line = input.ReadLine();
            s.Name = line.Substring(0, NAME_WIDTH);
            s.Value = Convert.ToInt32(line.Substring(NAME_WIDTH));
            _Scores.Add(s);
        }
        
        input.Close();
    }
    
    // '' <summary>
    // '' Saves the scores back to the highscores text file.
    // '' </summary>
    // '' <remarks>
    // '' The format is
    // '' # of scores
    // '' NNNSSS
    // '' 
    // '' Where NNN is the name and SSS is the score
    // '' </remarks>
    Private Static void SaveScores() {
        String filename;
        filename = SwinGame.PathToResource("highscores.txt");
        StreamWriter output;
        output = New StreamWriter(filename);
        output.WriteLine(_Scores.Count);
        foreach (Score s in _Scores) {
            output.WriteLine((s.Name + s.Value));
        }
        
        output.Close();
    }
    
    // '' <summary>
    // '' Draws the high scores to the screen.
    // '' </summary>
    Public Static void DrawHighScores() {
        Const int SCORES_HEADING = 40;
        Const int SCORES_TOP = 80;
        Const int SCORE_GAP = 30;
        If ((_Scores.Count == 0)) {
            HighScoreController.LoadScores();
        }
        
        SwinGame.DrawText("   High Scores   ", Color.White, GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);
        // For all of the scores
        int i;
        For (i = 0; (i 
                    <= (_Scores.Count - 1)); i++) {
            Score s;
            s = _Scores.Item[i];
            // for scores 1 - 9 use 01 - 09
            If ((i < 9)) {
                SwinGame.DrawText((" " 
                                + ((i + 1) + (":   " 
                                + (s.Name + ("   " + s.Value))))), Color.White, GameFont("Courier"), SCORES_LEFT, (SCORES_TOP 
                                + (i * SCORE_GAP)));
            }
            Else {
                SwinGame.DrawText(((i + 1) + (":   " 
                                + (s.Name + ("   " + s.Value)))), Color.White, GameFont("Courier"), SCORES_LEFT, (SCORES_TOP 
                                + (i * SCORE_GAP)));
            }
            
        }
        
    }
    
    // '' <summary>
    // '' Handles the user input during the top score screen.
    // '' </summary>
    // '' <remarks></remarks>
    Public Static void HandleHighScoreInput() {
        If ((SwinGame.MouseClicked(MouseButton.LeftButton) 
                    || (SwinGame.KeyTyped(KeyCode.VK_ESCAPE) || SwinGame.KeyTyped(KeyCode.VK_RETURN)))) {
            EndCurrentState();
        }
        
    }
    
    // '' <summary>
    // '' Read the user's name for their highsSwinGame.
    // '' </summary>
    // '' <param name="value">the player's sSwinGame.</param>
    // '' <remarks>
    // '' This verifies if the score is a highsSwinGame.
    // '' </remarks>
    Public Static void ReadHighScore(int value) {
        Const int ENTRY_TOP = 500;
        If ((_Scores.Count == 0)) {
            HighScoreController.LoadScores();
        }
        
        // Is it a high score
        If ((value > _Scores.Item[(_Scores.Count - 1)].Value)) {
            Score s = New Score();
            s.Value = value;
            AddNewState(GameState.ViewingHighScores);
            int x;
            x = (SCORES_LEFT + SwinGame.TextWidth(GameFont("Courier"), "Name: "));
            SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameFont("Courier"), x, ENTRY_TOP);
            // Read the text from the user
            While (SwinGame.ReadingText()) {
                SwinGame.ProcessEvents();
                DrawBackground();
                HighScoreController.DrawHighScores();
                SwinGame.DrawText("Name: ", Color.White, GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
                SwinGame.RefreshScreen();
            }
            
            s.Name = SwinGame.TextReadAsASCII();
            If ((s.Name.Length < 3)) {
                s.Name = (s.Name + New string(((char)(" ")), (3 - s.Name.Length)));
            }
            
            _Scores.RemoveAt((_Scores.Count - 1));
            _Scores.Add(s);
            _Scores.Sort();
            EndCurrentState();
        }
        
    }
}