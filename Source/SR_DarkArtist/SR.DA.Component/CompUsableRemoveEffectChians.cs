using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;
using ThingDefOf = SR.DA.Thing.ThingDefOf;

namespace SR.DA.Component;

public class CompUsableRemoveEffectChians : CompUsable
{
    private bool isBondaged;

    public bool IsBondaged => isBondaged;

    protected override string FloatMenuOptionLabel(Pawn pawn)
    {
        return "SR_CantRemove".Translate();
    }

    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
    {
        if (!isBondaged || myPawn.Map == null || myPawn.Map != Find.CurrentMap)
        {
            yield break;
        }

        _ = parent as Pawn ?? throw new Exception("prisoner is null");

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

        if (myPawn.WorkTagIsDisabled(WorkTypeDefOf.Warden.workTags))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(myPawn)} (" + "SR_Forbid".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        string str = "SR_Release_BondageChains".Translate(parent.Label);
        yield return new FloatMenuOption(str, Action, MenuOptionPriority.DisabledOption);
        yield break;

        void Action()
        {
            TryStartUseJob(myPawn, parent);
        }
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref isBondaged, "isBondaged");
    }

    public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
    {
        if (!pawn.CanReserveAndReach(extraTarget, PathEndMode.Touch, Danger.Some))
        {
            return;
        }

        var job = JobMaker.MakeJob(Props.useJob, extraTarget);
        job.count = 1;
        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
    }

    public override void ReceiveCompSignal(string signal)
    {
        base.ReceiveCompSignal(signal);
        if (signal.Equals("SR_Bound"))
        {
            isBondaged = true;
        }
    }

    public new void UsedBy(Pawn usedBy)
    {
        var hediff = HediffDefOf.SR_Hediff_BondageChains;
        using var enumerator = usedBy.health.hediffSet.hediffs.Where(x => x.def == hediff).ToList().GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            usedBy.health.RemoveHediff(current);
        }

        var thing = ThingMaker.MakeThing(ThingDefOf.SR_Item_Chains);
        thing.stackCount = 1;
        GenPlace.TryPlaceThing(thing, usedBy.Position, usedBy.Map, ThingPlaceMode.Near);
        isBondaged = false;
    }
}