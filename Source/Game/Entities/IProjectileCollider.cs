using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    interface IProjectileCollider
    {
        void HitByProjectile(Projectile projectile, Vector vector, Vector location, Entity source);
    }
}
