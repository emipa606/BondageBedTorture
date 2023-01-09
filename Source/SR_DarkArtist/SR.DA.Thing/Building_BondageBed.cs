using System.Text;
using UnityEngine;
using Verse;

namespace SR.DA.Thing;

public class Building_BondageBed : Building_BaseBondageBed
{
    private static readonly Color SheetColorNormal = new Color(0.654902f, 32f / 85f, 13f / 85f);

    public override Color DrawColorTwo => SheetColorNormal;

    public override void DrawGUIOverlay()
    {
        if (occupant == null)
        {
            return;
        }

        if (!SRMod.instance.Settings.ShowBound)
        {
            return;
        }

        var yellow = Color.yellow;
        GenMapUI.DrawThingLabel(this, "SR_Bound".Translate(), yellow);
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        if (def.building.bed_humanlike)
        {
            stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
        }

        return stringBuilder.ToString();
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        ForPrisoners = true;
    }
}