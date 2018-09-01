// AttackResult gives the result after a shot has been made.
public class AttackResult
{
    
    private ResultOfAttack _value;
    
    private Ship _ship;
    
    private string _text;
    
    private int _row;
    
    private int _column;
    
    // The result of the attack
    // Returns: The result of the attack
    // Properties below this are fairly self explanatory.
    public ResultOfAttack Value
	{
        get
		{
            return _value;
        }
    }
    
    public Ship Ship
	{
        get
		{
            return _ship;
        }
    }
    
    public string Text
	{
        get
		{
            return _text;
        }
    }
    
    public int Row
	{
        get
		{
            return _row;
        }
    }
    
    public int Column
	{
        get
		{
            return _column;
        }
    }
    
    public AttackResult(ResultOfAttack value, string text, int row, int column)
	{
        _value = value;
        _text = text;
        _ship = null;
        _row = row;
        _column = column;
    }
    
    // Set the _value to the PossibleAttack value, and the _ship to the ship
    // Parameter 'value': either hit, miss, destroyed, shotalready
    // Parameter 'ship': the ship information
    // Parameter 'text': text to display for the attack result
    // Parameter 'row': row on the board hit
    // Parameter 'column': column on the board hit

    public AttackResult(ResultOfAttack value, Ship ship, string text, int row, int column) : 
        this(value, text, row, column)
	{
        _ship = ship;
    }

    // Returns The textual information about the attack
    public override string ToString()
	{
        if((_Ship == null))
		{
            return Text;
        }
        
        return (Text + (" " + _ship.Name));
    }
}