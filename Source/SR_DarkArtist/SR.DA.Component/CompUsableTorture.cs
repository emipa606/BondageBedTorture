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

    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
    {
        if (pawn.Map == null || pawn.Map != Find.CurrentMap)
        {
            yield break;
        }

        if (!pawn.CanReach((LocalTargetInfo)parent, PathEndMode.Touch, Danger.Deadly))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "NoPath".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!pawn.CanReserve(parent))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "Reserved".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "Incapable".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        var hasPrisoner = false;
        foreach (var prisoner in pawn.Map.mapPawns.AllPawns)
        {
            if (prisoner == pawn || !prisoner.Spawned || !prisoner.IsPrisonerOfColony)
            {
                continue;
            }

            hasPrisoner = true;
            if (!pawn.CanReserve(prisoner))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabel(prisoner)} (" + "SR_Reserved".Translate(prisoner.Label) + ")", null,
                    MenuOptionPriority.DisabledOption);
                continue;
            }

            void Action()
            {
                TryStartUseJob(pawn, prisoner);
            }

            string str = "SR_Torture".Translate(pawn.Named(pawn.Name.ToString()),
                prisoner.Named(prisoner.Name.ToString()));
            yield return new FloatMenuOption(str, Action, MenuOptionPriority.DisabledOption);
        }

        if (!hasPrisoner)
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "SR_NoPrisoner".Translate() + ")",
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