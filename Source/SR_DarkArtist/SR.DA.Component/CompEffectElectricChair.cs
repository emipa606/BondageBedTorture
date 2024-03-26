using RimWorld;
using Verse;
using DamageDefOf = SR.DA.Damage.DamageDefOf;
using ThoughtDefOf = SR.DA.Thought.ThoughtDefOf;

namespace SR.DA.Component;

public class CompEffectElectricChair : CompUseEffect
{
    private static readonly float dmgAmount = 5f;

    public float DmgAmount { get; set; }

    public override void Initialize(CompProperties props)
    {
        base.Initialize(props);
        DmgAmount = dmgAmount;
    }

    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        usedBy.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SR_Thought_Mistreated);
        var dinfo = new DamageInfo(DamageDefOf.SR_Damage_ElecticShock, DmgAmount);
        usedBy.TakeDamage(dinfo);
    }

    public override void ReceiveCompSignal(string signal)
    {
        base.ReceiveCompSignal(signal);
        switch (signal)
        {
            case "HighVoltageOn":
                DmgAmount = dmgAmount * 20f;
                Messages.Message("SR_HighVoltageOn".Translate(), MessageTypeDefOf.NeutralEvent);
                break;
            case "HighVoltageOff":
                DmgAmount = dmgAmount;
                Messages.Message("SR_HighVoltageOff".Translate(), MessageTypeDefOf.NeutralEvent);
                break;
        }
    }
}