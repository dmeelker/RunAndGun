using SDL2;
using SdlTest.Components;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class PlayerEntity : Entity
    {
        public PhysicsComponent Physics = new PhysicsComponent();
        private IntPtr textureId;
        private IntPtr gunTexureId;
        public Vector AimVector;
        public Vector WeaponOffset;
        public Vector WeaponLength;
        public Direction Direction = Direction.Right;

        public PlayerEntity(IntPtr textureId, IntPtr gunTexureId, Vector location)
        {
            this.textureId = textureId;
            this.gunTexureId = gunTexureId;

            WeaponLength = new Vector(20, 0);

            Physics.Location = location;
            Physics.Size = new Vector(30, 30);
            Physics.Velocity.Y = 0.5;
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);
        }

        public void AimAt(int x, int y)
        {
            var vector = (new Vector(x, y) - Physics.Location).ToUnit();
            var angle = vector.Angle;

            Direction = angle > -90 && angle < 90 ? Direction.Right : Direction.Right;
            WeaponOffset = Direction == Direction.Right ? new Vector(10, 12) : new Vector(Physics.Size.X - 10, 12);

            AimVector = (new Vector(x, y) - Physics.Location - WeaponOffset).ToUnit();
        }

        public void Fire()
        {
            var sourceLocation = Physics.Location + WeaponOffset + (AimVector.ToUnit() * WeaponLength.X);
            var projectile = new Projectile(Services.TextureManager["projectile"], sourceLocation, AimVector * 40);
            Services.EntityManager.Add(projectile);
        }

        public override void Render(IntPtr rendererId)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = 30,
                h = 30
            };

            var destination = new SDL.SDL_Rect()
            {
                x = (int) Physics.Location.X,
                y = (int) Physics.Location.Y,
                w = 30,
                h = 30
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);

            SDL.SDL_QueryTexture(gunTexureId, out _, out _, out var width, out var height);

            source.w = 30;
            source.h = 12;

            destination.x = (int) Physics.Location.X + 10;
            destination.y = (int) Physics.Location.Y + 12;
            destination.w = source.w;
            destination.h = source.h;

            

            var angle = AimVector.Angle;
            var center = new SDL.SDL_Point { x = 0, y = height / 2 };

            if (angle > -90 && angle < 90)
            {
                // Aiming right
                
                SDL.SDL_RenderCopyEx(rendererId, gunTexureId, ref source, ref destination, AimVector.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
            else
            {
                // Aiming left
                //var center = new SDL.SDL_Point { x = source.w, y = 0 };
                destination.x = (int)(Physics.Location.X + Physics.Size.X - 10);
                SDL.SDL_RenderCopyEx(rendererId, gunTexureId, ref source, ref destination, AimVector.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL);
            }
        }

        public void Fire(Vector direction)
        {

        }
    }
}
