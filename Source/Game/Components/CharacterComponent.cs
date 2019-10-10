using SDL2;
using Game.Entities;
using Game.Entities.Collectables;
using Game.Sprites;
using Game.Types;
using Game.Weapons;
using SharedTypes;
using System;

namespace Game.Components
{
    public class CharacterComponent
    {
        private readonly Entity entity;
        private Sprite sprite;

        public Vector AimVector;
        public Vector WeaponLocation;

        public Weapon Weapon;

        public Direction Direction = Direction.Right;
        public const int MaxArmor = 10;
        public const int MaxHitpoints = 10;
        public const int MaxAccuracy = 15;
        public int Hitpoints = MaxHitpoints;
        public int Armor = 0;

        private bool firing = false;

        public CharacterComponent(Entity entity, Sprite textureId, Weapon weapon)
        {
            this.entity = entity;
            this.sprite = textureId;
            this.Weapon = weapon;
        }

        public void Update(uint time, int ticksPassed)
        {
            if (Weapon.AutomaticFire && firing)
                Fire(time);

            Weapon.Update(time);
        }

        public void AimAt(Point point)
        {
            var vector = (new Vector(point.X - 4, point.Y - 4) - entity.Location - WeaponLocation).ToUnit();
            var angle = vector.AngleInDegrees;

            Direction = angle > -90 && angle < 90 ? Direction.Right : Direction.Left;
            WeaponLocation = Direction == Direction.Right ? new Vector(18, 12) : new Vector(entity.Size.X - 18, 12);
            AimVector = vector;
        }

        public void BeginFiring(uint time)
        {
            if (!firing)
                Fire(time);

            firing = true;
        }

        public void StopFiring()
        {
            firing = false;
        }

        public void Fire(uint time, int accuracy = MaxAccuracy)
        {
            var vector = AimVector;

            if (accuracy < MaxAccuracy)
            {
                var angle = vector.AngleInDegrees;
                var rangeInDegrees = MaxAccuracy - accuracy;
                var deviation = Services.Random.Next(0, rangeInDegrees) - (rangeInDegrees / 2);
                angle += deviation;
                vector = Vector.FromAngleInDegrees(angle);
            }

            Weapon.Fire(time, entity, entity.Location + WeaponLocation, vector);
        }

        public void Reload(uint time)
        {
            Weapon.Reload(time);
        }

        public void Render(IntPtr rendererId, Point viewOffset)
        {
            var viewLocation = entity.Location.ToPoint() - viewOffset;

            if (Direction == Direction.Right)
            {
                sprite.DrawEx(rendererId, viewLocation, 0, null, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                Weapon.Render(rendererId, viewLocation + WeaponLocation, AimVector);
            }
            else
            {
                sprite.DrawEx(rendererId, viewLocation, 0, null, SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL);
                Weapon.Render(rendererId, viewLocation + WeaponLocation, AimVector);
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
                Services.Game.Entities.Add(new Gib(entity.Location + location, vector));
            }
        }

        private void Die()
        {
            for (var i = 0; i < 100; i++)
            {
                var angle = Services.Random.Next(0, 360) / (180.0 / Math.PI);
                var vector = new Vector(Math.Sin(angle), Math.Cos(angle));
                var power = Services.Random.Next(5, 10);

                Services.Game.Entities.Add(new Gib(entity.Location + (entity.Size * 0.5), vector * power));
            }

            Services.Game.Entities.Add(new WeaponCollectable(Weapon.WeaponType, entity.Location + (entity.Size * 0.5)));

            entity.Dispose();
        }

        public void FaceDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    AimAt(entity.Location.ToPoint() + new Point(-100, (int)WeaponLocation.Y));
                    break;
                case Direction.Right:
                    AimAt(entity.Location.ToPoint() + new Point(100, (int)WeaponLocation.Y));
                    break;
            }
        }
    }
}
