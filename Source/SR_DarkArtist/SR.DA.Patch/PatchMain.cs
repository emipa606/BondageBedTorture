using System.Linq;
using System.Reflection;
using HarmonyLib;
using SR.DA.Hediff;
using Verse;

namespace SR.DA.Patch;

[StaticConstructorOnStartup]
public class PatchMain
{
    public static Harmony instance;

    static PatchMain()
    {
        instance = new Harmony("SR.DarkArtist");
        instance.PatchAll(Assembly.GetExecutingAssembly());
    }

    public static void RemoveBondadgeHediff(Pawn pawn)
    {
        var hediffBed = HediffDefOf.SR_Hediff_BondageBed;
        using var enumerator = pawn.health.hediffSet.hediffs.Where(x => x.def == hediffBed).ToList().GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            pawn.health.RemoveHediff(current);
        }
    }
}