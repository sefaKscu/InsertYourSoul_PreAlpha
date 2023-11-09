using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    public class DamageDealer : MonoBehaviour
    {
        protected DamageData damage;
        protected LayerMask whatIsTarget;
        protected LayerMask whatIsNotTarget;

        protected HashSet<IDamagable> DamagablesThatHit = new HashSet<IDamagable>();

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                if (DamagablesThatHit.Add(damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }
        }
    }
}
