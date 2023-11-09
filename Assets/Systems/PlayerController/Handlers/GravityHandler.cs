using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class GravityHandler : ITickable
    {
        private IPlayerController parent;
        public GravityHandler(IPlayerController parent)
        {
            this.parent = parent;
        }
        public float GravityAxis => gravityVerletVelocity;



        private CharacterController CharacterController => parent.Model.CharacterController;
        private float Gravity => parent.Model.Gravity;
        private float GroundedGravity => parent.Model.GroundedGravity;


        private float gravityVelocity;
        private float gravityVerletVelocity;

        public void Tick()
        {
            HandleGravity();
        }

        private void HandleGravity()
        {
            if (CharacterController.isGrounded)
                ApplyStaticGravity();

            else
                FallToGround();
        }

        private void ApplyStaticGravity()
        {
            // applying small static gravity just to keep player grounded.
            gravityVerletVelocity = -GroundedGravity;
        }

        private void FallToGround()
        {
            // Velocity Verlet Integration (This makes the gravity framerate-independent)
            float previousVelocityY = gravityVelocity;
            gravityVelocity -= Gravity;
            gravityVerletVelocity = (previousVelocityY + gravityVelocity) * 0.5f;
        }
    }
}
