using System.Linq;
using RimWorld;
using SR.DA.Thing;
using Verse;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;

namespace SR.DA.Component;

public class CompRemoveEffectBondageBed : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        var building_BondageBed = (Building_BaseBondageBed)parent;
        var hediffBed = HediffDefOf.SR_Hediff_BondageBed;
        using var enumerator = usedBy.health.hediffSet.hediffs.Where(x => x.def == hediffBed).ToList().GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            usedBy.health.RemoveHediff(current);
        }

        building_BondageBed.RemoveOccupant();
    }
}