using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class AnimationHandler : ITickable
    {

        // Animator Parameters
        private const string velocity = "Velocity";
        private const string isMoving = "isMoving";
        private const string isAiming = "isAiming";
        private const string isCasting = "isCasting";
        private const string isDead = "isDead";


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

            Animator.SetBool(isMoving, parent.IsMoving);
            Animator.SetFloat(velocity, parent.CharacterVelocity, 0.1f, Time.fixedDeltaTime);
            Animator.SetBool(isAiming, parent.InputData.IsAiming || parent.InputData.IsAimingWithMouse);
            Animator.SetBool(isCasting, parent.IsCasting);
        }

        public void DieAnimation()
        {
            if(!Animator.GetBool(isDead))
                Animator.SetBool(isDead, !parent.IsAlive);
        }
    }
}
