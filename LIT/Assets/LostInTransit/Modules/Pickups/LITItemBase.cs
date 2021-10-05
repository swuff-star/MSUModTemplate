using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;

namespace LostInTransit.Items
{
    public abstract class LITItemBase : ItemBase
    {
        public virtual void Config() { }
        public abstract void DescriptionToken();
    }
}
