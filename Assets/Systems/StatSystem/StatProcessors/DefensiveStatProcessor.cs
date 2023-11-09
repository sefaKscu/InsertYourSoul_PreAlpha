namespace InsertYourSoul.StatSystem
{
    [System.Serializable]
    public class DefensiveStatProcessor : PassiveStatProcessor
    {
        public DefensiveStatProcessor(Stat _baseStat, DamageType _defenceType) : base(_baseStat)
        {
            this.defenceType = _defenceType;
        }

        public DamageType DefenceType => defenceType;
        public Stat CappedStat
        {
            get
            {
                cappedStat = TotalStat;
                if (cappedStat.Value >= capValue)
                    cappedStat.Value = capValue;

                return cappedStat;
            }
        }

        protected DamageType defenceType;
        protected Stat cappedStat;
        protected float capValue = 75f;

        public override void AddModifier(StatModifier modifier)
        {
            base.AddModifier(modifier);
        }
    }
}
