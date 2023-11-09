namespace InsertYourSoul
{
    /// <summary>
    /// This is the data type damageStats holds and returns. 
    /// It has min-max value and a DamageType enum
    /// </summary>
    [System.Serializable]
    public struct BaseDamage
    {
        public DamageType Type;
        public float minValue;
        public float maxValue;
    }
}
