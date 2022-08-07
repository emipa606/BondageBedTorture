using System.Collections.Generic;
using RimWorld;
using SR.DA.Component;
using Verse;
using Verse.AI;

namespace SR.DA.Job;

public class JobDriver_ReleaseBondageChains : JobDriver_UseItem
{
    protected Verse.Thing Target => job.GetTarget(TargetIndex.A).Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDestroyedOrNull(TargetIndex.A);
        this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
        var prisoner = (Pawn)Target;
        if (prisoner == null || prisoner.Dead)
        {
            yield break;
        }

        yield return Toils_General.WaitWith(TargetIndex.A, 60, true, true);
        yield return Toils_Reserve.Release(TargetIndex.A);
        yield return new Toil
        {
            initAction = delegate
            {
                var compUsableRemoveEffectChians = prisoner.TryGetComp<CompUsableRemoveEffectChians>();
                if (compUsableRemoveEffectChians == null)
                {
                    return;
                }

                compUsableRemoveEffectChians.UsedBy(prisoner);
                MoteMaker.ThrowText(prisoner.PositionHeld.ToVector3(), prisoner.MapHeld,
                    "SR_Release".Translate(), 4f);
            },
            defaultCompleteMode = ToilCompleteMode.Instant
        };
    }
}