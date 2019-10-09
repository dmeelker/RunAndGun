using SDL2;
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
    public class PlayerEntity : Entity, IProjectileCollider
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;

        public readonly WeaponType[] WeaponOrder = new WeaponType[] { WeaponType.Pistol, WeaponType.Shotgun, WeaponType.SubmachineGun, WeaponType.SniperRifle };
        private readonly Dictionary<WeaponType, Weapon> weapons = new Dictionary<WeaponType, Weapon>();
        private bool firing = false;
        public bool Moving { get; private set; } = false;

        public PlayerEntity(Vector location)
        {
            Physics = new PhysicsComponent(this) { applyGravity = true };
            Character = new CharacterComponent(this, Services.Sprites["player"], new Pistol());

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(uint time, int ticksPassed)
        {
            if (firing)
                Character.Fire(time);

            Physics.Update(ticksPassed, Services.Game.Level);
            Character.Update(time, ticksPassed);
        }

        public void AimAt(Point point)
        {
            Character.AimAt(point);
        }

        public void BeginFiring(uint time)
        {
            Character.BeginFiring(time);
        }

        public void StopFiring()
        {
            Character.StopFiring();
        }

        public void MoveLeft()
        {
            Physics.Impulse.X = -10;
            Moving = true;
        }

        public void MoveRight()
        {
            Physics.Impulse.X = 10;
            Moving = true;
        }

        public void StopMoving()
        {
            Physics.Impulse.X = 0;
            Moving = false;
        }

        public void Jump()
        {
            Physics.Velocity.Y = -13;
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            Character.Render(rendererId, viewOffset);
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

        public bool Collect(Collectable collectable)
        {
            switch(collectable)
            {
                case WeaponCollectable weaponCollectable:
                    AddWeapon(weaponCollectable.CreateWeapon());
                    return true;
                case ArmorCollectable armorCollectable:
                    if (Character.ArmorFull)
                        return false;

                    Character.AddArmor(5);
                    return true;
                case MedpackCollectable medpackCollectable:
                    if (Character.HitpointsFull)
                        return false;

                    Character.AddHitpoints(5);
                    return true;
                default:
                    throw new Exception($"Unknown collectable {collectable.GetType().Name}");
            }
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location, Entity source)
        {
            Character.HitByProjectile(projectile, vector, location);
        }
    }
}
