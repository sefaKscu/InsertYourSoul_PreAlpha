namespace InsertYourSoul
{
    public enum ModType
    {
        FlatAdditive,
        PercentAdditive,
        PercentMultiplicative
    }

    [System.Serializable]
    public class StatModifier
    {
        public StatType StatType;
        public ModType Type;
        public int Order;
        public float Value;
        public object Source;
    }

    [System.Serializable]
    public class DamageStatModifier : StatModifier
    {
        public DamageType DamageType;
    }
}
