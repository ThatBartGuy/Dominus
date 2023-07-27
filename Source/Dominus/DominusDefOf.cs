using Verse;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[DefOf]
public static class DominusDefOf
{
    static DominusDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DominusDefOf));
    }

    public static HediffDef DM_Dominus;
}
