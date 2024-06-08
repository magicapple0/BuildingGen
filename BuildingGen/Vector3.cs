namespace BuildingGen;

public record struct Vector3(int X, int Y, int Z)
{
    public static implicit operator Vector3((int, int, int) coord) => new (coord.Item1, coord.Item2, coord.Item3);
    public static implicit operator (int, int, int)(Vector3 coord) => (coord.X, coord.Y, coord.Z);

    public static Vector3 operator +(Vector3 a, Vector3 b) => new (a.X + b.X, a.Y + b.Y, a.Z + b.Z);
}