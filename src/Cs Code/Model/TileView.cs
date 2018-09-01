// The values that are visible for a given tile.
namespace BattleShips
{
    public enum TileView
    {
        // The viewer can see sea
        // May be masking a ship if viewed via a sea adapter
        Sea,
        
        /*
    	The viewer knows that site was attacked but nothing
        was hit
    	*/
        Miss,
        
        // The viewer can see a ship at this site
        Ship,
        
        /*
    	The viewer knows that the site was attacked and
        something was hit
        */
    	Hit,
    }
}