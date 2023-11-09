using InsertYourSoul.ItemSystem;
using InsertYourSoul.StatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InsertYourSoul.CharacterSystem
{
    public class Character : MonoBehaviour, ICanEquip, IDamagable, ICharacter
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
        // Misc
        private PassiveStatProcessor moveSpeed;
        private PassiveStatProcessor lightRadius;
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
            // Misc
            moveSpeed = new PassiveStatProcessor(characterTemplate.MoveSpeed); StatProcessors.Add(moveSpeed);
            lightRadius = new PassiveStatProcessor(characterTemplate.LightRadius); StatProcessors.Add(lightRadius);
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

            Debugger("Stats Initialized");
        }


        public void TakeDamage(DamageData damage)
        {
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
            Debugger(_resultingDamageData.Source.gameObject.name + " dealt " + _resultingDamageAmount + " to " + this.gameObject.name);
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

        #region SO Data
        [Header("StatDataReferences")]
        [SerializeField] ActiveStatDataSO playerLifeSO;
        [SerializeField] ActiveStatDataSO playerManaSO;
        [SerializeField] CharacterStatsSO characterStatsSO;
        private void UpdateScriptableStatData()
        {
            if (playerLifeSO != null)
                playerLifeSO.StatData = life.StatData;
            if (playerManaSO != null)
                playerManaSO.StatData = mana.StatData;
            if (characterStatsSO != null)
                characterStatsSO.CacheData
                    (life.TotalStat, mana.TotalStat,
                     lifeRegen.TotalStat, manaRegen.TotalStat,
                     armor.CappedStat, radiantResistance.CappedStat, chaosResistance.CappedStat,
                     armor.TotalStat, radiantResistance.TotalStat, chaosResistance.TotalStat,
                     moveSpeed.TotalStat, lightRadius.TotalStat);
        }
        #endregion

        #region MonoBehaviour
        private void Awake() => InitializeStatProcessors();


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                testItem.Equip(this);
            if (Input.GetKeyDown(KeyCode.O))
                testItem.UnEquip(this);
            if (Input.GetKeyDown(KeyCode.Alpha0))
                this.TakeDamage(new DamageData(DamageType.Physical, 20f, false, 0f, this.transform));
            if (Input.GetKeyDown(KeyCode.Alpha9))
                this.HealLife(5f);
            Regenerate();
            UpdateScriptableStatData();
        }
        #endregion

        #region Equipment
        public void EquipItem(EquippableItem _item)
        {
            foreach (StatModifier modifier in _item.ModifierList)
            {
                modifier.Source = _item;
                foreach (StatProcessor _stat in StatProcessors.Where(_stat => _stat.StatType == modifier.StatType))
                {
                    _stat.AddModifier(modifier);
                }
            }
        }

        public void UnEquipItem(EquippableItem _item)
        {
            foreach (StatProcessor _stat in StatProcessors)
            {
                _stat.RemoveModifier(_item);
            }
        }
        #endregion

        #region Debugger
        [Header("Testing")]
        public EquippableItem testItem;
        [SerializeField] private bool isDebugging;
        public void Debugger(string message)
        {
            if (!isDebugging)
                return;

            Debug.Log(message);
        }
        #endregion
    }
}
