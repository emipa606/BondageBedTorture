﻿using RimWorld;
using Verse;

namespace SR.DA.Component
{
    /// <summary>
    /// 电椅触发效果组件
    /// </summary>
    public class CompEffectElectricChair : CompUseEffect
    {
        private static readonly float dmgAmount = 5f;
        public float DmgAmount { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="props"></param>
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            DmgAmount = dmgAmount;
        }
        /// <summary>
        /// 作用效果 电击
        /// </summary>
        /// <param name="usedBy"></param>
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            //如果是受虐狂体质则增加正面心情
            usedBy.needs.mood.thoughts.memories.TryGainMemory(usedBy.story.traits.HasTrait(Trait.TraitDefOf.Masochist) ? Thought.ThoughtDefOf.SR_Thought_HappyElectricShock : Thought.ThoughtDefOf.SR_Thought_PainfulElectricShock);
            var damageInfo = new DamageInfo(Damage.DamageDefOf.SR_Damage_ElecticShock, DmgAmount);
            usedBy.TakeDamage(damageInfo);
        }
        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);
            //高压电模式
            if (signal.Equals("HighVoltageOn"))
            {
                DmgAmount = dmgAmount * 20;
                Messages.Message("SR_HighVoltageOn".Translate(), MessageTypeDefOf.NeutralEvent);
            }
            //普通模式
            else if (signal.Equals("HighVoltageOff"))
            {
                DmgAmount = dmgAmount;
                Messages.Message("SR_HighVoltageOff".Translate(), MessageTypeDefOf.NeutralEvent);
            }
        }
    }
}
