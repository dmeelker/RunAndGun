using Game.Entities;
using Game.Physics;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public static class Explosion
    {
        public static void Create(Vector location, int range, int power)
        {
            Damage(location, range, power);
            GenerateImpulse(location, range, power);
            
        }

        private static void Damage(Vector location, int range, int power)
        {
            var entities = FindEntitiesInRange(location, range);

            foreach (var entity in entities)
            {
                if (!(entity is IExplosionDamageReceiver))
                    continue;

                var distance = Math.Abs((entity.CenterLocation - location).Length);
                if (distance > range)
                    continue;

                var damage = CalculateDamage(distance, range, power);
                ((IExplosionDamageReceiver)entity).Damage(location, damage);
            }
        }

        private static IEnumerable<Entity> FindEntitiesInRange(Vector location, int range)
        {
            var halfRange = range / 2;
            return Services.Game.Entities.FindEntities(new Rect(
                location.X - halfRange,
                location.Y - halfRange,
                location.X + halfRange,
                location.Y + halfRange
            ));
        }

        private static int CalculateDamage(double distanceFromExplosion, int explosionRange, int explosionPower)
        {
            var velocityModifier = 1 - (distanceFromExplosion / (double)explosionRange);
            return (int) (explosionPower * velocityModifier);
        }

        private static void GenerateImpulse(Vector location, int range, int power)
        {
            var components = FindPhysicsComponentsInRange(location, range);

            foreach (var component in components)
            {
                var distance = Math.Abs((component.CenterLocation - location).Length);
                if (distance > range)
                    continue;

                var impactVector = (component.Location - location).ToUnit();
                component.Velocity += impactVector * CalculateExplosionImpulse(range, power, distance);
            }
        }

        private static IEnumerable<PhysicsComponent> FindPhysicsComponentsInRange(Vector location, int range)
        {
            var halfRange = range / 2;
            return Services.Game.Physics.FindComponents(new Rect(
                location.X - halfRange,
                location.Y - halfRange,
                location.X + halfRange,
                location.Y + halfRange
            ));
        }

        private static double CalculateExplosionImpulse(int explosionRange, int explosionPower, double distanceFromExplosion)
        {
            var velocityModifier = 1 - (distanceFromExplosion / (double)explosionRange);
            return explosionPower * velocityModifier;
        }
    }
}
