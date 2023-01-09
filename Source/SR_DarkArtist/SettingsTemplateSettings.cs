using Verse;

namespace SR;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class SRSettings : ModSettings
{
    public bool ShowBound = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowBound, "ShowBound", true);
    }
}