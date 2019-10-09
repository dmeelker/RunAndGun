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

        public Pistol() : base(12, 150, 1500)
        {
            sprite = Services.Sprites["pistol"];
            Range = 800;
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            var sourceLocation = location + vector.ToUnit() * sprite.Width;
            var projectile = new Projectile(source, sourceLocation, vector * 40, 2, Range);
            Services.Game.Entities.Add(projectile);

            CreateCasing(location, vector, Services.Sprites["bulletcasing"]);
        }

        public override void Render(IntPtr rendererId, Point location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            if (renderRecoil)
                location -= vector.ToUnit() * 2;

            sprite.DrawEx(rendererId, location, vector.AngleInDegrees, center, flip);
        }
    }
}
