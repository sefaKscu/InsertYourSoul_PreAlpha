using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class RotationHandler : ITickable
    {
        private IPlayerController parent;
        public RotationHandler(IPlayerController parent)
        {
            this.parent = parent;
        }

        private Transform ParentTransform => parent.Model.Transform;
        private Animator ParentAnimator => parent.Model.Animator;
        private float RotationFactorPerFrame => parent.Model.RotationFactorPerFrame;
        private Vector3 PositionToLookAt => parent.InputData.AimDirectionRaw.normalized;

        public void Tick()
        {
            if (RotationLogic())
                HandleRotation();
        }

        private Quaternion currentRotation;
        private Quaternion targetRotation;

        private bool RotationLogic()
        {
            if (ParentAnimator.GetBool("isMoving") && ParentAnimator.GetBool("isAiming"))
                return false;
            if (PositionToLookAt == Vector3.zero)
                return false;

            return true;
        }
        private void HandleRotation()
        {

            // Begin & End values of rotation
            currentRotation = ParentTransform.rotation;
            targetRotation = Quaternion.LookRotation(PositionToLookAt);


            // Interpolate Rotation
            ParentTransform.rotation = Quaternion.Slerp(currentRotation, targetRotation, RotationFactorPerFrame * Time.deltaTime);
        }
    }
}
