using System;
using System.Collections.Generic;
using RimWorld;
using SR.DA.Component;
using Verse;

namespace SR.DA.Thing;

public class Building_BaseBondageBed : Building_Bed
{
    public Pawn occupant;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref occupant, "occupant");
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        var currentFaction = factionInt;
        factionInt = null;
        foreach (var gizmo in base.GetGizmos())
        {
            yield return gizmo;
        }

        factionInt = currentFaction;
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        Medical = false;
    }

    public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
    {
        if (occupant != null)
        {
            var compRemoveEffectBondageBed = GetComp<CompRemoveEffectBondageBed>() ??
                                             throw new Exception("cant find comp : CompRemoveEffectBondageBed");
            compRemoveEffectBondageBed.DoEffect(occupant);
        }

        base.DeSpawn(mode);
    }

    public void SetOccupant(Pawn newOccupant)
    {
        if (newOccupant == null)
        {
            return;
        }

        occupant = newOccupant;
        GetComp<CompAssignableToPawn>().TryAssignPawn(newOccupant);
    }

    public void RemoveOccupant()
    {
        GetComp<CompAssignableToPawn>().TryUnassignPawn(occupant);
        occupant = null;
    }

    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
    {
        if (AllComps == null)
        {
            yield break;
        }

        foreach (var thingComp in AllComps)
        {
            foreach (var item in thingComp.CompFloatMenuOptions(myPawn))
            {
                yield return item;
            }
        }
    }
}