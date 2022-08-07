using RimWorld;
using Verse;

namespace SR.DA.Component;

public class CompLayerExtension : ThingComp
{
    public CompProperties_LayerExtension Props => (CompProperties_LayerExtension)props;

    public override void PostDraw()
    {
        base.PostDraw();
        if (Props.gas == null || Props.gas.Count == 0)
        {
            return;
        }

        foreach (var currentGas in Props.gas)
        {
            currentGas.graphicData.Graphic
                .Draw(
                    GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size,
                        currentGas.altitudeLayer.AltitudeFor()), parent.Rotation, parent);
        }
    }
}