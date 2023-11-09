using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul.StatSystem
{
    /// <summary>
    /// Holds stat modifiers and caches them into values.
    /// Applies these values when you pass in a stat.
    /// </summary>
    [System.Serializable]
    public class StatProcessor
    {
        public StatProcessor(StatType _statType)
        {
            this.statType = _statType;
            Modifiers = new List<StatModifier>();
        }
        public bool IsDirty => isModifierValuesDirty;
        public StatType StatType => statType;



        protected StatType statType;
        protected List<StatModifier> Modifiers;

        public virtual void AddModifier(StatModifier modifier)
        {
            if (modifier.StatType != statType)
                return;
            Modifiers.Add(modifier);
            isModifierValuesDirty = true;
        }
        public void RemoveModifier(object _source)
        {
            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                if (Modifiers[i].Source == _source)
                    Modifiers.RemoveAt(i);
            }
            isModifierValuesDirty = true;
            DistrubuteAndCacheModifierValues();
        }

        // Modifier Values
        protected float flatAdditive;
        protected float percentIncreasive;
        protected float percentMultiplicitive;
        protected bool isModifierValuesDirty = true;
        protected void DistrubuteAndCacheModifierValues()
        {
            if (Modifiers.Count <= 0)
            {
                flatAdditive = 0f;
                percentIncreasive = 0f;
                percentMultiplicitive = 0f;
                return;
            }

            foreach (StatModifier modifier in Modifiers)
            {
                switch (modifier.Type)
                {
                    case ModType.FlatAdditive:
                        flatAdditive += modifier.Value;
                        break;
                    case ModType.PercentAdditive:
                        percentIncreasive += modifier.Value;
                        break;
                    case ModType.PercentMultiplicative:
                        percentMultiplicitive += modifier.Value;
                        break;
                }
            }
        }
        public Stat ProcessStat(Stat baseStat)
        {
            Stat _result = baseStat;

            if(isModifierValuesDirty)
            {
                DistrubuteAndCacheModifierValues();
                isModifierValuesDirty = false;
            }

            _result.Value += flatAdditive;
            _result.Value *= 1f + (percentIncreasive / 100f);
            _result.Value *= 1f + (percentMultiplicitive / 100f);

            return _result;
        }
    }
}
