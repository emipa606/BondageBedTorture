using RimWorld;
using Verse;
using DamageDefOf = SR.DA.Damage.DamageDefOf;
using ThoughtDefOf = SR.DA.Thought.ThoughtDefOf;

namespace SR.DA.Component;

public class CompEffectElectromagneticBearHand : CompUseEffect
{
    private static readonly float dmgAmount = 1f;

    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        usedBy.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SR_Thought_Mistreated);
        var dinfo = new DamageInfo(DamageDefOf.SR_Damage_ElecticShock, dmgAmount);
        usedBy.TakeDamage(dinfo);
    }
}