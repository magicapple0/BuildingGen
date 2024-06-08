using System;
using System.Collections.Generic;
using System.Linq;
using BuildingGen;
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
    private Cube _activeCube;
    private Border _activeCubeBorder;
    private TileInfo _activeTileType;
    private BuildingGen.Vector3 _activeTilePosition;
    private int _activeTileRotation;

    public BuildingGen.Vector3 ActiveTilePosition
    {
        get => _activeTilePosition;
        set
        {
            _activeTilePosition = value;
            var pos = new Vector3(_activeTilePosition.X, _activeTilePosition.Y, _activeTilePosition.Z);
            _activeCubeBorder = new Border(_core, pos, new Vector3(1));
            _activeCube = new Cube(_core, pos,  _textureManager.GetTexture( new Tile(_activeTileType)));
        }
    }

    public TileInfo ActiveTileType
    {
        get => _activeTileType;
        set
        {
            _activeTileType = value;
            var pos = new Vector3(_activeTilePosition.X, _activeTilePosition.Y, _activeTilePosition.Z);
            _activeCubeBorder = new Border(_core, pos, new Vector3(1));
            _activeCube = new Cube(_core, pos,  _textureManager.GetTexture( new Tile(_activeTileType)));
        }
    }

    public int ActiveTileRotation
    {
        get => _activeTileRotation;
        set => _activeTileRotation = (value + 4) % 4;
    }


    public World(Core core)
    {
        _core = core;
        _textureManager = new TextureManager(core);
        Max = new Vector3(0, 0, 0);
        _border = new Border(core, Max);
        ClearActiveTile();
    }

    public void PlaceActiveTile()
    {
        var tile = new Tile(ActiveTileType);
        _tiles[new BuildingGen.Vector3(_activeTilePosition.X, _activeTilePosition.Z, _activeTilePosition.Y)] = tile;
        LoadTiles(_tiles);
    }

    public void ClearActiveTile()
    {
        _activeTileType = new TileInfo("air", null, null);
        _activeTilePosition = new BuildingGen.Vector3(0, 0, 0);
        _activeCube = null;
        _activeCubeBorder = null;
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
        _cubes = newCubes;
    }
    
    public void Draw()
    {
        foreach (var cube in _cubes.Where(x => _activeCube == null || x.Position != _activeCube.Position))
            cube.Draw();
        if (_activeCube != null)
        {
            _activeCube.Draw();
            _activeCubeBorder.Draw();
        }
        _border.Draw();
    }
}