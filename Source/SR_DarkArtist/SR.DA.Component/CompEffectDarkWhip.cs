using RimWorld;
using Verse;
using ThoughtDefOf = SR.DA.Thought.ThoughtDefOf;

namespace SR.DA.Component;

public class CompEffectDarkWhip : CompUseEffect
{
    private static readonly float dmgAmount = 1f;

    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        usedBy.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SR_Thought_Mistreated);
        usedBy.TakeDamage(new DamageInfo(DamageDefOf.Crush, dmgAmount));
    }
}