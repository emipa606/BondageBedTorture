using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace SR.DA.Component;

public class CompUsableTorture : CompUsable
{
    protected override string FloatMenuOptionLabel(Pawn pawn)
    {
        return "SR_CantUse".Translate();
    }

    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
    {
        if (myPawn.Map == null || myPawn.Map != Find.CurrentMap)
        {
            yield break;
        }

        if (!myPawn.CanReach((LocalTargetInfo)parent, PathEndMode.Touch, Danger.Deadly))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "NoPath".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!myPawn.CanReserve(parent))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "Reserved".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "Incapable".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
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

            string str = "SR_Torture".Translate(myPawn.Named(myPawn.Name.ToString()),
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