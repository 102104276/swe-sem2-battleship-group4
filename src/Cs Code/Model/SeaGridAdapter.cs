/*
 Summary:
 The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
 presented it changes the view into a sea tile instead of a ship tile.
*/

public class SeaGridAdapter : ISeaGrid {

    private SeaGrid _MyGrid;

    /*
     Summary
     Create the SeaGridAdapter, with the grid, and it will allow it to be changed

     Parameter: grid: the grid that needs to be adapted
     */

    public SeaGridAdapter(SeaGrid grid) {
        _MyGrid = grid;
        _MyGrid.Changed += new EventHandler(// TODO: Warning!!!! NULL EXPRESSION DETECTED...
        .);
    }

    /*
     Summary
     MyGrid_Changed causes the grid to be redrawn by raising a changed event

     Parameter: sender - the object that caused the change
     Parameter: e - what needs to be redrawn
     */

    private void MyGrid_Changed(object sender, EventArgs e) {
        Changed(this, e);
    }

    /*
     Summary:
     Changes the discovery grid. Where there is a ship we will sea water

     Parameter: x - tile x coordinate
     Parameter: y - tile y coordinate
     Returns : a tile, either what it actually is, or if it was a ship then return a sea tile</returns>
     */

    public TileView this[int x, int y] {
    }
}

  EndPropertyImplementsISeaGrid.Changed;
  Endclass Unknown {}

    //Summary: Indicates that the grid has been changed
    public event EventHandler Changed;

    //Summary: Get the width of a tile
    public int Width {
        get {
            return _MyGrid.Width;
        }
    }

    public int Height {
        get {
            return _MyGrid.Height;
        }
    }

    public AttackResult HitTile(int row, int col) {
        return _MyGrid.HitTile(row, col);
    }
