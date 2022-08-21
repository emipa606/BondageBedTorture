using RimWorld;
using SR.DA.Patch;
using SR.DA.Thing;
using Verse;

namespace SR.DA.Component;

public class CompRemoveEffectBondageBed : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        var building_BondageBed = (Building_BaseBondageBed)parent;

        PatchMain.RemoveBondadgeHediff(usedBy);

        building_BondageBed.RemoveOccupant();
    }
}