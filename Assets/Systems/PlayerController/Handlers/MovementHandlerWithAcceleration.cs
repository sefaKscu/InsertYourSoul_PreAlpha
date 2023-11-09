using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class MovementHandlerWithAcceleration : ITickable
    {
        private IPlayerController parent;
        public MovementHandlerWithAcceleration(IPlayerController parent)
        {
            this.parent = parent;
        }


        // References
        private CharacterController ParentCharacterController => parent.Model.CharacterController;

        private float SprintSpeedConstant => parent.Model.SprintSpeedConstant;
        private float RunSpeedConstant => parent.Model.RunSpeedConstant;
        private float WalkSpeedConstant => parent.Model.WalkSpeedConstant;
        private float DefaultDeceleration => parent.Model.DefaultDeceleration;
        private Vector3 InputVector => parent.InputData.MovementDirection;
        private float GravityAxis => parent.GravityAxis;
        private bool IsMovementPressed => parent.InputData.IsMovementPressed;
        private bool IsSprintPressed => parent.InputData.IsSprintPressed;
        private bool IsRunToggled => parent.InputData.IsRunToggled;


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
        private float Acceleration
        {
            get
            {
                if (IsMovementPressed && currentMovementSpeed < MovementSpeedConstant)
                    return MovementSpeedConstant;
                else if (IsMovementPressed && currentMovementSpeed > MovementSpeedConstant)
                    return -MovementSpeedConstant * 2f;
                else if (!IsMovementPressed && currentMovementSpeed > 0f)
                    return -DefaultDeceleration;
                else
                    return 0f;
            }
        }
        private float MovementSpeedConstant
        {
            get
            {
                if (IsSprintPressed)
                    return SprintSpeedConstant;
                else if (IsRunToggled)
                    return RunSpeedConstant;

                return WalkSpeedConstant;
            }
        }



        // LocalValues
        private Vector3 cachedInputVector;
        private float currentMovementSpeed;
        

        public void Tick()
        {
            CacheMovementInput();
            HandleAcceleration();
            Move(cachedInputVector, GravityAxis, currentMovementSpeed);
            HandleAcceleration();
        }

        private void CacheMovementInput()
        {
            if (!IsMovementPressed)
                return;

            // cache input if moving
            cachedInputVector.x = InputVector.x;
            cachedInputVector.z = InputVector.z;
        }

        /// <summary>
        /// This method designed to be called before and after the movement method calls.
        /// It applies half of accelaration each time it called.
        /// This approach makes acceleration frame-rate independent.
        /// </summary>
        private void Move(Vector3 _movementVector, float _gravityAxis, float _movementSpeed)
        {
            Vector3 _vectorToApply;
            // Apply values to movement vector
            _vectorToApply.x = _movementVector.x * _movementSpeed;
            _vectorToApply.y = _gravityAxis;
            _vectorToApply.z = _movementVector.z * _movementSpeed;
            ParentCharacterController.Move(_vectorToApply * Time.fixedDeltaTime);
        }

        private void HandleAcceleration()
        {
            // start movement with a constant speed
            if (IsMovementPressed && (currentMovementSpeed < 2f || !IsMoving))
                currentMovementSpeed = 2f;

            // Accelerate or Decelerate
            if (currentMovementSpeed > 0f)
                currentMovementSpeed += Acceleration * Time.fixedDeltaTime * 0.5f;

            // speed can't go lower than zero
            else if (currentMovementSpeed < 0f)
                currentMovementSpeed = 0f;
        }

    }
}
