using UnityEngine;

namespace InsertYourSoul.StatSystem
{
    [CreateAssetMenu(fileName = "new_CharacterStatsData", menuName = "Insert Your Soul/Stat System/Data/Character Stats Data")]
    public class CharacterStatsSO : ScriptableObject
    {
        public CharacterStatsViewData CharacterStats;

        // Overloads of this method is too many for clean code.
        // Should refactor this
        public void CacheData(Stat life, Stat mana, Stat lifeRegen, Stat manaRegen, Stat armor, Stat armorUncapped, Stat radiantResistance, Stat radiantResistanceUncapped, Stat chaosResistance, Stat chaosResistanceUncapped, Stat movementSpeed, Stat lightRadius)
        {
            CharacterStats.Life = life;
            CharacterStats.Mana = mana;
            CharacterStats.LifeRegen = lifeRegen;
            CharacterStats.ManaRegen = manaRegen;
            CharacterStats.Armor = armor;
            CharacterStats.ArmorUncapped = armorUncapped;
            CharacterStats.RadiantResistance = radiantResistance;
            CharacterStats.RadiantResistanceUncapped = radiantResistanceUncapped;
            CharacterStats.ChaosResistance = chaosResistance;
            CharacterStats.ChaosResistanceUncapped = chaosResistanceUncapped;
            CharacterStats.MovementSpeed = movementSpeed;
            CharacterStats.LightRadius = lightRadius;
        }
    }
}
