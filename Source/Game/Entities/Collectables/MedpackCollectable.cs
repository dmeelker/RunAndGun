using Game.Components;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities.Collectables
{
    public class MedpackCollectable : Collectable
    {
        public MedpackCollectable(Vector location) : base(location, Services.Sprites["medpack"])
        {
        }
    }
}
