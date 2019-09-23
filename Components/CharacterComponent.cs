using SDL2;
using SdlTest.Entities;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Components
{
    public class CharacterComponent
    {
        private readonly Entity entity;
        private IntPtr textureId;
        private IntPtr gunTexureId;

        public Vector AimVector;
        public Vector WeaponOffset;
        public Vector WeaponLength;
        public Direction Direction = Direction.Right;

        public CharacterComponent(Entity entity, IntPtr textureId, IntPtr gunTexureId)
        {
            this.entity = entity;
            this.textureId = textureId;
            this.gunTexureId = gunTexureId;

            WeaponLength = new Vector(20, 0);
        }

        public void Update(int ticksPassed)
        {
            
        }

        public void AimAt(int x, int y)
        {
            var vector = (new Vector(x, y) - entity.Location).ToUnit();
            var angle = vector.Angle;

            Direction = angle > -90 && angle < 90 ? Direction.Right : Direction.Right;
            WeaponOffset = Direction == Direction.Right ? new Vector(10, 12) : new Vector(entity.Size.X - 10, 12);

            AimVector = (new Vector(x, y) - entity.Location - WeaponOffset).ToUnit();
        }

        public void Fire()
        {
            var sourceLocation = entity.Location + WeaponOffset + (AimVector.ToUnit() * WeaponLength.X);
            var projectile = new Projectile(entity, Services.TextureManager["projectile"], sourceLocation, AimVector * 40);
            Services.EntityManager.Add(projectile);
        }

        public void Render(IntPtr rendererId)
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
                x = (int)entity.Location.X,
                y = (int)entity.Location.Y,
                w = 30,
                h = 30
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);

            SDL.SDL_QueryTexture(gunTexureId, out _, out _, out var width, out var height);

            source.w = 30;
            source.h = 12;

            destination.x = (int)entity.Location.X + 10;
            destination.y = (int)entity.Location.Y + 12;
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
                destination.x = (int)(entity.Location.X + entity.Size.X - 10);
                SDL.SDL_RenderCopyEx(rendererId, gunTexureId, ref source, ref destination, AimVector.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL);
            }
        }
    }
}
