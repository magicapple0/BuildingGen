using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Visualize.UI;

public class TextInput : IUiElement
{
    private readonly TextLabel _text;

    public bool IsActive
    {
        get => _text.IsActive;
        set => _text.IsActive = value;
    }

    public TextInput(Vector2 position)
    {
        _text = new TextLabel() { Value = "", Position = position };
        KeyboardInput.CharPressed += CharacterTyped;
        KeyboardInput.KeyPressed += KeyPressed;
    }

    private void CharacterTyped(object sender, KeyboardInput.CharacterEventArgs e, KeyboardState ks)
    {
        if (!IsActive)
            return;
        _text.Value += e.Character;
    }

    private void KeyPressed(object sender, KeyboardInput.KeyEventArgs e, KeyboardState ks)
    {
        if (!IsActive)
            return;
        if (e.KeyCode == Keys.Back && _text.Value.Length > 0)
            _text.Value = _text.Value.Substring(0, _text.Value.Length - 1);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _text.Draw(spriteBatch);
    }
}