using System;
using System.Collections.Generic;
using RimWorld;
using SR.DA.Component;
using SR.DA.Thing;
using Verse;
using Verse.AI;
using ThoughtDefOf = SR.DA.Thought.ThoughtDefOf;

namespace SR.DA.Job;

public class JobDriver_UseElectricChair : JobDriver_UseItem
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
        this.FailOnDestroyedOrNull(TargetIndex.A);
        this.FailOnDestroyedOrNull(TargetIndex.B);
        this.FailOnAggroMentalStateAndHostile(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
        yield return Toils_Haul.StartCarryThing(TargetIndex.B);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnForbidden(TargetIndex.A);
        var prisoner = (Pawn)Target;
        var chair = (Building_ElectricChair)Thing;
        if (!prisoner.Dead)
        {
            var toilWaitWith = Toils_General.WaitWith(TargetIndex.A, 180, true, true);
            var cpt = chair.GetComp<CompPowerTrader>() ?? throw new Exception("cant find comp:CompPowerTrader");
            toilWaitWith.AddPreInitAction(delegate { chair.OnOrOff(true); });
            toilWaitWith.AddFinishAction(delegate { chair.OnOrOff(false); });
            toilWaitWith.tickAction = delegate
            {
                if (cpt.PowerOn)
                {
                    return;
                }

                Messages.Message("SR_ElectrocutionFailure".Translate(prisoner.Label),
                    MessageTypeDefOf.NeutralEvent);
                pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
            };
            yield return toilWaitWith;
            yield return new Toil
            {
                initAction = delegate
                {
                    pawn.carryTracker.TryDropCarriedThing(Thing.Position, ThingPlaceMode.Direct, out var _);
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }

        yield return Toils_Reserve.Release(TargetIndex.A);
        yield return Toils_Reserve.Release(TargetIndex.B);
        yield return new Toil
        {
            initAction = delegate
            {
                var compEffectElectricChair = Thing?.TryGetComp<CompEffectElectricChair>();
                if (compEffectElectricChair == null)
                {
                    return;
                }

                compEffectElectricChair.DoEffect(prisoner);
                pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SR_Thought_Maltreat);
                MoteMaker.ThrowText(Target.PositionHeld.ToVector3(), Target.MapHeld,
                    "SR_ElectricShock".Translate(), 4f);
            },
            defaultCompleteMode = ToilCompleteMode.Instant
        };
    }
}