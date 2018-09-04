
/*
    Summary:
    AIHardPlayer is a type of player. This AI will know directions of ships
    when it has found 2 ship tiles and will try to destroy that ship. If that ship
    is not destroyed it will shoot the other way. Ship still not destroyed, then
    the AI knows it has hit multiple ships. Then will try to destoy all around tiles
    that have been hit.
*/

namespace BattleShips
{
    public class AIHardPlayer
    {

        // Summary: Target allows the AI to know more things, for example the source of a shot target

        class Target
        {

            private Location _shotAt;

            private Location _source;

            // Summary: The target shot at
            // Value: The target shot at
            // Returns: The target shot at

            public Location ShotAt
            {
                get
                {
                    return _shotAt;
                }
            }

            public Location Source
            {
                get
                {
                    return _source;
                }
            }

            // sets local variables _shotat and _source as the 
            internal Target(Location shootat, Location source)
            {
                _shotAt = shootat;
                _source = source;
            }

            // Summary: If source shot and shootat shot are on the same row then give a boolean true

            public bool SameRow
            {
                get
                {
                    return;
                }
            }

            // Summary: If source shot and shootat shot are on the same column then give a boolean true

            public bool SameColumn
            {
                get
                {
                    return;
                }
            }
        }

        /*
            Summary:
            Private enumarator for AI states. currently there are two states,
            the AI can be searching for a ship, or if it has found a ship it will
            target the same ship
        */

        private enum AIStates
        {

            // Summary: The AI is searching for its next target

            Searching,

            // Summary: The AI is trying to target a ship

            TargetingShip,

            // Summary: The AI is locked onto a ship

            HittingShip,
        }

        private AIStates _currentState = AIStates.Searching;

        private Stack<Target> _targets = new Stack<Target>();

        private List<Target> _lastHit = new List<Target>();

        private Target _currentTarget;

        public AIHardPlayer(BattleShipsGame game) :
                base(game)
        {
        }

        // Summary: GenerateCoords will call upon the right methods to generate the appropriate shooting coordinates
        // Parameter: row - the row that will be shot at
        // Parameter: column - the column that will be shot at

        protected override void GenerateCoords(ref int row, ref int column)
        {
            for (
            ; ((row < 0)
                        || ((column < 0)
                        || ((row >= EnemyGrid.Height)
                        || ((column >= EnemyGrid.Width)
                        || (EnemyGrid.Item[row, column] != TileView.Sea)))));
            )
            {
                _currentTarget = null;
                switch (_currentState)
                {
                    case AIStates.Searching:
                        this.SearchCoords(row, column);
                        break;
                    case AIStates.TargetingShip:
                    case AIStates.HittingShip:
                        this.TargetCoords(row, column);
                        break;
                    default:
                        throw new ApplicationException("AI has gone in an invalid state");
                        break;
                }
            }

        }

        // Summary: TargetCoords is used when a ship has been hit and it will try and destroy this ship
        // Parameter: row - row generated around the hit tile
        // Parameter: column - column generated around the hit tile

        private void TargetCoords(ref int row, ref int column)
        {
            Target t;
            t = _targets.Pop();
            row = t.ShotAt.Row;
            column = t.ShotAt.Column;
            _currentTarget = t;
        }

        // Summary: SearchCoords will randomly generate shots within the grid as long as its not hit that tile already
        // Parameter: row - the generated row
        // Parameter: column - the generated column

        private void SearchCoords(ref int row, ref int column)
        {  // underscores removed at Random.next
            row = Random.Next(0, EnemyGrid.Height);
            column = Random.Next(0, EnemyGrid.Width);
            _currentTarget = new Target(new Location(row, column), null);
        }

        /*
            Summary:
            ProcessShot is able to process each shot that is made and call the right methods belonging
            to that shot. For example, if its a miss = do nothing, if it's a hit = process that hit location
        */
        // Parameter: row - the row that was shot at
        // Parameter: col - the column that was shot at
        // Parameter: result - the result from that hit
        protected override void ProcessShot(int row, int col, AttackResult result)
        {
            switch (result.Value)
            {
                case ResultOfAttack.Miss:
                    _currentTarget = null;
                    break;
                case ResultOfAttack.Hit:
                    ProcessHit(row, col);
                    break;
                case ResultOfAttack.Destroyed:
                    this.ProcessDestroy(row, col, result.Ship);
                    break;
                case ResultOfAttack.ShotAlready:
                    throw new ApplicationException("Error in AI");
                    break;
            }
            if ((_targets.Count == 0))
            {
                _currentState = AIStates.Searching;
            }

            /*
                Summary:
                ProcessDetroy is able to process the destroyed ships targets and remove _lastHit targets.
                It will also call RemoveShotsAround to remove targets that it was going to shoot at
            */
            // Parameter: row - the row that was shot at and destroyed
            // Parameter: col - the row that was shot at and destroyed
            // Parameter: ship - the row that was shot at and destroyed
        }

