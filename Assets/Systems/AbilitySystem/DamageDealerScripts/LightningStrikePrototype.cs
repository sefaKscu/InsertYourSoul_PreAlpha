using InsertYourSoul.AbilitySystem;
using UnityEngine;

namespace InsertYourSoul
{
    public class LightningStrikePrototype : MonoBehaviour
    {
        ParticleSystem spellEffect;

        [SerializeField] float waitSecondsBetweenStrikes;
        [SerializeField] float maxIntensity;
        private float cooldownCounter;
        private int intensity = 1;

        private AbilityReferences references;

        public void Initialize(AbilityReferences _abilityReferences)
        {
            references = _abilityReferences;
        }

        private void Awake()
        {
            spellEffect = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (EmitLogic())
            {
                spellEffect.Emit(intensity);
                intensity++;

                cooldownCounter = waitSecondsBetweenStrikes;
            }
        }

        private bool EmitLogic()
        {
            if (intensity > maxIntensity)
            {
                Destroy(this.gameObject, 1f);
                return false;
            }
            if (spellEffect == null)
            {
                return false;
            }
            if (cooldownCounter > 0)
            {
                cooldownCounter -= Time.deltaTime;
                return false;
            }
            return true;
        }
    }
}
