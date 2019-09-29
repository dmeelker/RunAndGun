﻿using SDL2;
using SdlTest.Entities;
using SdlTest.Sprites;
using SdlTest.Types;
using System;

namespace SdlTest.Weapons
{
    public class Shotgun : Weapon
    {
        public override WeaponType WeaponType => WeaponType.Shotgun;
        public override string Name => "Shotgun";
        private readonly Sprite sprite;

        public Shotgun() : base(8, 400, 3000)
        {
            sprite = Services.SpriteManager["shotgun"];
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            for (var i = 0; i < 10; i++)
            {
                var angle = vector.AngleInDegrees + Services.Random.Next(-5, 5);
                var projectileVector = Vector.FromAngleInDegrees(angle) * Services.Random.Next(35, 40);
                var sourceLocation = location + vector.ToUnit() * sprite.Width;

                var projectile = new Projectile(source, sourceLocation, projectileVector, 1, 300);
                Services.EntityManager.Add(projectile);
            }
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