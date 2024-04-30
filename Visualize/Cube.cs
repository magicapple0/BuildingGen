using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualize
{
    public class Cube
    {
        Vector3 position;
        Game1 game;
        BasicEffect effect;
        Quad[] faces;
        private string[] textures;

        public Cube(Game1 game, Vector3 position, string[] textures) 
        {
            this.game = game;
            this.textures = textures;
            effect = new BasicEffect(game.GraphicsDevice);
            this.position = position;
            InitializeEffect();
            InitializeFaces();
        }

        private void InitializeFaces()
        {
            faces = new Quad[6];
            faces[0] = new Quad(game, textures[1],
                new [] { position + new Vector3(0, 1, 1), position + new Vector3(1, 1, 1), position + new Vector3(1, 0, 1), position + new Vector3(0, 0, 1) }); //front
            faces[1] = new Quad(game, textures[4],
                new [] { position + new Vector3(1, 1, 1), position + new Vector3(1, 1, 0), position + new Vector3(1, 0, 0), position + new Vector3(1, 0, 1) }); //right
            faces[2] = new Quad(game, textures[3],
                new [] { position + new Vector3(0, 1, 0), position + new Vector3(0, 1, 1), position + new Vector3(0, 0, 1), position + new Vector3(0, 0, 0) }); //left
            faces[3] = new Quad(game, textures[5],
                new [] { position + new Vector3(1, 1, 0), position + new Vector3(0, 1, 0), position + new Vector3(0, 0, 0), position + new Vector3(1, 0, 0) }); //back
            faces[4] = new Quad(game, textures[0],
                new [] { position + new Vector3(0, 1, 1), position + new Vector3(0, 1, 0), position + new Vector3(1, 1, 0), position + new Vector3(1, 1, 1) }); //up
            faces[5] = new Quad(game, textures[2],
                new [] { position + new Vector3(1, 0, 0), position + new Vector3(0, 0, 0), position + new Vector3(0, 0, 1), position + new Vector3(1, 0, 1) }); //down
        }

        public void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.VertexColorEnabled = true;
        }
        
        public void Draw()
        {
            effect.World = game.WorldMatrix;
            effect.View = game.ViewMatrix;
            effect.Projection = game.ProjectionMatrix;

            foreach (var face in faces)
            {
                face.Draw();
            }
        }
    }
}
