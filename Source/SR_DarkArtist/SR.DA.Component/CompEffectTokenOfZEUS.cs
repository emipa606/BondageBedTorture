using RimWorld;
using Verse;

namespace SR.DA.Component;

public class CompEffectTokenOfZEUS : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        usedBy.guest.resistance = 0f;
        parent.SplitOff(1).Destroy();
        Messages.Message("SR_BrainWashing".Translate(usedBy.Label), MessageTypeDefOf.NeutralEvent);
    }
}