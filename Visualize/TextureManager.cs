using BuildingGen;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = System.Drawing.Color;

namespace Visualize;

public class TextureManager
{
    private readonly Game1 _game;
    public TextureManager(Game1 game)
    {
        _game = game;
    }

    public Texture2D[] GetTexture(Tile tile)
    {
        var textures = new Texture2D[6];

        if (tile.ModifiedTextures == null)
        {
            var colorTexture = GenerateTexture(tile.TileInfo.Color);
            using var stream = TitleContainer.OpenStream(colorTexture);
            for (int i = 0; i < 6; i++)
                textures[i] = Texture2D.FromStream(_game.GraphicsDevice, stream);
            return textures;
        }
            
        for (var i = 0; i < 6; i++)
        {
            using var stream = TitleContainer.OpenStream(tile.ModifiedTextures[i]);
            textures[i] = Texture2D.FromStream(_game.GraphicsDevice, stream);
        }
        return textures;
    }

    private static string GenerateTexture(string hexColor)
    {
        var texturePath = hexColor + ".png"; 
        #pragma warning disable CA1416
        using var b = new Bitmap(50, 50);
        using (Graphics g = Graphics.FromImage(b)) 
            g.Clear(Color.FromArgb(int.Parse("FF" + hexColor,System.Globalization.NumberStyles.HexNumber)));
        b.Save(texturePath, ImageFormat.Png);
        return texturePath;
    }
}