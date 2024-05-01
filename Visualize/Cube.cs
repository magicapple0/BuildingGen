using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualize
{
    public class Cube
    {
        private readonly Vector3 _position;
        private readonly Game1 _game;
        private BasicEffect _effect;
        private Quad[] _faces;
        private readonly string[] _textures;

        public Cube(Game1 game, Vector3 position, string[] textures) 
        {
            _game = game;
            _textures = textures;
            _effect = new BasicEffect(game.GraphicsDevice);
            _position = position;
            InitializeEffect();
            InitializeFaces();
        }

        private void InitializeFaces()
        {
            _faces = new Quad[6];
            _faces[0] = new Quad(_game, _textures[1],
                new [] { _position + new Vector3(0, 1, 1), _position + new Vector3(1, 1, 1), _position + new Vector3(1, 0, 1), _position + new Vector3(0, 0, 1) }); //front
            _faces[1] = new Quad(_game, _textures[4],
                new [] { _position + new Vector3(1, 1, 1), _position + new Vector3(1, 1, 0), _position + new Vector3(1, 0, 0), _position + new Vector3(1, 0, 1) }); //right
            _faces[2] = new Quad(_game, _textures[3],
                new [] { _position + new Vector3(0, 1, 0), _position + new Vector3(0, 1, 1), _position + new Vector3(0, 0, 1), _position + new Vector3(0, 0, 0) }); //left
            _faces[3] = new Quad(_game, _textures[5],
                new [] { _position + new Vector3(1, 1, 0), _position + new Vector3(0, 1, 0), _position + new Vector3(0, 0, 0), _position + new Vector3(1, 0, 0) }); //back
            _faces[4] = new Quad(_game, _textures[0],
                new [] { _position + new Vector3(0, 1, 1), _position + new Vector3(0, 1, 0), _position + new Vector3(1, 1, 0), _position + new Vector3(1, 1, 1) }); //up
            _faces[5] = new Quad(_game, _textures[2],
                new [] { _position + new Vector3(1, 0, 0), _position + new Vector3(0, 0, 0), _position + new Vector3(0, 0, 1), _position + new Vector3(1, 0, 1) }); //down
        }

        private void InitializeEffect()
        {
            _effect = new BasicEffect(_game.GraphicsDevice);
            _effect.VertexColorEnabled = true;
        }
        
        public void Draw()
        {
            _effect.World = _game.WorldMatrix;
            _effect.View = _game.ViewMatrix;
            _effect.Projection = _game.ProjectionMatrix;

            foreach (var face in _faces)
            {
                face.Draw();
            }
        }
    }
}
