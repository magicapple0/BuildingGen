using System;
using System.Collections.Generic;
using System.Linq;
using BuildingGen;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Visualize;

public class World
{
    public Dictionary<BuildingGen.Vector3, Tile> Tiles { get; private set; }
    private readonly TextureManager _textureManager;
    private readonly Core _core;
    public Vector3 Max;
    private Border _border;
    private Dictionary<Vector3, Cube> _cubes = new();
    private Cube _activeCube;
    private Border _activeCubeBorder;
    private TileInfo _activeTileType;
    private Vector3 _activeTilePosition;
    private int _activeTileRotation;
    private readonly object _activeLock = new ();

    public Vector3 ActiveTilePosition
    {
        get => _activeTilePosition;
        set
        {
            lock (_activeLock)
            {
                _activeTilePosition = value;
                var pos = new Vector3(_activeTilePosition.X, _activeTilePosition.Y, _activeTilePosition.Z);
                if (_activeCube != null)
                {
                    _activeCube.Dispose();
                    _activeCubeBorder.Dispose();
                }
                _activeCubeBorder = new Border(_core, pos, new Vector3(1));
                _activeCube = new Cube(_core, pos,  _textureManager.GetTexture( new Tile(_activeTileType)));   
            }
        }
    }

    public TileInfo ActiveTileType
    {
        get => _activeTileType;
        set
        {
            lock (_activeLock)
            {
                _activeTileType = value;
                var pos = new Vector3(_activeTilePosition.X, _activeTilePosition.Y, _activeTilePosition.Z);
                if (_activeCube != null)
                {
                    _activeCube.Dispose();
                    _activeCubeBorder.Dispose();
                }
                _activeCubeBorder = new Border(_core, pos, new Vector3(1));
                _activeCube = new Cube(_core, pos,  _textureManager.GetTexture( new Tile(_activeTileType)));   
            }
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
        var pos = new BuildingGen.Vector3((int)_activeTilePosition.X, (int)_activeTilePosition.Z, (int)_activeTilePosition.Y);
        lock (Tiles)
        {
            if (_activeTileType.Name == "air")
            {
                Tiles.Remove(pos);
            }
            else
            {
                var tile = new Tile(ActiveTileType);
                Tiles[pos] = tile;   
            }    
        }
        

        lock (_cubes)
        {
            if (_cubes.TryGetValue(_activeTilePosition, out var oldCube))
            {
                oldCube.Dispose();
            }
            lock(_activeLock)
            {
                _cubes[_activeTilePosition] = _activeCube;
                _activeCube = new Cube(_core, _activeCube.Position,
                    _textureManager.GetTexture(new Tile(_activeTileType)));
            }
        }
        //LoadTiles(Tiles);
    }

    public void ClearActiveTile()
    {
        lock (_activeLock)
        {
            _activeTileType = new TileInfo("air", null, null);
            _activeTilePosition = new Vector3(0, 0, 0);
            _activeCube = null;
            _activeCubeBorder = null;   
        }
    }

    public void LoadTiles(Dictionary<BuildingGen.Vector3, Tile> tiles)
    {
        Tiles = tiles;
        Max = new Vector3();
        var newCubes = new Dictionary<Vector3, Cube>();

        foreach (var tile in Tiles)
        {
            Max.X = Math.Max(Max.X, tile.Key.X + 1);
            Max.Y = Math.Max(Max.Y, tile.Key.Y + 1);
            Max.Z = Math.Max(Max.Z, tile.Key.Z + 1);
            if (tile.Value.TileInfo.Name == "air")
                continue;
            var pos = new Vector3(tile.Key.X, tile.Key.Z, tile.Key.Y);
            newCubes[pos] = (new Cube(_core, pos, _textureManager.GetTexture(tile.Value)));
        }
        lock (_cubes)
        {
            foreach (var cube in _cubes.Values)
            {
                cube.Dispose();
            }
            _cubes = newCubes;
        }
    }
    
    public void Draw()
    {
        lock (_cubes)
        {
            foreach (var cube in _cubes.Where(x => _activeCube == null || x.Value.Position != _activeCube.Position))
                cube.Value.Draw();
        }
        if (_activeCube != null)
        {
            lock (_activeLock)
            {
                _activeCube.Draw();
                _activeCubeBorder.Draw();
            }
        }
        //_border.Draw();
    }
}