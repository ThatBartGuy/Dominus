﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Dominus
{
    public class Hediff_DominusLink : HediffWithComps
    {
        public Gene_Dominus owner;
        public List<Hediff> upgrades;

        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            owner.RemoveSlave(this);
        }

        public override void Tick()
        {
            base.Tick();
            
            if (!pawn.IsHashIntervalTick(60))
            {
                return;
            }

            if (!pawn.IsSlaveOfColony)
            {
                owner.RemoveSlave(this);
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            for (int i = upgrades.Count - 1; i >= 0; i--)
            {
                pawn.health.RemoveHediff(upgrades[i]);
            }
        }
    }
}
