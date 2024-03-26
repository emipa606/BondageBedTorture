using Verse;

namespace SR.DA.Component;

public class CompProperties_HighVoltage : CompProperties
{
    [NoTranslate] public readonly string commandDescKey = "SR_CommandDesignateHighVoltageDesc";

    [NoTranslate] public readonly string commandLabelKey = "SR_CommandDesignateHighVoltageLabel";

    [NoTranslate] public readonly string commandTexture = "UI/Commands/HighVoltage";

    public CompProperties_HighVoltage()
    {
        compClass = typeof(CompHighVoltage);
    }
}