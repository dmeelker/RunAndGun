using SdlTest.Types;
using SdlTest.Weapons;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities.Enemies
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemy(EnemyType type, Vector location, Direction initialDirection)
        {
            return type switch {
                EnemyType.PistolGrunt => CreatePistolGrunt(location, initialDirection),
                EnemyType.ShotgunGrunt => CreateShotgunGrunt(location, initialDirection),
                _ => throw new Exception($"Unknown type {type}")
            };
        }

        private static Enemy CreatePistolGrunt(Vector location, Direction initialDirection)
        {
            var enemy = new Enemy(location, initialDirection) { 
                Accuracy = 0,
                SenseRange = 400
            };

            enemy.SetWeapon(new Pistol());
            return enemy;
        }

        private static Enemy CreateShotgunGrunt(Vector location, Direction initialDirection)
        {
            var enemy = new Enemy(location, initialDirection)
            {
                Accuracy = 0,
                SenseRange = 400
            };

            enemy.SetWeapon(new Shotgun());
            return enemy;
        }
    }
}
