using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR.DA.Component;

public class CompHighVoltage : ThingComp
{
    private const string HighVoltageOnSignal = "HighVoltageOn";

    private const string HighVoltageOffSignal = "HighVoltageOff";
    private Texture2D cachedCommandTex;

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

    private bool SwitchIsOn
    {
        get => switchIsOn;
        set
        {
            switchIsOn = value;
            parent.BroadcastCompSignal(switchIsOn ? HighVoltageOnSignal : HighVoltageOffSignal);

            if (parent.Spawned)
            {
                parent.Map.mapDrawer.MapMeshDirty(parent.Position,
                    MapMeshFlagDefOf.Things | MapMeshFlagDefOf.Buildings);
            }
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