        void ProcessDestroy(int row, int col, Ship ship)
        {
            bool foundOriginal;
            Location source;
            Target current;
            current = _currentTarget;
            foundOriginal = false;
            int i;
            for (i = 1; (i
                        <= (ship.Hits - 1)); i++)
            {
                if (!foundOriginal)
                {
                    source = current.Source;

                    /*  
                        Source is nothing if the ship was originally hit in
                        the middle. This then searched forward, rather than
                        backward through the list of targets
                    */

                    if ((source == null))
                    {
                        source = current.ShotAt;
                        foundOriginal = true;
                    }

                }
                else
                {
                    source = current.ShotAt;
                }

                // find the source in _lastHit
                foreach (Target t in _lastHit)
                {
                    if (((!foundOriginal
                                && (t.ShotAt == source))
                                || (foundOriginal
                                && (t.Source == source))))
                    {
                        current = t;
                        _lastHit.Remove(t);
                        break;
                    }

                }

                this.RemoveShotsAround(current.ShotAt);
            }

        }

        /*
            Summary:
            RemoveShotsAround will remove targets that belong to the destroyed ship by checking if 
            the source of the targets belong to the destroyed ship. If they don't put them on a new stack.
            Then clear the targets stack and move all the targets that still need to be shot at back 
            onto the targets stack
        */
        // Parameter: toRemove - 

        private void RemoveShotsAround(Location toRemove)
        {
            Stack<Target> newStack = new Stack<Target>();
            // create a new stack
            // check all targets in the _targets stack

            foreach (Target t in _targets)
            {

                // if the source of the target does not belong to the destroyed ship put them on the newStack

                if (t.Source)
                {
                    IsNot;
                    toRemove;
                    newStack.Push(t);
                    _targets.Clear();

                    // clear the _targets stack
                    // for all the targets in the newStack, move them back onto the _targets stack

                    foreach (Target t in newStack)
                    {
                        _targets.Push(t);
                    }

                    // if the _targets stack is 0 then change the AI's state back to searching

                    if ((_targets.Count == 0))
                    {
                        _currentState = AIStates.Searching;
                    }

                    /*
                        Summary:
                        ProcessHit gets the last hit location coordinates and will ask AddTarget to
                        create targets around that location by calling the method four times each time with
                        a new location around the last hit location.
                        It will then set the state of the AI and if it's not Searching or targetingShip then 
                        start ReOrderTargets.
                    */
                    // Parameter: row - 
                    // Parameter: col - 

                    ProcessHit(((int)(row)), ((int)(col)));
                    _lastHit.Add(_currentTarget);

                    // Uses _currentTarget as the source

                    AddTarget((row - 1), col);
                    AddTarget(row, (col - 1));
                    AddTarget((row + 1), col);
                    AddTarget(row, (col + 1));
                    if ((_currentState == AIStates.Searching))
                    {
                        _currentState = AIStates.TargetingShip;
                    }
                    else
                    {

                        // either targetting or hitting... both are the same here

                        _currentState = AIStates.HittingShip;
                        ReOrderTargets();
                    }

                }

                /* 
                    Summary:
                    ReOrderTargets will optimise the targeting by re-orderin the stack that the targets are in.
                    By putting the most important targets at the top they are the ones that will be shot at first.
                */

                ReOrderTargets();

                // if the ship is lying on the same row, call MoveToTopOfStack to optimise on the row

                if (_currentTarget.SameRow)
                {
                    MoveToTopOfStack(_currentTarget.ShotAt.Row, -1);
                }
                else if (_currentTarget.SameColumn)
                {

                    // else if the ship is lying on the same column, call MoveToTopOfStack to optimise on the column

                    MoveToTopOfStack(-1, _currentTarget.ShotAt.Column);
                }

                /*
                    Summary:
                    MoveToTopOfStack will re-order the stack by checkin the coordinates of each target
                    If they have the right column or row values it will be moved to the _match stack else 
                    put it on the _noMatch stack. Then move all the targets from the _noMatch stack back on the 
                    _targets stack, these will be at the bottom making them less important. The move all the
                    targets from the _match stack on the _targets stack, these will be on the top and will there
                    for be shot at first
                */
                // Parameter: row - the row of the optimisation
                // Parameter: column - the column of the optimisation

                MoveToTopOfStack(((int)(row)), ((int)(column)));
                Stack<Target> _noMatch = new Stack<Target>();
                Stack<Target> _match = new Stack<Target>();
                Target current;
                while ((_targets.Count > 0))
                {
                    current = _targets.Pop();
                    if (((current.ShotAt.Row == row)
                                || (current.ShotAt.Column == column)))
                    {
                        _match.Push(current);
                    }
                    else
                    {
                        _noMatch.Push(current);
                    }

                }

                foreach (Target t in _noMatch)
                {
                    _targets.Push(t);
                }

                foreach (Target t in _match)
                {
                    _targets.Push(t);
                }

                // Summary: AddTarget will add the targets it will shoot onto a stack
                // Parameter: row - the row of the targets location
                // Parameter: column - the column of the targets location

                AddTarget(((int)(row)), ((int)(column)));
                if (((row >= 0)
                            && ((column >= 0)
                            && ((row < EnemyGrid.Height)
                            && ((column < EnemyGrid.Width)
                            && (EnemyGrid.Item[row, column] == TileView.Sea))))))
                {
                    _targets.Push(new Target(new Location(row, column), _currentTarget.ShotAt));
                }

            }

        }
    }
}



