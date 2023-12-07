using UnityEngine;

namespace InsertYourSoul
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Combat System/Attack", fileName = "new_Attack")]
    public class Attack : ScriptableObject
    {        
        public AnimatorOverrideController AnimatorOverrider;
        public DamageStat Damage;
    }
}
