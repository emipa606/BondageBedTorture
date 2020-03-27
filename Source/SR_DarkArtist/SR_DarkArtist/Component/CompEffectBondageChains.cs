﻿using System;
using RimWorld;
using Verse;
using SR.DA.Thing;

namespace SR.DA.Component
{
    public class CompEffectBondageChains : CompUseEffect
    {
        /// <summary>
        /// 作用效果 束缚
        /// </summary>
        /// <param name="usedBy"></param>
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            Building_BondageBed building_BondageBed = (Building_BondageBed)parent;
            building_BondageBed.SetOccupant(usedBy);//设置使用者
            HediffDef hediffChains = Hediff.HediffDefOf.SR_BondageChains;
            foreach (BodyPartRecord bpr in usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Arm))
            {
                usedBy.health.AddHediff(hediffChains, bpr, null, null);
            }
            foreach (BodyPartRecord bpr in usedBy.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Leg))
            {
                usedBy.health.AddHediff(hediffChains, bpr, null, null);
            }
        }
    }
}