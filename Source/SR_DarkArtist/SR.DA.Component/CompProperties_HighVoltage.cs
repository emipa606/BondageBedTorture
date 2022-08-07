using Verse;

namespace SR.DA.Component;

public class CompProperties_HighVoltage : CompProperties
{
    [NoTranslate] public string commandDescKey = "SR_CommandDesignateHighVoltageDesc";

    [NoTranslate] public string commandLabelKey = "SR_CommandDesignateHighVoltageLabel";

    [NoTranslate] public string commandTexture = "UI/Commands/HighVoltage";

    public CompProperties_HighVoltage()
    {
        compClass = typeof(CompHighVoltage);
    }
}