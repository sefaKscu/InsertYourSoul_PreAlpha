using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class AnimationHandler : ITickable
    {
        private IPlayerController parent;
        public AnimationHandler(IPlayerController parent)
        {
            this.parent = parent;
        }

        private Animator Animator => parent.Model.Animator;

        public void Tick()
        {
            HandleAnimation();
        }
        private void HandleAnimation()
        {
            if (Animator == null)
                return;
            Animator.SetBool("isMoving", parent.IsMoving);
            Animator.SetFloat("Velocity", parent.CharacterVelocity, 0.1f, Time.fixedDeltaTime);
            Animator.SetBool("isAiming", parent.InputData.IsAiming || parent.InputData.IsAimingWithMouse);
        }

    }
}
