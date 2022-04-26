using R2API;
using RoR2;

namespace MyMod
{
    public static class MyModUtil
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
