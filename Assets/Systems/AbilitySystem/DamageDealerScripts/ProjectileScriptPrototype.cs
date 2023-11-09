using InsertYourSoul.AbilitySystem;
using UnityEditor;
using UnityEngine;

namespace InsertYourSoul
{
    public class ProjectileScriptPrototype : MonoBehaviour
    {
        Rigidbody spellBody;
        [SerializeField] float speed;
        [SerializeField] GameObject explosionPrefab;

        private AbilityReferences references;
        private Vector3 moveDirection;

        public void Initialize(AbilityReferences _abilityReferences)
        {
            references = _abilityReferences;
            moveDirection = _abilityReferences.AimData.TargetDirection;
        }

        private void Awake()
        {
            spellBody = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            spellBody.velocity = speed * Time.fixedDeltaTime * moveDirection;
        }

        private void OnTriggerEnter(Collider other)
        {
            spellBody.velocity = Vector3.zero;
            Destroy(this.gameObject, 0.1f);
            ParticleSystem _explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            _explosion.Play();
            Destroy(_explosion.gameObject, 1.2f);
        }
    }
}
