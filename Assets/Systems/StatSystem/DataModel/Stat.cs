namespace InsertYourSoul.StatSystem
{
    /// <summary>
    /// This is the data type each passive stat holds and returns.
    /// It has a value and an StatType enum
    /// </summary>
    [System.Serializable]
    public struct Stat
    {
        public StatType Type;
        public float Value;

        public Stat(StatType type, float value)
        {
            Type = type;
            Value = value;
        }
    }
}
