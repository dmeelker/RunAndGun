using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public interface IExplosionDamageReceiver
    {
        void Damage(Vector explosionOrigin, int damage);
    }
}
