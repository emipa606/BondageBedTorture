using System;
using SR.DA.Component;
using Verse;

namespace SR.DA.Thing;

public static class ThingExtension
{
    public static bool HasChains(this Pawn pawn)
    {
        return (pawn.GetComp<CompUsableRemoveEffectChians>() ??
                throw new Exception("cant find comp:CompUsableRemoveEffectChians")).IsBondaged;
    }
}