using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    public class Ability : ScriptableObject
    {
        [Header("Definition")]
        [SerializeField] protected string abilityName;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected GameObject AbilityPrefab;
        [TextArea]
        [SerializeField] protected string description;
        [Header("Cost")]
        [SerializeField] protected float activeSeconds;
        [SerializeField] protected float cooldownSeconds;
        [SerializeField] protected float manaCost;


        public Sprite Icon => icon;
        public string Name => abilityName;
        public string Description => description;

        // These values will be passiveStats
        public float ActiveSeconds => activeSeconds;
        public float CooldownSeconds => cooldownSeconds;
        public float ManaCost => manaCost;


        protected AbilityReferences references;

        public virtual void Activate(AbilityReferences _references)
        {
            references = _references;
            Debug.Log(Name + " used.");
        }
    }

    [System.Serializable]
    public class AbilityReferences
    {
        public Transform Source;
        public Transform ProjectileExit;
        public AimDataPackage AimData;

        public AbilityReferences(Transform source, Transform projectileExit)
        {
            Source = source;
            ProjectileExit = projectileExit;
        }
    }
}
