using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class MovementHandler : ITickable
    {
        private IPlayerController parent;
        public MovementHandler(IPlayerController parent)
        {
            this.parent = parent;
        }


        // References
        private CharacterController ParentCharacterController => parent.Model.CharacterController;


        private float RunSpeedConstant => parent.Model.RunSpeedConstant;
        private Vector3 InputVector => parent.InputData.MovementDirection;
        private float GravityAxis => parent.GravityAxis;
        private bool IsMovementPressed => parent.InputData.IsMovementPressed;


        private bool IsMoving => CharacterVelocity > 0f;
        private float CharacterVelocity
        {
            get
            {
                Vector3 velocityVector = ParentCharacterController.velocity;
                velocityVector.y = 0f;

                return velocityVector.magnitude;
            }
        }
        private float MovementSpeedConstant => RunSpeedConstant;

        public Vector3 CachedInputVector 
        {
            get 
            {
                if (IsMovementPressed && !parent.IsCasting)
                {
                    cachedInputVector = InputVector;
                }
                else
                {
                    cachedInputVector = Vector3.zero;
                }
                return cachedInputVector; 
            } 
        }

        // LocalValues
        private Vector3 cachedInputVector;


        public void Tick()
        {
            Move(CachedInputVector, GravityAxis, MovementSpeedConstant);
        }

        /// <summary>
        /// This method designed to be called before and after the movement method calls.
        /// It applies half of accelaration each time it called.
        /// This approach makes acceleration frame-rate independent.
        /// </summary>
        private void Move(Vector3 _movementVector, float _gravityAxis, float _movementSpeed)
        {
            Vector3 _vectorToApply = Vector3.zero;
            // Apply values to movement vector
            _vectorToApply.x = _movementVector.x * _movementSpeed;
            _vectorToApply.y = _gravityAxis;
            _vectorToApply.z = _movementVector.z * _movementSpeed;
            ParentCharacterController.Move(_vectorToApply * Time.fixedDeltaTime);
        }
    }
}
