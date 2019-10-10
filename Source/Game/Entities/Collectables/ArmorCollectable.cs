using Game.Components;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities.Collectables
{
    public class ArmorCollectable : Collectable
    {
        public ArmorCollectable(Vector location) : base(location, Services.Sprites["armor"])
        {
        }
    }
}
