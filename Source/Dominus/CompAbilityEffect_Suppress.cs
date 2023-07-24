using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Dominus
{
    public class CompAbilityEffect_Suppress : CompAbilityEffect
    {
        private new CompProperties_AbilitySuppress Props => props as CompProperties_AbilitySuppress;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Gene_Dominus gene = parent.pawn.genes.GetFirstGeneOfType<Gene_Dominus>();

            if (gene == null)
            {
                return;
            }

            for (int i = gene.slaves.Count - 1; i >= 0; i--)
            {
                Need_Suppression need = gene.slaves[i].pawn.needs.TryGetNeed<Need_Suppression>();

                if (need != null)
                {
                    need.CurLevel += Props.amount;
                }
            }
        }
    }

    public class CompProperties_AbilitySuppress : CompProperties_AbilityEffect
    {
        public float amount;

        public CompProperties_AbilitySuppress()
        {
            compClass = typeof(CompAbilityEffect_Suppress);
        }
    }
}
