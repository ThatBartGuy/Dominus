using RimWorld;
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
        public List<Hediff> upgrades = new List<Hediff>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref owner, "owner");
            Scribe_Collections.Look(ref upgrades, "upgrades", LookMode.Reference);
        }

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
            owner.HandleSlaveRemove(this);
        }
    }
}
