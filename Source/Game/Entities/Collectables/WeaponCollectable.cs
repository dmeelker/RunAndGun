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
    public class WeaponCollectable : Collectable
    {
        private readonly WeaponType weaponType;

        public WeaponCollectable(WeaponType weaponType, Vector location) : base(location, GetSprite(weaponType))
        {
            this.weaponType = weaponType;
        }

        private static Sprite GetSprite(WeaponType weaponType)
        {
            return weaponType switch
            {
                WeaponType.Pistol => Services.Sprites["shotgun"],
                WeaponType.Shotgun => Services.Sprites["shotgun"],
                _ => throw new Exception()
            };
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
