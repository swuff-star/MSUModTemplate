using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceBarrier : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceBarrier");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceBarrierBehavior>(stack);
        }

        public class DiceBarrierBehavior : CharacterBody.ItemBehavior
        {
            private float origBarrierDecay;
            public void Start()
            {
                origBarrierDecay = body.barrierDecayRate;
                body.barrierDecayRate *= HGMath.Clamp((Items.BlessedDice.decayMult / 100), 0, 1);
                body.healthComponent.AddBarrier(body.maxBarrier * (Items.BlessedDice.barrierAmount / 100));
            }
            public void OnDestroy()
            {
                body.barrierDecayRate = origBarrierDecay;
            }
        }
    }
}