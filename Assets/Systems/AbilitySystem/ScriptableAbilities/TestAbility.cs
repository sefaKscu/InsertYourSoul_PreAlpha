using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Ability System/Ability/Test Ability", fileName = "new_TestAbility")]
    public class TestAbility : Ability
    {
        public override void Activate(AbilityReferences _references)
        {
            base.Activate(_references);
        }
    }
}
