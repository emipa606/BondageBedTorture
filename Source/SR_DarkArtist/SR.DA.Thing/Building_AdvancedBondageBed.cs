using System.Text;
using UnityEngine;
using Verse;

namespace SR.DA.Thing;

public class Building_AdvancedBondageBed : Building_BaseBondageBed
{
    private static readonly Color SheetColorNormal = new Color(0.6313726f, 71f / 85f, 0.7058824f);

    public override Color DrawColorTwo => SheetColorNormal;

    public override void DrawGUIOverlay()
    {
        if (occupant == null)
        {
            return;
        }

        var green = Color.green;
        GenMapUI.DrawThingLabel(this, "SR_Bound".Translate(), green);
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        if (def.building.bed_humanlike)
        {
            stringBuilder.AppendInNewLine("ForColonistUse".Translate());
        }

        return stringBuilder.ToString();
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        ForPrisoners = false;
    }
}