using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR.DA.Component;

public class CompHighVoltage : ThingComp
{
    private const string OffGraphicSuffix = "_Off";

    public const string HighVoltageOnSignal = "HighVoltageOn";

    public const string HighVoltageOffSignal = "HighVoltageOff";
    private Texture2D cachedCommandTex;

    private Graphic offGraphic;

    private bool switchIsOn;

    private CompProperties_HighVoltage Props => (CompProperties_HighVoltage)props;

    private Texture2D CommandTex
    {
        get
        {
            if (cachedCommandTex == null)
            {
                cachedCommandTex = ContentFinder<Texture2D>.Get(Props.commandTexture);
            }

            return cachedCommandTex;
        }
    }

    public bool SwitchIsOn
    {
        get => switchIsOn;
        set
        {
            switchIsOn = value;
            parent.BroadcastCompSignal(switchIsOn ? "HighVoltageOn" : "HighVoltageOff");

            if (parent.Spawned)
            {
                parent.Map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
            }
        }
    }

    public Graphic CurrentGraphic
    {
        get
        {
            if (SwitchIsOn)
            {
                return parent.DefaultGraphic;
            }

            if (offGraphic == null)
            {
                offGraphic = GraphicDatabase.Get(parent.def.graphicData.graphicClass,
                    $"{parent.def.graphicData.texPath}_Off", parent.def.graphicData.shaderType.Shader,
                    parent.def.graphicData.drawSize, parent.DrawColor, parent.DrawColorTwo);
            }

            return offGraphic;
        }
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref switchIsOn, "switchIsOn", true);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var item in base.CompGetGizmosExtra())
        {
            yield return item;
        }

        if (parent.Faction == Faction.OfPlayer)
        {
            yield return new Command_Toggle
            {
                icon = CommandTex,
                defaultLabel = Props.commandLabelKey.Translate(),
                defaultDesc = Props.commandDescKey.Translate(),
                isActive = () => switchIsOn,
                toggleAction = delegate { SwitchIsOn = !SwitchIsOn; }
            };
        }
    }
}