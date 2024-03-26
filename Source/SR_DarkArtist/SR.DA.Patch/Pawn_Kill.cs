using HarmonyLib;
using RimWorld;
using SR.DA.Component;
using SR.DA.Thing;
using Verse;
using HediffDefOf = SR.DA.Hediff.HediffDefOf;

namespace SR.DA.Patch;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill))]
internal class Pawn_Kill
{
    private static bool Prefix(ref Pawn __instance)
    {
        var inBondadgeBed = false;
        foreach (var hediff in __instance.health.hediffSet.hediffs)
        {
            if (hediff.def != HediffDefOf.SR_Hediff_BondageBed)
            {
                continue;
            }

            inBondadgeBed = true;
            break;
        }

        if (!inBondadgeBed)
        {
            return true;
        }

        var building_BondageBed = (Building_BaseBondageBed)__instance.CurrentBed();
        var comp = building_BondageBed.GetComp<CompRemoveEffectBondageBed>();
        if (comp == null)
        {
            return true;
        }

        comp.DoEffect(__instance);
        return false;
    }
}