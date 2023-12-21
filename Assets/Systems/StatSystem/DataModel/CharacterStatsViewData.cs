namespace InsertYourSoul.StatSystem
{
    [System.Serializable]
    public struct CharacterStatsViewData
    {
        public Stat Life;
        public Stat Mana;
        public Stat LifeRegen;
        public Stat ManaRegen;

        public Stat Armor;
        public Stat ArmorUncapped;
        public Stat RadiantResistance;
        public Stat RadiantResistanceUncapped;
        public Stat ChaosResistance;
        public Stat ChaosResistanceUncapped;

        public Stat MovementSpeed;
        public Stat LightRadius;
    }
}
