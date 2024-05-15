namespace BuildingGen;

public static class DirectionConstants
{
    public static readonly Dictionary<Directions, Vector3> DirectionsVectors = new()
    {
        { Directions.Face, (0, 1, 0) },
        { Directions.Back, (0, -1, 0) },
        { Directions.Right, (1, 0, 0) },
        { Directions.Left, (-1, 0, 0) },
        { Directions.Up, (0, 0, 1) },
        { Directions.Down, (0, 0, -1) },
    };
    public static readonly Dictionary<Directions, (int, int)> CellOppositeSides = new()
    {
        { Directions.Face, (5, 1) },
        { Directions.Back, (1, 5) },
        { Directions.Right, (3, 4) },
        { Directions.Left, (4, 3) },
        { Directions.Up, (2, 0) },
        { Directions.Down, (0, 2) },
    };
}

public enum Directions
{
    Up, Down, Left, Right, Face, Back
}