using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Visualize.UI;

public class UserInterface
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Core _game;
    private readonly Camera _camera;
    private readonly World _world;
    private bool isActive = false;
    private List<IUiElement> _elements = new ();
    private List<IUiElement> _staticElements = new ();
    private int curActive = 0;

    public UserInterface(SpriteBatch spriteBatch, Core game, Camera camera, World world)
    {
        _spriteBatch = spriteBatch;
        _game = game;
        _camera = camera;
        _world = world;
        LoadElements();
        KeyboardInput.KeyPressed += KeyPressed;
    }

    private void LoadElements()
    {
        _staticElements.Add(new TextLabel(){Value = "UI Active", Position = new Vector2()});
        
        _elements.Add(new TilePlacer(_game, _world, new Vector2(0, 50)));
        _elements.Add(new TextBoxWithLabel("Aboba", new Vector2(0, 150)));
    }

    private void KeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
    {
        if (e.KeyCode == Keys.LeftAlt)
        {
            isActive = !isActive;
            _camera.IsActive = !isActive;
            if (isActive)
            {
                curActive = 0;
                _elements[curActive].IsActive = true;
            }
            else
            {
                _elements.ForEach(x => x.IsActive = false);
            }
        }

        if (e.KeyCode == Keys.Tab)
        {
            _elements[curActive].IsActive = false;
            if (ks.IsKeyDown(Keys.LeftShift))
                curActive = (curActive + _elements.Count - 1) % _elements.Count;
            else
                curActive = (curActive + _elements.Count + 1) % _elements.Count;
            _elements[curActive].IsActive = true;
        }
    }

    public void Update()
    {
    }

    public void Draw()
    {
        if (!isActive)
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