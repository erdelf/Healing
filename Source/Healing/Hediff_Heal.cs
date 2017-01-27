using System.Linq;
using Verse;

namespace Healing
{
    public class Hediff_Heal : Hediff
    {
        

        public override void PostMake()
        {
            base.PostMake();

            Log.Message("hey");

            Hediff_HealDef def = this.def as Hediff_HealDef;

            if (pawn != null)
            {
                Log.Message("pawn there");
                int counter = 0;
                if (def.maxHealedInjuries != 0)
                {
                    Log.Message("heal injuries");

                    counter = def.maxHealedInjuries;
                    foreach (Hediff_Injury current in pawn.health.hediffSet.GetHediffs<Hediff_Injury>())
                    {
                        bool heal = false;
                        Log.Message(current.Label);
                        if (counter != 0)
                        {
                            Log.Message("counter");
                            if (current.IsOld() && def.healsOld)
                                heal = true;
                            if (current.CanHealNaturally() && def.healsNaturalHealing)
                                heal = true;
                            if (current.CanHealFromTending() && def.healsTendableHealing)
                                heal = true;
                            if (current.comps.Any((HediffComp x) => x.props is HediffCompProperties_Immunizable) == def.healsInfections)
                                heal = true;
                            if (heal)
                            {
                                current.Heal(def.severityHeal);
                                counter--;
                            }
                        }
                    }
                }
                if(def.maxRegenerateLimbs != 0)
                {
                    counter = def.maxRegenerateLimbs;

                    while(pawn.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0)
                    {
                        if (counter == 0)
                            break;
                        pawn.health.RestorePart(pawn.health.hediffSet.GetMissingPartsCommonAncestors()[0].Part);
                        counter--;
                    }
                        
                }
            }
            this.Heal(this.severityInt * 2);
        }
    }

    public class Hediff_HealDef : HediffDef
    {
        public int maxHealedInjuries = -1;
        public bool healsOld = false;
        public bool healsNaturalHealing = false;
        public bool healsTendableHealing = false;
        public bool healsInfections = false;
        public int maxRegenerateLimbs = 0;
        public float severityHeal = 0f;
    }
}
