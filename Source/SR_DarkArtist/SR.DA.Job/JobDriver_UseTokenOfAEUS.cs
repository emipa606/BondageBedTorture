using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace SR.DA.Job;

public class JobDriver_UseTokenOfAEUS : JobDriver_UseItem
{
    protected Verse.Thing Thing => job.GetTarget(TargetIndex.A).Thing;

    protected Verse.Thing Target => job.GetTarget(TargetIndex.B).Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(Thing, job, 1, -1, null, errorOnFailed) &&
               pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var prisoner = (Pawn)Target;
        this.FailOnDestroyedOrNull(TargetIndex.A);
        this.FailOnDestroyedOrNull(TargetIndex.B);
        this.FailOnAggroMentalStateAndHostile(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
        yield return new Toil
        {
            initAction = delegate { pawn.carryTracker.TryStartCarry(Thing, 1); },
            defaultCompleteMode = ToilCompleteMode.Instant
        };
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
        if (prisoner.Dead)
        {
            yield break;
        }

        yield return Toils_General.WaitWith(TargetIndex.B, 60, true, true);
        yield return Toils_Reserve.Release(TargetIndex.A);
        yield return Toils_Reserve.Release(TargetIndex.B);
        yield return new Toil
        {
            initAction = delegate { Thing?.TryGetComp<CompUseEffect>()?.DoEffect(prisoner); },
            defaultCompleteMode = ToilCompleteMode.Instant
        };
    }
}