using SdlTest.Components;
using SdlTest.Sprites;
using SdlTest.Types;
using SdlTest.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SdlTest.Entities.Collectables
{

    public class WeaponCollectable : Entity, ICollectable
    {
        private readonly PhysicsComponent physicsComponent;
        private readonly WeaponType weaponType;
        private readonly Sprite sprite;
        private int yOffset = 0;

        public WeaponCollectable(WeaponType weaponType, Vector location)
        {
            sprite = weaponType switch {
                WeaponType.Pistol => Services.SpriteManager["shotgun"],
                WeaponType.Shotgun => Services.SpriteManager["shotgun"],
                _ => throw new Exception()
            };

            physicsComponent = new PhysicsComponent(this);
            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);
            this.weaponType = weaponType;
        }

        public override void Render(IntPtr rendererId)
        {
            sprite.Draw(rendererId, (int)Location.X, (int)Location.Y + yOffset);
        }

        public override void Update(uint time, int ticksPassed)
        {
            physicsComponent.Update(ticksPassed, Services.Session.Level);
            yOffset = (int) -Math.Abs(Math.Sin((time / 200) ) * 8.0);

            var boundingBox = GetBoundingBox();
            var entityCollisions = Services.EntityManager.FindEntities(boundingBox).ToArray();

            foreach (var entity in entityCollisions)
            {
                if (!(entity is PlayerEntity))
                    continue;

                ((PlayerEntity)entity).Collect(this);
                Dispose();
                return;
            }
        }

        public Weapon CreateWeapon()
        {
            return weaponType switch { 
                WeaponType.Pistol => new Pistol(),
                WeaponType.Shotgun => new Shotgun(),
                _ => throw new Exception("Unknown weapon")
            };
        }
    }
}
