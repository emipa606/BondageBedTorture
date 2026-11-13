using System.Collections.Generic;
using RimWorld;
using SR.DA.Thing;
using Verse;
using Verse.AI;
using JobDefOf = SR.DA.Job.JobDefOf;

namespace SR.DA.Component;

public class CompUsableBondageBed : CompUsable
{
    protected override string FloatMenuOptionLabel(Pawn pawn)
    {
        return "SR_CantUse".Translate();
    }

    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
    {
        if (parent is not Building_BaseBondageBed bbb || myPawn.Map == null || myPawn.Map != Find.CurrentMap)
        {
            yield break;
        }

        if (!myPawn.CanReach((LocalTargetInfo)parent, PathEndMode.Touch, Danger.Deadly))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "NoPath".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "Incapable".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (myPawn.WorkTagIsDisabled(WorkTypeDefOf.Warden.workTags))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "SR_Forbid".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (bbb.occupant != null)
        {
            if (!myPawn.CanReserve(bbb.occupant))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabel(bbb.occupant)} (" + "SR_Reserved".Translate() + ")", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            string str2 = "SR_Release_BondageBed".Translate(bbb.occupant.Label);
            yield return new FloatMenuOption(str2, action2, MenuOptionPriority.DisabledOption);
            yield break;

            void action2()
            {
                TryReleasePrisoner(myPawn, bbb.occupant);
            }
        }

        var hasPrisoner = false;
        foreach (var prisoner in myPawn.Map.mapPawns.AllPawns)
        {
            if (prisoner == myPawn || !prisoner.Spawned || !prisoner.IsPrisonerOfColony)
            {
                continue;
            }

            hasPrisoner = true;
            if (!myPawn.CanReserve(prisoner))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabel(prisoner)} (" + "SR_Reserved".Translate(prisoner.Label) + ")", null,
                    MenuOptionPriority.DisabledOption);
                continue;
            }

            string str = "SR_BondageBed".Translate(myPawn.Named(myPawn.Name.ToString()),
                prisoner.Named(prisoner.Name.ToString()));
            yield return new FloatMenuOption(str, action, MenuOptionPriority.DisabledOption);
            continue;

            void action()
            {
                TryStartUseJob(myPawn, prisoner);
            }
        }

        if (!hasPrisoner)
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "SR_NoPrisoner".Translate() + ")",
                null, MenuOptionPriority.DisabledOption);
        }
    }

    protected void TryReleasePrisoner(Pawn pawn, LocalTargetInfo extraTarget)
    {
        if (!pawn.CanReach((LocalTargetInfo)parent, PathEndMode.Touch, Danger.Some) ||
            !pawn.CanReserveAndReach(extraTarget, PathEndMode.Touch, Danger.Some))
        {
            return;
        }

        var job = extraTarget.IsValid
            ? JobMaker.MakeJob(JobDefOf.SR_Job_ReleaseBed, parent, extraTarget)
            : JobMaker.MakeJob(JobDefOf.SR_Job_ReleaseBed, parent);
        job.count = 1;
        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
    }

    public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
    {
        if (!pawn.CanReserveAndReach(parent, PathEndMode.Touch, Danger.Some) ||
            !pawn.CanReserveAndReach(extraTarget, PathEndMode.Touch, Danger.Some))
        {
            return;
        }

        var job = extraTarget.IsValid
            ? JobMaker.MakeJob(Props.useJob, parent, extraTarget)
            : JobMaker.MakeJob(Props.useJob, parent);
        job.count = 1;
        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
    }
}