using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Visualize.UI;

public class Button : IUiElement
{
    private readonly Action _onClick;
    private readonly TextLabel _text;

    public Button(string text, Vector2 pos, Action onClick)
    {
        _onClick = onClick;
        _text = new TextLabel(){Value = $"<{text}>", Position = pos};
        KeyboardInput.KeyPressed += KeyPressed;
    }
    
    private void KeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
    {
        if (!IsActive)
            return;
        if (e.KeyCode == Keys.Enter)
            _onClick();
    }

    public bool IsActive
    {
        get => _text.IsActive;
        set => _text.IsActive = value;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        _text.Draw(spriteBatch);
    }
}