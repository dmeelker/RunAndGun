using SDL2;
using SdlTest.Entities;
using SdlTest.Sprites;
using SdlTest.Types;
using System;

namespace SdlTest.Weapons
{
    public class SniperRifle : Weapon
    {
        public override WeaponType WeaponType => WeaponType.SniperRifle;
        public override string Name => "Sniper Rifle";

        private readonly Sprite sprite;

        public SniperRifle() : base(12, 600, 1500)
        {
            sprite = Services.Sprites["sniperrifle"];
            Range = 2000;
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            var sourceLocation = location + vector.ToUnit() * sprite.Width;
            var projectile = new Projectile(source, sourceLocation, vector * 100, 7, Range);
            Services.Game.Entities.Add(projectile);

            CreateCasing(location, vector, Services.Sprites["bulletcasing"]);
        }

        public override void Render(IntPtr rendererId, Point location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            location -= vector.ToUnit() * 8;

            sprite.DrawEx(rendererId, location, vector.AngleInDegrees, center, flip);
        }
    }
}
