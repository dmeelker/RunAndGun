using SdlTest.Components;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities.Collectables
{
    class ArmorCollectable : Collectable
    {
        public ArmorCollectable(Vector location) : base(location, Services.SpriteManager["shotgun"])
        {
        }
    }
}
