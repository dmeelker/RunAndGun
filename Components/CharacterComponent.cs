using SDL2;
using SdlTest.Entities;
using SdlTest.Sprites;
using SdlTest.Types;
using System;

namespace SdlTest.Components
{
    public class CharacterComponent
    {
        private readonly Entity entity;
        private Sprite sprite;
        private Sprite gunSprite;

        public Vector AimVector;
        public Vector WeaponOffset;
        public Vector WeaponLength;
        public Direction Direction = Direction.Right;

        public CharacterComponent(Entity entity, Sprite textureId, Sprite gunTexureId)
        {
            this.entity = entity;
            this.sprite = textureId;
            this.gunSprite = gunTexureId;

            WeaponLength = new Vector(30, 0);
        }

        public void Update(int ticksPassed)
        {
            
        }

        public void AimAt(int x, int y)
        {
            var vector = (new Vector(x - 4, y - 4) - entity.Location).ToUnit();
            var angle = vector.Angle;

            Direction = angle > -90 && angle < 90 ? Direction.Right : Direction.Right;
            WeaponOffset = Direction == Direction.Right ? new Vector(10, 12) : new Vector(entity.Size.X - 10, 12);

            AimVector = (new Vector(x - 4, y - 4) - entity.Location - WeaponOffset).ToUnit();
        }

        public void Fire()
        {
            var sourceLocation = entity.Location + WeaponOffset + (AimVector.ToUnit() * WeaponLength.X);
            var projectile = new Projectile(entity, sourceLocation, AimVector * 40);
            Services.EntityManager.Add(projectile);
        }

        public void Render(IntPtr rendererId)
        {
            sprite.Draw(rendererId, (int)entity.Location.X, (int)entity.Location.Y);

            //SDL.SDL_QueryTexture(gunSprite, out _, out _, out var width, out var height);

            //source.w = 30;
            //source.h = 12;

            //destination.x = (int)entity.Location.X + 10;
            //destination.y = (int)entity.Location.Y + 12;
            //destination.w = source.w;
            //destination.h = source.h;



            var angle = AimVector.Angle;
            var center = new Vector(0, gunSprite.Height / 2);

            if (angle > -90 && angle < 90)
            {
                // Aiming right
                gunSprite.DrawEx(rendererId, (int)entity.Location.X + 10, (int)entity.Location.Y + 12, AimVector.Angle, center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                //SDL.SDL_RenderCopyEx(rendererId, gunSprite, ref source, ref destination, AimVector.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
            else
            {
                // Aiming left
                //destination.x = (int)(entity.Location.X + entity.Size.X - 10);
                gunSprite.DrawEx(rendererId, (int) (entity.Location.X + entity.Size.X - 10), (int)entity.Location.Y + 12, AimVector.Angle, center, SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL);

                //SDL.SDL_RenderCopyEx(rendererId, gunSprite, ref source, ref destination, AimVector.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL);
            }
        }
    }
}
