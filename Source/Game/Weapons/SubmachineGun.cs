﻿using SDL2;
using SdlTest.Entities;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Weapons
{
    public class SubmachineGun : Weapon
    {
        public override WeaponType WeaponType => WeaponType.SubmachineGun;
        public override string Name => "Submachine gun";

        private readonly Sprite sprite;

        public SubmachineGun() : base(50, 100, 1900)
        {
            sprite = Services.Sprites["submachinegun"];
            AutomaticFire = true;
        }

        protected override void FireInternal(uint time, Entity source, Vector location, Vector vector)
        {
            var angle = vector.AngleInDegrees + Services.Random.Next(-2, 2);
            var projectileVector = Vector.FromAngleInDegrees(angle);
            var sourceLocation = location + vector.ToUnit() * sprite.Width;
            var projectile = new Projectile(source, sourceLocation, projectileVector * 40, 1, 800);
            Services.EntityManager.Add(projectile);
        }

        public override void Render(IntPtr rendererId, Point location, Vector vector)
        {
            var angle = vector.AngleInDegrees;
            var center = new Vector(0, sprite.Height / 2);
            var flip = angle > -90 && angle < 90 ? SDL.SDL_RendererFlip.SDL_FLIP_NONE : SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;

            sprite.DrawEx(rendererId, location, vector.AngleInDegrees, center, flip);
        }
    }
}