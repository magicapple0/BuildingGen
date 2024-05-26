using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Visualize;

public class Camera
{
    private readonly Core _core;
    private readonly World _world;
    private Vector3 _centerOfBuilding;
    private Vector3 _cameraPosition;
    private Vector3 _cameraTarget;

    public bool IsActive = true;

    public Camera(Core core, World world)
    {
        _core = core;
        _world = world;
        SetCameraSettings(new Vector3(0, 0, 0));
    }
    
    
    public void SetCameraSettings(Vector3 min)
    {
        _centerOfBuilding = new Vector3((_world.Max.X - min.X) / 2, (_world.Max.Y - min.Y) / 2, (_world.Max.Z - min.Z) / 2);
        var zoom = MathHelper.Max(_world.Max.X, MathHelper.Max(_world.Max.Y, _world.Max.Z)) * 1.2f + 4;
        _cameraPosition = new Vector3(0, -_centerOfBuilding.Z + 2, zoom);
        _cameraTarget = new Vector3(0, _centerOfBuilding.Z - 2, -zoom);
        _core.ViewMatrix = Matrix.CreateLookAt(_cameraPosition, new Vector3(0, _centerOfBuilding.Z - 0.7f, -zoom),
            Vector3.Up);
        _core.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            (float)_core.Window.ClientBounds.Width / _core.Window.ClientBounds.Height, 1, 100);
        _core.WorldMatrix =
            Matrix.CreateWorld(
                new Vector3(-_centerOfBuilding.X - 0.5f, -_centerOfBuilding.Z + 0.5f, -_centerOfBuilding.Y - 0.5f),
                new Vector3(0, 0, -1), Vector3.Up);
    }

    public void Update()
    {
        if (!IsActive)
            return;
        
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _core.WorldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _core.WorldMatrix *= Matrix.CreateRotationX(-1 * MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _core.WorldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _core.WorldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _core.WorldMatrix =
                Matrix.CreateWorld(
                    new Vector3(-_centerOfBuilding.X - 0.5f, -_centerOfBuilding.Z + 0.5f,
                        -_centerOfBuilding.Y - 0.5f), new Vector3(0, 0, -1), Vector3.Up);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Z))
        {
            _cameraPosition = new Vector3(_cameraPosition.X, _cameraPosition.Y, _cameraPosition.Z - 0.1f);
            _core.ViewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.X))
        {
            _cameraPosition = new Vector3(_cameraPosition.X, _cameraPosition.Y, _cameraPosition.Z + 0.1f);
            _core.ViewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);
        }
    }
}