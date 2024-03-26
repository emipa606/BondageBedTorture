using System.Collections.Generic;
using Verse;

namespace SR.DA.Component;

public class CompProperties_LayerExtension : CompProperties
{
    public List<GA> gas;

    public CompProperties_LayerExtension()
    {
        compClass = typeof(CompLayerExtension);
    }

    public class GA
    {
        public AltitudeLayer altitudeLayer;
        public GraphicData graphicData;
    }
}