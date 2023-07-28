using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace Dominus
{
    public class HediffComp_DominusUpgrade : HediffComp
    {
        private HediffCompProperties_DominusUpgrade Props => props as HediffCompProperties_DominusUpgrade;

        private Gene_Dominus cachedGene;
        private float cachedSeverity;

        public Gene_Dominus Gene
        {
            get
            {
                if (cachedGene == null)
                {
                    cachedGene = parent.pawn.genes.GetFirstGeneOfType<Gene_Dominus>();
                }

                return cachedGene;
            }
        }

        public HediffDef UpgradeHediff
        {
            get
            {
                return Props.upgradeHediff;
            }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            Gene.AddUpgrade(this);
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            Gene.RemoveUpgrade(this);
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (cachedSeverity != parent.Severity)
            {
                Log.Message("1");
                cachedSeverity = parent.Severity;
                Gene.RefreshSeverity(this);
            }
        }
    }

    public class HediffCompProperties_DominusUpgrade : HediffCompProperties
    {
        public HediffDef upgradeHediff;

        public HediffCompProperties_DominusUpgrade()
        {
            compClass = typeof(HediffComp_DominusUpgrade);
        }
    }
}
