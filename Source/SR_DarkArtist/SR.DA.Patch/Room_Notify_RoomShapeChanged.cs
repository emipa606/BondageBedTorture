using HarmonyLib;
using SR.DA.Thing;
using Verse;

namespace SR.DA.Patch;

[HarmonyPatch(typeof(Room), "Notify_RoomShapeChanged")]
internal class Room_Notify_RoomShapeChanged
{
    private static void Postfix(ref Room __instance)
    {
        foreach (var containedBed in __instance.ContainedBeds)
        {
            if (containedBed is Building_AdvancedBondageBed)
            {
                if (containedBed.ForPrisoners)
                {
                    containedBed.ForPrisoners = false;
                }

                continue;
            }

            if (containedBed is not Building_BondageBed)
            {
                continue;
            }

            if (!containedBed.ForPrisoners)
            {
                containedBed.ForPrisoners = true;
            }
        }
    }
}