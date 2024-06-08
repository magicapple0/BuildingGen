namespace BuildingGen;

public static class DirectionConstants2
{
    public static readonly Dictionary<Directions2, Vector2> DirectionsVectors = new()
    {
        { Directions2.Face, (0, 1) },
        { Directions2.Back, (0, -1) },
        { Directions2.Right, (1, 0) },
        { Directions2.Left, (-1, 0) }
    };
    
    public static readonly Dictionary<Directions2, int> DirectionsSides = new()
    {
        { Directions2.Face, 0 },
        { Directions2.Back, 3 },
        { Directions2.Right, 2 },
        { Directions2.Left, 1 }
    };
    public static readonly Dictionary<Directions2, (int, int)> CellOppositeSides = new()
    {
        { Directions2.Face, (0, 3) },
        { Directions2.Back, (3, 0) },
        { Directions2.Right, (1, 2) },
        { Directions2.Left, (2, 1) }
    };
}

public enum Directions2
{
    Left, Right, Face, Back
}