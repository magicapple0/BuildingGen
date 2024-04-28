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

        public Cube(Game1 game, Vector3 position) 
        {
            this.game = game;
            effect = new BasicEffect(game.GraphicsDevice);
            this.position = position;
            InitializeEffect();
            InitializeFaces();
        }

        private void InitializeFaces()
        {
            faces = new Quad[6];
            faces[0] = new Quad(game, "wall.png",
                new [] { position + new Vector3(0, 1, 1), position + new Vector3(1, 1, 1), position + new Vector3(1, 0, 1), position + new Vector3(0, 0, 1) }); //front
            faces[1] = new Quad(game, "wall.png",
                new [] { position + new Vector3(1, 1, 1), position + new Vector3(1, 1, 0), position + new Vector3(1, 0, 0), position + new Vector3(1, 0, 1) }); //right
            faces[2] = new Quad(game, "wall.png",
                new [] { position + new Vector3(0, 1, 0), position + new Vector3(0, 1, 1), position + new Vector3(0, 0, 1), position + new Vector3(0, 0, 0) }); //left
            faces[3] = new Quad(game, "wall.png",
                new [] { position + new Vector3(1, 1, 0), position + new Vector3(0, 1, 0), position + new Vector3(0, 0, 0), position + new Vector3(1, 0, 0) }); //back
            faces[4] = new Quad(game, "roof.png",
                new [] { position + new Vector3(0, 1, 1), position + new Vector3(0, 1, 0), position + new Vector3(1, 1, 0), position + new Vector3(1, 1, 1) }); //up
            faces[5] = new Quad(game, "bottom.png",
                new [] { position + new Vector3(1, 0, 0), position + new Vector3(0, 0, 0), position + new Vector3(0, 0, 1), position + new Vector3(1, 0, 1) }); //down
        }

        public void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.VertexColorEnabled = true;
        }
        
        public void Draw()
        {
            effect.World = game.worldMatrix;
            effect.View = game.viewMatrix;
            effect.Projection = game.projectionMatrix;

            foreach (var face in faces)
            {
                face.Draw();
            }
        }
    }
}
