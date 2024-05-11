using BuildingGen;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        if (tile.Modifiers.Contains(TileModifiers.FlipX) || tile.Modifiers.Contains(TileModifiers.FlipY))
        {
            var newTexturePath = tile.ModifiedTextures[0][..^4];
            newTexturePath = tile.Modifiers.Where(tileModifier => tileModifier == TileModifiers.FlipX || tileModifier == TileModifiers.FlipY)
                .Aggregate(newTexturePath, (current, tileModifier) => current + tileModifier) + ".png";
            if (File.Exists(newTexturePath))
            {
                for (var i = 0; i < 6; i++)
                {
                    using var stream2 = TitleContainer.OpenStream(newTexturePath);
                    textures[i] = Texture2D.FromStream(_game.GraphicsDevice, stream2);
                }
                return textures;
            }

            for (var i = 0; i < 6; i++)
            {
                var modifiedTexturePath = tile.ModifiedTextures[i];
                foreach (var tileModifier in tile.Modifiers.Where(tileModifier => tileModifier == TileModifiers.FlipX || tileModifier == TileModifiers.FlipY))
                    modifiedTexturePath = FlipTexture(modifiedTexturePath, tileModifier);
                using var stream2 = TitleContainer.OpenStream(modifiedTexturePath);
                textures[i] = Texture2D.FromStream(_game.GraphicsDevice, stream2);
            }
            return textures;
        }
        
        for (var i = 0; i < 6; i++)
        {
            using var stream = TitleContainer.OpenStream(tile.ModifiedTextures[i]);
            textures[i] = Texture2D.FromStream(_game.GraphicsDevice, stream);
        }
        return textures;
    }

    private static string FlipTexture(string texturePath, TileModifiers tileModifier)
    {
        if (tileModifier is TileModifiers.FlipX or TileModifiers.FlipY)
        {
            var newTexturePath = texturePath[..^4] + "_flip"  + ".png";
            var b = (Bitmap)Bitmap.FromFile(texturePath);
            b.RotateFlip(RotateFlipType.Rotate180FlipY);
            b.Save(newTexturePath, ImageFormat.Png);
            return newTexturePath;
        }
        return texturePath;
    }

    private static string GenerateTexture(string hexColor)
    {
        var texturePath = hexColor + ".png";
        if (File.Exists(texturePath))
            return texturePath;
        #pragma warning disable CA1416
        using var b = new Bitmap(50, 50);
        using (Graphics g = Graphics.FromImage(b)) 
            g.Clear(Color.FromArgb(int.Parse("FF" + hexColor,System.Globalization.NumberStyles.HexNumber)));
        b.Save(texturePath, ImageFormat.Png);
        return texturePath;
    }
}