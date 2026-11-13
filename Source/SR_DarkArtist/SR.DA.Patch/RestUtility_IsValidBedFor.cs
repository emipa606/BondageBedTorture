using HarmonyLib;
using RimWorld;
using SR.DA.Thing;

namespace SR.DA.Patch;

[HarmonyPatch(typeof(RestUtility), nameof(RestUtility.IsValidBedFor))]
internal static class RestUtility_IsValidBedFor
{
    public static bool Prefix(ref bool __result, Verse.Thing bedThing)
    {
        if (bedThing is not Building_BaseBondageBed)
        {
            return true;
        }

        __result = false;
        return false;
    }
}