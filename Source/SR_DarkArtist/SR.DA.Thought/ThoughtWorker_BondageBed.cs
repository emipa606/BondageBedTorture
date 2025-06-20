using RimWorld;
using SR.DA.Thing;
using Verse;

namespace SR.DA.Thought;

public class ThoughtWorker_BondageBed : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        Building_Bed buildingBed = p.CurrentBed() as Building_AdvancedBondageBed;
        if (buildingBed != null)
        {
            return ThoughtState.ActiveAtStage(0);
        }

        buildingBed = p.CurrentBed() as Building_BaseBondageBed;
        return buildingBed == null ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(1);
    }
}