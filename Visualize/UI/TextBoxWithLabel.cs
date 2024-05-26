using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualize.UI;

public class TextBoxWithLabel : IUiElement
{
    private readonly TextLabel _label;
    private readonly TextInput _input;

    public TextBoxWithLabel(string label, Vector2 position)
    {
        _label = new TextLabel{Value = label + ": ", Position = position};
        _input = new TextInput(new Vector2(position.X + label.Length * 14, position.Y));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _label.Draw(spriteBatch);
        _input.Draw(spriteBatch);
    }

    public bool IsActive
    {
        get => _label.IsActive;
        set
        {
            _label.IsActive = value;
            _input.IsActive = value;
        }
    }
}