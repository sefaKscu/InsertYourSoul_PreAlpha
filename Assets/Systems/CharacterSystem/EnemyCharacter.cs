using InsertYourSoul.CharacterSystem;
using InsertYourSoul.StatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul
{
    public class EnemyCharacter : MonoBehaviour, IDamagable
    {
        public CharacterTemplate characterTemplate;

        #region Stats
        private List<StatProcessor> StatProcessors = new List<StatProcessor>();

        public float CurrentLife => life.CurrentValue;
        public float CurrentMana => mana.CurrentValue;
        public bool IsAlive => isAlive;

        private bool isAlive = true;

        // Vitals
        private ActiveStatProcessor life;
        private ActiveStatProcessor mana;
        // Regen
        private PassiveStatProcessor lifeRegen;
        private PassiveStatProcessor manaRegen;
        // Defence
        private DefensiveStatProcessor armor;
        private DefensiveStatProcessor radiantResistance;
        private DefensiveStatProcessor chaosResistance;
        // Casting
        private StatProcessor reducedCastTime;
        private StatProcessor reducedManaCost;
        // Damage
        private StatProcessor physicalDamage;
        private StatProcessor radiantDamage;
        private StatProcessor chaosDamage;
        private void InitializeStatProcessors()
        {
            // Vitals
            life = new ActiveStatProcessor(characterTemplate.Life); StatProcessors.Add(life);
            mana = new ActiveStatProcessor(characterTemplate.Mana); StatProcessors.Add(mana);
            // Regen
            lifeRegen = new PassiveStatProcessor(characterTemplate.LifeRegen); StatProcessors.Add(lifeRegen);
            manaRegen = new PassiveStatProcessor(characterTemplate.ManaRegen); StatProcessors.Add(manaRegen);
            // Mitigation
            armor = new DefensiveStatProcessor(new Stat(StatType.Armor, 0f), DamageType.Physical); StatProcessors.Add(armor);
            radiantResistance = new DefensiveStatProcessor(new Stat(StatType.RadiantResistance, 0f), DamageType.Radiant); StatProcessors.Add(radiantResistance);
            chaosResistance = new DefensiveStatProcessor(new Stat(StatType.ChaosResistance, 0f), DamageType.Chaos); StatProcessors.Add(chaosResistance);
            // Cast
            reducedCastTime = new StatProcessor(StatType.ReducedCastTime); StatProcessors.Add(reducedCastTime);
            reducedManaCost = new StatProcessor(StatType.ReducedManaCost); StatProcessors.Add(reducedManaCost);
            // Damage
            physicalDamage = new StatProcessor(StatType.PhysicalDamage); StatProcessors.Add(physicalDamage);
            radiantDamage = new StatProcessor(StatType.RadiantDamage); StatProcessors.Add(radiantDamage);
            chaosDamage = new StatProcessor(StatType.ChaosDamage); StatProcessors.Add(chaosDamage);
        }


        public void TakeDamage(DamageData damage)
        {
            Debug.Log(name + " took damage");
            float _resultingDamageAmount;
            DamageData _resultingDamageData = damage;
            switch (damage.Type)
            {
                case DamageType.Physical:
                    _resultingDamageAmount = (damage.Value / 100f) * (100f - armor.CappedStat.Value);
                    break;
                case DamageType.Radiant:
                    _resultingDamageAmount = (damage.Value / 100f) * (100f - radiantResistance.CappedStat.Value);
                    break;
                case DamageType.Chaos:
                    _resultingDamageAmount = (damage.Value / 100f) * (100f - chaosResistance.CappedStat.Value);
                    break;
                default:
                    _resultingDamageAmount = damage.Value;
                    break;
            }

            _resultingDamageData.Value = _resultingDamageAmount;
            isAlive = life.LooseValue(_resultingDamageAmount);
        }
        public void HealLife(float amount)
        {
            life.GainValue(amount);
        }
        public bool SpendMana(float amount)
        {
            return mana.LooseValue(amount);
        }
        public void HealMana(float amount)
        {
            mana.GainValue(amount);
        }
        public void Regenerate()
        {
            if (isAlive && !life.OnFullAmount)
                life.GainValue(lifeRegen.TotalStat.Value * Time.deltaTime);
            if (isAlive && !mana.OnFullAmount)
                mana.GainValue(manaRegen.TotalStat.Value * Time.deltaTime);
        }
        #endregion

        #region MonoBehaviour
        private void Awake() => InitializeStatProcessors();

        private void Update() => Regenerate();
        #endregion
    }
}
