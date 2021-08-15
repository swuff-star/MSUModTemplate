using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTransit.Modules
{
    public interface IOnIncomingDamageOtherServerReciever
    {
        void OnIncomingDamageOther(DamageInfo damageInfo);
    }
}
