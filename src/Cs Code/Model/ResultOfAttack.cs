/*
  Summary:
  The result of an attack.
*/
namespace BattleShips
{
    public enum ResultOfAttack
    {
        // Summary: The player hit something
        Hit,

        //Summary: The player missed
        Miss,

        //Summary: The player destroyed a ship
        Destroyed,

        //Summary: That location was already shot.
        ShotAlready,

        //Summary: The player killed all of the opponents ships
        GameOver,
    }
}