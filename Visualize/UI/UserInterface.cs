using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BuildingGen;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Visualize.UI;

public class UserInterface
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Core _game;
    private readonly Camera _camera;
    private readonly World _world;
    private bool _isActive = true;
    private List<IUiElement> _elements = new ();
    private List<IUiElement> _staticElements = new ();
    private int _curActive = 0;
    private TextLabel _errorLabel;
    private TextBoxWithLabel _fileInput;

    public UserInterface(SpriteBatch spriteBatch, Core game, Camera camera, World world)
    {
        _spriteBatch = spriteBatch;
        _game = game;
        _camera = camera;
        _world = world;
        LoadElements();
        _elements[_curActive].IsActive = true;
        KeyboardInput.KeyPressed += KeyPressed;
    }

    private void LoadElements()
    {
        _staticElements.Add(new TextLabel(){Value = "UI Active", Position = new Vector2()});
        _errorLabel = new TextLabel() { Position = new Vector2(0, 150) };
        _staticElements.Add(_errorLabel);
        
        _elements.Add(new TilePlacer(_game, _world, new Vector2(0, 25)));
        _fileInput = new TextBoxWithLabel("File", new Vector2(0, 75));
        _elements.Add(_fileInput);
        _elements.Add(new Button("Load", new Vector2(0, 100), LoadFile));
        _elements.Add(new Button("Save", new Vector2(75, 100), SaveFile));
        _elements.Add(new Button("Update TileSet", new Vector2(0, 125), UpdateTileSet));
        
        if (!Directory.Exists("saves"))
            Directory.CreateDirectory("saves");
    }

    private void UpdateTileSet()
    {
        try
        {
            _game.LoadTileSet();
            _errorLabel.Value = "Updated";
        }
        catch (Exception e)
        {
            _errorLabel.Value = "Error: " + e.GetType();
        }
    }

    private void SaveFile()
    {
        var path = $"saves/{_fileInput.Value}.json";
        try
        {
            var fieldJson = JsonSerializer.Serialize(_world.Tiles.ToArray());
            File.WriteAllText(path, fieldJson);
            _errorLabel.Value = "Saved";
        }
        catch (Exception e)
        {
            _errorLabel.Value = "Error: " + e.GetType();
        }
    }

    private void LoadFile()
    {
        var path = $"saves/{_fileInput.Value}.json";
        try
        {
            _errorLabel.Value = "Loading...";
            var fieldJson = File.ReadAllText(path);
            var tiles = JsonSerializer.Deserialize<KeyValuePair<Vector3, Tile>[]>(fieldJson)
                .ToDictionary(x => x.Key, x => x.Value);
            _world.LoadTiles(tiles);
            _errorLabel.Value = "Loaded";
        }
        catch (Exception e)
        {
            _errorLabel.Value = "Error: " + e.GetType();
        }
    }

    private void KeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
    {
        if (e.KeyCode == Keys.LeftAlt)
        {
            _isActive = !_isActive;
            //_camera.IsActive = !_isActive;
            if (_isActive)
            {
                _curActive = 0;
                _elements[_curActive].IsActive = true;
            }
            else
            {
                _elements.ForEach(x => x.IsActive = false);
            }
        }

        if (e.KeyCode == Keys.Tab)
        {
            _elements[_curActive].IsActive = false;
            if (ks.IsKeyDown(Keys.LeftShift))
                _curActive = (_curActive + _elements.Count - 1) % _elements.Count;
            else
                _curActive = (_curActive + _elements.Count + 1) % _elements.Count;
            _elements[_curActive].IsActive = true;
        }
    }

    public void Update()
    {
    }

    public void Draw()
    {
        if (!_isActive)
            return;
        var graphicsDevice = _game.GraphicsDevice;
        var samplerState = graphicsDevice.SamplerStates[0];
        var depthStencilState = graphicsDevice.DepthStencilState;
        _spriteBatch.Begin();
        
        _staticElements.ForEach(x => x.Draw(_spriteBatch));
        _elements.ForEach(x => x.Draw(_spriteBatch));
        
        _spriteBatch.End();
        graphicsDevice.SamplerStates[0] = samplerState;
        graphicsDevice.DepthStencilState = depthStencilState;
    }
}