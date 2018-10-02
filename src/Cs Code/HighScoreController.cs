// Summary: HighScoreController controls displaying and collecting high score data.
// Remarks: Data is saved to a file.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SwinGameSDK;
namespace BattleShips
{
    static class HighScoreController
    {
        private const int NAME_WIDTH = 3;
        private const int SCORES_LEFT = 490;
        /* 
           Summary: The score structure is used to keep the name and
           score of the top players together.
        */ 
        private struct Score : IComparable
        {
            public string Name;

            public int Value;

            // Summary: Allows scores to be compared to facilitate sorting
            // obj: the object to compare to
            // Returns: a value that indicates the sort order
            public int CompareTo(object obj)
            {
                if (obj is Score)
                {
                    Score other = (Score)obj;

                    return other.Value - this.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        private static List<Score> _scores = new List<Score>();
        // Summary: Loads the scores from the highscores text file.
        // Remarks: The format is # of scores NNNSSS Where NNN is the name and SSS is the score
        private static void LoadScores()
        {
            string filename = null;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamReader input = default(StreamReader);
            input = new StreamReader(filename);

            //Read in the # of scores
            int numScores = 0;
            numScores = Convert.ToInt32(input.ReadLine());

            _scores.Clear();

            int i = 0;

            //Reads in each line
            for (i = 1; i <= numScores; i++)
            {
                Score s = default(Score);
                string line = null;

                line = input.ReadLine();

                s.Name = line.Substring(0, NAME_WIDTH);
                s.Value = Convert.ToInt32(line.Substring(NAME_WIDTH));

                _scores.Add(s);
            }
            input.Close();
        }

        // Summary: Saves the scores back to the highscores text file.
        // Remarks: The format is # of scores NNNSSS Where NNN is the name and SSS is the score
        private static void SaveScores()
        {
            string filename = null;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamWriter output = default(StreamWriter);
            output = new StreamWriter(filename);

            output.WriteLine(_scores.Count);

            foreach (Score s in _scores)
            {
                output.WriteLine(s.Name + s.Value);
            }

            output.Close();
        }

        // Summary: Draws the high scores to the screen.
        public static void DrawHighScores()
        {
            const int SCORES_HEADING = 40;
            const int SCORES_TOP = 80;
            const int SCORE_GAP = 30;

            if (_scores.Count == 0)
                LoadScores();

            SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);

            //For all of the scores
            int i = 0;
            for (i = 0; i <= _scores.Count - 1; i++)
            {
                Score s = default(Score);

                s = _scores[i];

                //for scores 1 - 9 use 01 - 09
                if (i < 9)
                {
                    SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
                else
                {
                    SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
            }
        }

        // Summary: Handles the user input during the top score screen.
        // Remarks: Updated the Keycodes
        public static void HandleHighScoreInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN))
            {
                GameController.EndCurrentState();
            }
        }

        // Summary: Read the user's name for their highscore.
        // Value: the player's score.
        // Remarks: This verifies if the score is a highscore.
        public static void ReadHighScore(int value)
        {
            const int ENTRY_TOP = 500;

            if (_scores.Count == 0)
                LoadScores();

            //is it a high score
            if (value > _scores[_scores.Count - 1].Value)
            {
                Score s = new Score();
                s.Value = value;

                GameController.AddNewState(GameState.ViewingHighScores);

                int x = 0;
                x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont("Courier"), "Name: ");

                SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont("Courier"), x, ENTRY_TOP);

                //Read the text from the user
                while (SwinGame.ReadingText())
                {
                    SwinGame.ProcessEvents();

                    UtilityFunctions.DrawBackground();
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = SwinGame.TextReadAsASCII();

                if (s.Name.Length <= 3)
                {
                    s.Name = s.Name + new string(Convert.ToChar(" "), 3 - s.Name.Length);
                }

                //Slides the new score into the correct positionC:\Users\timke\OneDrive\Documents\dev project\battleships\src\Cs Code\HighScoreController.cs
                _scores.RemoveAt(_scores.Count - 1);
                _scores.Add(s);
                _scores.Sort();
                HighScoreController.SaveScores();
                GameController.EndCurrentState();
            }
        }
    }
}