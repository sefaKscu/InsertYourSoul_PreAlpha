using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul
{
    public class CombatHandler : MonoBehaviour
    {
        public List<Attack> ComboAttacks;
        public int ComboCount;
        public float ComboWindow;
        public float ComboCooldown;
        public float CurrentClipLength => animator.GetCurrentAnimatorClipInfo(0).Length;
        public float CurrentStateSpeed => animator.GetCurrentAnimatorStateInfo(0).speed;

        private const float allowNextAttackTreshold = 0.8f;

        Animator animator;

        private void Awake()
        {
            if (animator == null) 
                animator = GetComponent<Animator>();
        }

        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }

            if (ComboCount > 0)
                ComboWindow -= Time.deltaTime;

            if (ComboWindow <= 0)
            {
                ComboWindow = 0;
                EndCombo();
            }
        }

        private void Attack()
        {

            if (CheckAnimatorState())
            {
                animator.runtimeAnimatorController = ComboAttacks[ComboCount].AnimatorOverrider;
                animator.SetTrigger("Attack");
                ComboCount++;
                ComboWindow += (CurrentClipLength / CurrentStateSpeed) + ComboCooldown;

                if (ComboCount == ComboAttacks.Count)
                    EndCombo();
            }
        }

        private bool CheckAnimatorState()
        {
            bool isCurrentStateIsAttackState = CheckAttackState();
            float tresholdOfCurrentState = allowNextAttackTreshold * (CurrentClipLength / CurrentStateSpeed);
            bool isInComboWindow = animator.GetCurrentAnimatorStateInfo(0).normalizedTime > tresholdOfCurrentState;
            return !isCurrentStateIsAttackState || isCurrentStateIsAttackState && isInComboWindow;
        }

        private bool CheckAttackState()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }

        private void EndCombo()
        {
            ComboCount = 0;
            ComboWindow = 0;
        }
    }
}
