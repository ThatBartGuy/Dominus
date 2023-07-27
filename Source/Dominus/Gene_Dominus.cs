using Mono.Unix.Native;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Verse;

namespace Dominus
{
    public class Gene_Dominus : Gene
    {
        public List<Hediff_DominusLink> slaves;
        public List<HediffComp_DominusUpgrade> upgrades;

        public void AddSlave(Hediff_DominusLink slave)
        {
            slaves.Add(slave);

            for (int i = upgrades.Count - 1; i >= 0; i--)
            {
                Hediff hediff = HediffMaker.MakeHediff(upgrades[i].UpgradeHediff, slave.pawn);
                slaves[i].upgrades.Add(hediff);
                slave.pawn.health.AddHediff(hediff);
            }
        }

        public void RemoveSlave(Hediff_DominusLink slave)
        {
            slave.pawn.health.RemoveHediff(slave);
        }

        public void HandleSlaveRemove(Hediff_DominusLink slave)
        {
            slaves.Remove(slave);

            for (int i = slave.upgrades.Count - 1; i >= 0; i--)
            {
                slave.pawn.health.RemoveHediff(slave.upgrades[i]);
            }
        }

        public void AddUpgrade(HediffComp_DominusUpgrade upgrade)
        {
            upgrades.Add(upgrade);

            for (int i = slaves.Count - 1; i >= 0; i--)
            {
                Pawn slave = slaves[i].pawn;
                Hediff hediff = HediffMaker.MakeHediff(upgrade.UpgradeHediff, slave);
                slaves[i].upgrades.Add(hediff);
                slave.health.AddHediff(hediff);
            }
        }

        public void RemoveUpgrade(HediffComp_DominusUpgrade upgrade)
        {
            upgrades.Remove(upgrade);

            for (int i = slaves.Count - 1; i >= 0; i--)
            {
                Pawn slave = slaves[i].pawn;
                Hediff hediff = slaves[i].upgrades.First((Hediff x) => x.def == upgrade.UpgradeHediff);
                slaves[i].upgrades.Remove(hediff);
                slave.health.RemoveHediff(hediff);
            }
        }

        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();

            for (int i = slaves.Count - 1; i >= 0; i--)
            {
                slaves[i].pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "{0} died.".Formatted(pawn.LabelCap), forceWake: true);
                RemoveSlave(slaves[i]);
            }
        }

        public void RefreshSeverity(HediffComp_DominusUpgrade upgrade)
        {
            for (int i = slaves.Count - 1; i >= 0; i--)
            {
                Pawn slave = slaves[i].pawn;
                Hediff hediff = slave.health.hediffSet.GetFirstHediffOfDef(upgrade.UpgradeHediff);

                if (hediff != null)
                {
                    hediff.Severity = upgrade.parent.Severity;
                }
            }
        }
    }
}
