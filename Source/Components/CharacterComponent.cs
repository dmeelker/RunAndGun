using SDL2;
using SdlTest.Entities;
using SdlTest.Entities.Collectables;
using SdlTest.Sprites;
using SdlTest.Types;
using SdlTest.Weapons;
using System;

namespace SdlTest.Components
{
    public class CharacterComponent
    {
        private readonly Entity entity;
        private Sprite sprite;

        public Vector AimVector;
        public Vector WeaponOffset;

        public Weapon Weapon;

        public Direction Direction = Direction.Right;
        public const int MaxArmor = 10;
        public const int MaxHitpoints = 10;
        public int Hitpoints = MaxHitpoints;
        public int Armor = 0;

        public CharacterComponent(Entity entity, Sprite textureId, Weapon weapon)
        {
            this.entity = entity;
            this.sprite = textureId;
            this.Weapon = weapon;
        }

        public void Update(uint time, int ticksPassed)
        {
            Weapon.Update(time);
        }

        public void AimAt(int x, int y)
        {
            var vector = (new Vector(x - 4, y - 4) - entity.Location).ToUnit();
            var angle = vector.AngleInDegrees;

            Direction = angle > -90 && angle < 90 ? Direction.Right : Direction.Right;
            WeaponOffset = Direction == Direction.Right ? new Vector(10, 12) : new Vector(entity.Size.X - 10, 12);

            AimVector = (new Vector(x - 4, y - 4) - entity.Location - WeaponOffset).ToUnit();
        }

        public void Fire(uint time)
        {
            Weapon.Fire(time, entity, entity.Location + WeaponOffset, AimVector);
        }

        public void Reload(uint time)
        {
            Weapon.Reload(time);
        }

        public void Render(IntPtr rendererId)
        {
            if (Direction == Direction.Right)
            {
                sprite.DrawEx(rendererId, (int)entity.Location.X, (int)entity.Location.Y, 0, null, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                Weapon.Render(rendererId, new Vector(entity.Location.X + 10, entity.Location.Y + 12), AimVector);
            }
            else
            {
                sprite.DrawEx(rendererId, (int)entity.Location.X, (int)entity.Location.Y, 0, null, SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL);
                Weapon.Render(rendererId, new Vector(entity.Location.X + entity.Size.X - 10, entity.Location.Y + 12), AimVector);
            }
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location)
        {
            var spawnGibs = projectile.Power > Armor;

            Damage(projectile.Power);

            if(spawnGibs)
                SpawnGibs(vector, location);
        }

        public void AddHitpoints(int amount) => Hitpoints = Math.Min(Hitpoints + amount, MaxHitpoints);

        public void AddArmor(int amount) => Armor = Math.Min(Armor + amount, MaxArmor);

        public bool ArmorFull => Armor == MaxArmor;
        public bool HitpointsFull => Hitpoints == MaxHitpoints;

        public void Damage(int amount)
        {
            if(Armor > 0)
            {
                var armorToRemove = Math.Min(amount, Armor);
                Armor -= armorToRemove;
                amount -= armorToRemove;
            }

            Hitpoints -= amount;

            if (Hitpoints <= 0)
                Die();
        }

        private void SpawnGibs(Vector vector, Vector location)
        {
            var count = Services.Random.Next(2, 5);

            for (var i = 0; i < count; i++)
            {
                vector = vector.ToUnit() * Services.Random.Next(2, 5);
                Services.EntityManager.Add(new Gib(entity.Location + location, vector));
            }
        }

        private void Die()
        {
            for (var i = 0; i < 100; i++)
            {
                var angle = Services.Random.Next(0, 360) / (180.0 / Math.PI);
                var vector = new Vector(Math.Sin(angle), Math.Cos(angle));
                var power = Services.Random.Next(5, 10);

                Services.EntityManager.Add(new Gib(entity.Location + (entity.Size * 0.5), vector * power));
            }

            Services.EntityManager.Add(new WeaponCollectable(WeaponType.Shotgun, entity.Location + (entity.Size * 0.5)));

            entity.Dispose();
        }
    }
}
