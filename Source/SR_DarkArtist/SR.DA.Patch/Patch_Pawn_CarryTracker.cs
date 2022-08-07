using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using SR.DA.Component;
using SR.DA.Thing;
using Verse;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;

namespace SR.DA.Patch;

[HarmonyPatch]
internal class Patch_Pawn_CarryTracker
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(Pawn_CarryTracker), "TryStartCarry", new[] { typeof(Verse.Thing) });
        yield return AccessTools.Method(typeof(Pawn_CarryTracker), "TryStartCarry",
            new[] { typeof(Verse.Thing), typeof(int), typeof(bool) });
    }

    private static void Prefix(Verse.Thing item)
    {
        if (item == null || item.GetType() != typeof(Pawn))
        {
            return;
        }

        var pawn = (Pawn)item;
        var inBondageBed = false;
        foreach (var hediff in pawn.health.hediffSet.hediffs)
        {
            if (hediff.def != HediffDefOf.SR_Hediff_BondageBed)
            {
                continue;
            }

            inBondageBed = true;
            break;
        }

        if (inBondageBed)
        {
            ((Building_BaseBondageBed)pawn.CurrentBed())?.GetComp<CompRemoveEffectBondageBed>()?.DoEffect(pawn);
        }
    }
}