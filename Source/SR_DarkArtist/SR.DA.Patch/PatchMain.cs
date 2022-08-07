using System.Reflection;
using HarmonyLib;
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
}