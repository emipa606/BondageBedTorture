using System.Collections.Generic;
using RimWorld;
using SR.DA.Thing;
using Verse;
using Verse.AI;

namespace SR.DA.Component;

public class CompUsableAdvancedBondageBed : CompUsableBondageBed
{
    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
    {
        if (parent is not Building_BaseBondageBed bbb || pawn.Map == null || pawn.Map != Find.CurrentMap)
        {
            yield break;
        }

        if (!pawn.CanReach((LocalTargetInfo)parent, PathEndMode.Touch, Danger.Deadly))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "NoPath".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "Incapable".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (pawn.WorkTagIsDisabled(WorkTypeDefOf.Warden.workTags))
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "SR_Forbid".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
            yield break;
        }

        if (bbb.occupant != null)
        {
            if (!pawn.CanReserve(bbb.occupant))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabel(bbb.occupant)} (" + "SR_Reserved".Translate() + ")", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            string str2 = "SR_Release_BondageBed".Translate(bbb.occupant.Label);
            yield return new FloatMenuOption(str2, Action2, MenuOptionPriority.DisabledOption);
            yield break;

            void Action2()
            {
                TryReleasePrisoner(pawn, bbb.occupant);
            }
        }

        var hasTarget = false;
        foreach (var target in pawn.Map.mapPawns.AllPawns)
        {
            if (target == pawn || !target.Spawned || !target.IsColonist || target.IsPrisoner)
            {
                continue;
            }

            hasTarget = true;
            if (!pawn.CanReserve(target))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabel(target)} (" + "SR_Reserved".Translate(target.Label) + ")", null,
                    MenuOptionPriority.DisabledOption);
                continue;
            }

            string str =
                "SR_BondageBed".Translate(pawn.Named(pawn.Name.ToString()), target.Named(target.Name.ToString()));
            yield return new FloatMenuOption(str, Action, MenuOptionPriority.DisabledOption);
            continue;

            void Action()
            {
                TryStartUseJob(pawn, target);
            }
        }

        if (!hasTarget)
        {
            yield return new FloatMenuOption($"{FloatMenuOptionLabel(pawn)} (" + "SR_NoTarget".Translate() + ")", null,
                MenuOptionPriority.DisabledOption);
        }
    }
}