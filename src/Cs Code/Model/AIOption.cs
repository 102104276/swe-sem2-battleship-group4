//Summary: The different AI levels.

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public enum AIOption
    {
        //Summary: Easy, total random shooting
        Easy,

        //Summary Medium, marks squares around hits
        Medium,

        //Summary: As same as medium, but removes shots once it misses
        Hard
    }
}