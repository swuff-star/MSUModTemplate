using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTransit.Buffs
{
    public abstract class BuffBase
    {
        public abstract BuffDef BuffDef { get; set; }
        
        public BuffBase() { }

        public virtual void Initialize() { }

        public virtual void AddBehavior(ref CharacterBody body, int stack) { }
    }
}
