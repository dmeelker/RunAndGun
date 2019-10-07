using SdlTest.Components;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities.Collectables
{
    public class MedpackCollectable : Collectable
    {
        public MedpackCollectable(Vector location) : base(location, Services.Sprites["shotgun"])
        {
        }
    }
}
