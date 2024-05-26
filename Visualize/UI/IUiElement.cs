using Microsoft.Xna.Framework.Graphics;

namespace Visualize.UI;

public interface IUiElement
{
    
    bool IsActive { get; set; }
    void Draw(SpriteBatch spriteBatch);
}
