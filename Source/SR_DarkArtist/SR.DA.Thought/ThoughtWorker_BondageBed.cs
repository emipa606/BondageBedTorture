using RimWorld;
using SR.DA.Thing;
using Verse;

namespace SR.DA.Thought;

public class ThoughtWorker_BondageBed : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        Building_Bed building_Bed = p.CurrentBed() as Building_AdvancedBondageBed;
        if (building_Bed != null)
        {
            return ThoughtState.ActiveAtStage(0);
        }

        building_Bed = p.CurrentBed() as Building_BaseBondageBed;
        return building_Bed == null ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(1);
    }
}