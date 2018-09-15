
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
//Summary:
//The different AI levels.
namespace BattleShips
{
    public enum AIOption
    {
        //Summary:
        //Easy, total random shooting
        Easy,

        //Summary
        //Medium, marks squares around hits
        Medium,

        //Summary:
        //As medium, but removes shots once it misses
        Hard
    }
}