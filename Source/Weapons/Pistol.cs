using SDL2;
using SdlTest.Entities;
using SdlTest.Sprites;
using SdlTest.Types;
using System;

namespace SdlTest.Weapons
{
    public class Pistol : Weapon
    {
        public override WeaponType WeaponType => WeaponType.Pistol;
        public override string Name => "Pistol";

        private readonly Sprite sprite;

        public Pistol() : base(12, 100, 1500)
        {
            sprite = Services.SpriteManager["shotgun"];
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            var sourceLocation = location + vector.ToUnit() * sprite.Width;
            var projectile = new Projectile(source, sourceLocation, vector * 40, 2, 800);
            Services.EntityManager.Add(projectile);
        }

        public override void Render(IntPtr rendererId, Vector location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            sprite.DrawEx(rendererId, (int)location.X, (int)location.Y, vector.AngleInDegrees, center, flip);
        }
    }
}
