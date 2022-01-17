using R2API;
using RoR2;

namespace LostInTransit
{
    public static class LITUtil
    {
        public static void AddCooldownBuff(this CharacterBody body, BuffDef buffDef, float seconds)
        {
            for (int i = 0; i <= seconds; i++)
            {
                body.AddTimedBuff(buffDef, i);
            }
        }
    }
}
