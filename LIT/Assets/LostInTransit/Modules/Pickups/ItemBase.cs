using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTransit.Items
{
    public abstract class ItemBase
    {
        public abstract ItemDef ItemDef { get; set; }

        public virtual void Initialize() { }

        public virtual void AddBehavior(ref CharacterBody body, int stack) { }
    }
}
