using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul
{
    public class DamageDealerMonoObject : MonoBehaviour
    {
        DamageData damageData;

        private void Awake() => InitializeDamage();

        void InitializeDamage()
        {
            damageData = new DamageData(DamageType.Chaos, 30f, false, 0, this.transform);
        }

        void OnTriggerEnter(Collider other)
        {
            IDamagable damagable;
            if (other.gameObject.TryGetComponent<IDamagable>(out damagable))
                damagable.TakeDamage(damageData);
        }
    }
}
