using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualize.UI;

public class TextLabel : IUiElement
{
    public string Value { get; set; } = "";
    public Vector2 Position { get; set; }

    public bool IsActive { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Core.FontSystem.GetFont(20), Value, Position, IsActive ? Color.Red : Color.Black);
    }
}