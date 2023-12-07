using UnityEngine;

namespace InsertYourSoul
{
    /// <summary>
    /// This is the data type ability passes to the "damage dealer" scripts. 
    /// It has all the necessary information about damage dealing.
    /// </summary>
    public class DamageData
    {
        public DamageData(DamageType _type, float _value, bool _isCrit, float _critMultiplier, Transform _source)
        {
            Type = _type;
            Value = _value;
            IsCrit = _isCrit;
            CritMultiplier = _critMultiplier;
            Source = _source;
        }
        public DamageType Type;
        public float Value;
        public bool IsCrit;
        public float CritMultiplier;
        public Transform Source;
    }

    /// <summary>
    /// Types of damage
    /// </summary>
    public enum DamageType
    {
        Physical,
        Radiant,
        Chaos
    }

    /// <summary>
    /// Basic damage stat. Has a min and max value. 
    /// </summary>
    [System.Serializable]
    public struct DamageStat
    {
        public DamageType Type;
        public float MinValue;
        public float MaxValue;
    }
}
