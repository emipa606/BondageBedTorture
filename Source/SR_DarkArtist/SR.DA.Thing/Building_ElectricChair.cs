using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SR.DA.Thing;

public class Building_ElectricChair : Building
{
    private static readonly float workingPower = 5000f;

    private CompFlickable cf;

    private CompPowerTrader cpt;
    private bool isUsing;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref isUsing, "isUsing");
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        cpt = GetComp<CompPowerTrader>();
        cf = GetComp<CompFlickable>();
        OnPowerChanged();
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

    public void OnOrOff(bool b)
    {
        isUsing = b;
        OnPowerChanged();
    }

    private void OnPowerChanged()
    {
        cpt.PowerOutput = !cpt.PowerOn ? 0f : isUsing ? 0f - workingPower : 0f - cpt.Props.basePowerConsumption;
    }
}