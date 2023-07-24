using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Dominus
{
    public class ThinkNode_DominusRebellionCheck : ThinkNode_Conditional
    {
        public override bool Satisfied(Pawn pawn)
        {
            Gene_Dominus gene = pawn.genes.GetFirstGeneOfType<Gene_Dominus>();

            if (gene == null)
            {
                return false;
            }

            List<float> rebellionMTBs = new List<float>();

            for (int i = gene.slaves.Count - 1; i >= 0; i--)
            {
                float slaveMTB = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(gene.slaves[i].pawn);

                if (slaveMTB != -1f)
                {
                    rebellionMTBs.Add(slaveMTB);
                }
            }

            float totalMTB = Rand.CombineMTBs(rebellionMTBs);

            if (totalMTB < 20f && totalMTB > 0f)
            {
                return true;
            }

            return false;
        }
    }
}
