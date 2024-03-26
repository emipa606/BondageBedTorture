using RimWorld;
using Verse;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;

namespace SR.DA.Component;

public class CompEffectBondageChains : CompUseEffect
{
    [NoTranslate] private static readonly string signal = "SR_Bound";

    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        var sR_Hediff_BondageChains = HediffDefOf.SR_Hediff_BondageChains;
        var partsWithDef = usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Arm);
        var addedChains = false;
        if (partsWithDef != null)
        {
            foreach (var item in partsWithDef)
            {
                if (item == null || usedBy.health.hediffSet.PartIsMissing(item))
                {
                    continue;
                }

                usedBy.health.AddHediff(sR_Hediff_BondageChains, item);
                addedChains = true;
            }
        }

        var partsWithDef2 = usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Leg);
        if (partsWithDef2 != null)
        {
            foreach (var item2 in partsWithDef2)
            {
                if (item2 == null || usedBy.health.hediffSet.PartIsMissing(item2))
                {
                    continue;
                }

                usedBy.health.AddHediff(sR_Hediff_BondageChains, item2);
                addedChains = true;
            }
        }

        if (addedChains)
        {
            usedBy.BroadcastCompSignal(signal);
            parent.SplitOff(1).Destroy();
        }
        else
        {
            Messages.Message($"fail.{usedBy}dont have arms and legs.", MessageTypeDefOf.NeutralEvent);
        }
    }
}