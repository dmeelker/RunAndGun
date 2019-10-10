using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    interface IProjectileCollider
    {
        void HitByProjectile(Projectile projectile, Vector vector, Vector location, Entity source);
    }
}
