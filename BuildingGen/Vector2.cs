namespace BuildingGen;

public record struct Vector2(int X, int Y)
{
    public static implicit operator Vector2((int, int) coord) => new (coord.Item1, coord.Item2);
    public static implicit operator Vector2((long, long) coord) => new ((int)(coord.Item1), (int)(coord.Item2));
    public static implicit operator (int, int)(Vector2 coord) => (coord.X, coord.Y);

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, 0);
    }
}