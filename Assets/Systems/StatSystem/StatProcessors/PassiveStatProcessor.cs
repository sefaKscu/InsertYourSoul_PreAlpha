using System.Collections.Generic;

namespace InsertYourSoul.StatSystem
{
    [System.Serializable]
    public class PassiveStatProcessor : StatProcessor
    {
        public PassiveStatProcessor(Stat _baseStat) : base(_baseStat.Type)
        {
            baseStat = _baseStat;
            CalculateTotalStatValue();
        }

        public Stat TotalStat
        {
            get
            {                
                CalculateTotalStatValue();
                return totalStat;
            }
        }


        // Private Variables
        protected readonly Stat baseStat;
        protected Stat totalStat;

        protected void CalculateTotalStatValue()
        {
            if (!isModifierValuesDirty)
                return;

            totalStat = ProcessStat(baseStat);
        }
    }
}
