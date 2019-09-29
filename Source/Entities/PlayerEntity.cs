﻿using SDL2;
using SdlTest.Components;
using SdlTest.Entities.Collectables;
using SdlTest.Levels;
using SdlTest.Types;
using SdlTest.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class PlayerEntity : Entity
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;

        public readonly WeaponType[] WeaponOrder = new WeaponType[] { WeaponType.Pistol, WeaponType.Shotgun };
        private readonly Dictionary<WeaponType, Weapon> weapons = new Dictionary<WeaponType, Weapon>();

        public PlayerEntity(Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, Services.SpriteManager["player"], new Pistol());

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);
            Character.Update(time, ticksPassed);
        }

        public void AimAt(int x, int y)
        {
            Character.AimAt(x, y);
        }

        public void Fire(uint time)
        {
            Character.Fire(time);
        }

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }

        public void ChangeWeapon(WeaponType weaponType)
        {
            if (weapons.ContainsKey(weaponType))
                Character.Weapon = weapons[weaponType];
        }

        public void AddWeapon(Weapon weapon)
        {
            if (weapons.TryGetValue(weapon.WeaponType, out var existingWeapon))
            {
                existingWeapon.AddAmmo(weapon.ClipContent + weapon.AmmoReserve);
            }
            else
            {
                weapons.Add(weapon.WeaponType, weapon);
            }
        }

        internal void Collect(ICollectable collectable)
        {
            switch(collectable)
            {
                case WeaponCollectable weaponCollectable:
                    AddWeapon(weaponCollectable.CreateWeapon());
                    break;
            }
        }
    }
}