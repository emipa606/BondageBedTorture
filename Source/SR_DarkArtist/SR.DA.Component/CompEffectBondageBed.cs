using RimWorld;
using SR.DA.Patch;
using SR.DA.Thing;
using Verse;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;

namespace SR.DA.Component;

public class CompEffectBondageBed : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        var sR_Hediff_BondageBed = HediffDefOf.SR_Hediff_BondageBed;
        var partsWithDef = usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Arm);
        if (partsWithDef != null)
        {
            foreach (var item in partsWithDef)
            {
                if (item != null && !usedBy.health.hediffSet.PartIsMissing(item))
                {
                    usedBy.health.AddHediff(sR_Hediff_BondageBed, item);
                }
            }
        }

        var partsWithDef2 = usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Leg);
        if (partsWithDef2 == null)
        {
            return;
        }

        foreach (var item2 in partsWithDef2)
        {
            if (item2 != null && !usedBy.health.hediffSet.PartIsMissing(item2))
            {
                usedBy.health.AddHediff(sR_Hediff_BondageBed, item2);
            }
        }
    }

    public override void CompTickRare()
    {
        base.CompTickRare();
        if (parent is not Pawn pawn)
        {
            return;
        }

        if (pawn.CurrentBed() is Building_BondageBed)
        {
            return;
        }

        PatchMain.RemoveBondadgeHediff(pawn);
    }
}