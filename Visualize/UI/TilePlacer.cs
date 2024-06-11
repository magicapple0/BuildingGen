using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Visualize.UI;

public class TilePlacer : IUiElement
{
    private readonly Core _core;
    private readonly World _world;
    private readonly Vector2 _pos;
    private List<IUiElement> _elements = new();
    private bool _isActive;
    private int curTileInfoId;
    private readonly TextLabel tileTypeLabel;
    private readonly TextLabel tilePosLabel;

    public TilePlacer(Core core, World world, Vector2 pos)
    {
        _core = core;
        _world = world;
        _pos = pos;
        tileTypeLabel = new TextLabel()
        {
            Position = pos + new Vector2(0, 0)
        };
        tilePosLabel = new TextLabel()
        {
            Position = pos + new Vector2(0, 25)
        };
        UpdateLabels();
        _elements.Add(tileTypeLabel);
        _elements.Add(tilePosLabel);
        KeyboardInput.KeyPressed += KeyPressed;
    }

    private void UpdateLabels()
    {
        tileTypeLabel.Value = $"Tile: {_core.Tileset[curTileInfoId].Name}";
        tilePosLabel.Value =
            $"X:{_world.ActiveTilePosition.X} Y:{_world.ActiveTilePosition.Y} Z:{_world.ActiveTilePosition.Z}";
    }

    private void KeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
    {
        if (!IsActive)
            return;
        switch (e.KeyCode)
        {
            case Keys.NumPad7:
                curTileInfoId = (_core.Tileset.Length + curTileInfoId - 1) % _core.Tileset.Length;
                _world.ActiveTileType = _core.Tileset[curTileInfoId];
                break;
            case Keys.NumPad9:
                curTileInfoId = (curTileInfoId + 1) % _core.Tileset.Length;
                _world.ActiveTileType = _core.Tileset[curTileInfoId];
                break;
            case Keys.NumPad8:
                _world.ActiveTilePosition += new Vector3(1, 0, 0);
                break;
            case Keys.NumPad4:
                if (_world.ActiveTilePosition.Z > 0 )
                    _world.ActiveTilePosition += new Vector3(0, 0, -1);
                break;
            case Keys.NumPad5:
                if (_world.ActiveTilePosition.X > 0 )
                    _world.ActiveTilePosition += new Vector3(-1, 0, 0);
                break;
            case Keys.NumPad6:
                _world.ActiveTilePosition += new Vector3(0, 0, 1);
                break;
            case Keys.NumPad1:
                if (_world.ActiveTilePosition.Y > 0 )
                    _world.ActiveTilePosition += new Vector3(0, -1, 0);
                break;
            case Keys.NumPad3:
                _world.ActiveTilePosition += new Vector3(0, 1, 0);
                break;
            case Keys.Enter:
                _world.PlaceActiveTile();
                break;
        }

        UpdateLabels();
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _elements.ForEach(x => x.IsActive = value);
            _isActive = value;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _elements.ForEach(x => x.Draw(spriteBatch));
    }
}