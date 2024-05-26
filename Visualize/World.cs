using System;
using System.Collections.Generic;
using BuildingGen;
using Microsoft.Xna.Framework;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Visualize;

public class World
{
    private Dictionary<BuildingGen.Vector3, Tile> _tiles;
    private readonly TextureManager _textureManager;
    private readonly Core _core;
    public Vector3 Max;
    private Border _border;
    private List<Cube> _cubes = new();
    

    public World(Core core)
    {
        _core = core;
        _textureManager = new TextureManager(core);
        Max = new Vector3(0, 0, 0);
        _border = new Border(core, Max);
    }

    

    public void LoadTiles(Dictionary<BuildingGen.Vector3, Tile> tiles)
    {
        _tiles = tiles;
        var newCubes = new List<Cube>();

        foreach (var tile in _tiles)
        {
            Max.X = Math.Max(Max.X, tile.Key.X + 1);
            Max.Y = Math.Max(Max.Y, tile.Key.Z + 1);
            Max.Z = Math.Max(Max.Z, tile.Key.Y + 1);
            if (tile.Value.TileInfo.Name == "air")
                continue;
            newCubes.Add(new Cube(_core, new Vector3(tile.Key.X, tile.Key.Z, tile.Key.Y), _textureManager.GetTexture(tile.Value)));
        }

        lock (_cubes)
        {
            _cubes = newCubes;
        }
    }
    

    public void Draw()
    {
        foreach (var cube in _cubes)
            cube.Draw();
        _border.Draw();
    }
}