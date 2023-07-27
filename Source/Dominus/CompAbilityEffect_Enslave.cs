using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static RimWorld.RitualStage_InteractWithRole;

namespace Dominus
{
    public class CompAbilityEffect_Enslave : CompAbilityEffect
    {
        private new CompProperties_AbilityEnslave Props => props as CompProperties_AbilityEnslave;

        private Gene_Dominus cachedGene;

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

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!base.Valid(target, throwMessages))
            {
                return false;
            }

            if (!target.IsValid || target.Pawn == null || target.Pawn.health.hediffSet.HasHediff(Props.hediffDef))
            {
                return false;
            }

            if (Props.onlyDownedNonColonists)
            {
                if(!target.Pawn.IsColonist && !target.Pawn.IsSlaveOfColony && !target.Pawn.IsPrisonerOfColony && !target.Pawn.Downed)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (base.GizmoDisabled(out reason))
            {
                return true;
            }

            float stat = parent.pawn.GetStatValue(StatDefOf.PsychicSensitivity);

            if (stat / 0.25f <= Gene.slaves.Count)
            {
                reason = "{0} already has {1}/{2} slaves".Formatted(parent.pawn.LabelCap, (stat / 0.25f) + "", Gene.slaves.Count + "");
                return true;
            }

            return false;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Hediff_DominusLink hediff = HediffMaker.MakeHediff(Props.hediffDef, target.Pawn) as Hediff_DominusLink;
            target.Pawn.health.AddHediff(hediff);
            hediff.owner = Gene;
            Gene.AddSlave(hediff);
            GenGuest.EnslavePrisoner(parent.pawn, target.Pawn);
            Messages.Message("{0} has successfully dominated {1}.".Formatted(parent.pawn.LabelCap, target.Pawn.LabelCap), MessageTypeDefOf.PositiveEvent);
        }
    }

    public class CompProperties_AbilityEnslave : CompProperties_AbilityEffect
    {
        public HediffDef hediffDef;

        public bool onlyDownedNonColonists = true;

        public CompProperties_AbilityEnslave()
        {
            compClass = typeof(CompProperties_AbilityEnslave);
        }
    }
}
