using HarmonyLib;
using RimWorld;
using SR.DA.Thing;

namespace SR.DA.Patch;

[HarmonyPatch(typeof(RestUtility), "IsValidBedFor")]
internal class RestUtility_IsValidBedFor
{
    private static bool Prefix(ref bool __result, Verse.Thing bedThing)
    {
        if (bedThing is not Building_BaseBondageBed)
        {
            return true;
        }

        __result = false;
        return false;
    }
}