using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class MovementHandler : ITickable
    {
        private IPlayerController parent;
        public MovementHandler(IPlayerController parent)
        {
            this.parent = parent;
            dashState = DashState.Ready;
        }


        // References
        private CharacterController ParentCharacterController => parent.Model.CharacterController;

        public bool IsDashing => dashState == DashState.Active;

        private float RunSpeedConstant => parent.Model.RunSpeedConstant;
        private Vector3 InputVector => parent.InputData.MovementDirection;
        private float GravityAxisValue => parent.GravityAxisValue;
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
            Move(CachedInputVector, GravityAxisValue, MovementSpeedConstant);
            SwitchDashStates();
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
        private const float dashMultilier = 5f;
        private void Dash()
        {
            Move(parent.SelfTransform.forward * dashMultilier, GravityAxisValue, MovementSpeedConstant);
        }
        private DashState dashState;
        private const float activeTime = 0.15f;
        private float activeCounter;
        private const float cooldownTime = 1f;
        private float cooldownCounter;
        private void SwitchDashStates()
        {
            switch (dashState)
            {
                case DashState.Ready:
                    if (parent.InputData.IsSprintPressed)
                    {
                        dashState = DashState.Active;
                        activeCounter = activeTime;
                    }
                    break;
                case DashState.Active:
                    if (activeCounter > 0)
                    {
                        Dash();
                        activeCounter -= Time.fixedDeltaTime;
                        if (CharacterVelocity <= 1f)
                            SwitchToCooldown();
                    }
                    else
                    {
                        activeCounter = 0f;
                        SwitchToCooldown();
                    }
                    break;
                case DashState.Cooldown:
                    if (cooldownCounter > 0)
                    {
                        cooldownCounter -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        cooldownCounter = 0f;
                        dashState = DashState.Ready;
                    }
                    break;
                    
            }
            Debug.Log("Dash " + dashState);
        }

        private void SwitchToCooldown()
        {
            dashState = DashState.Cooldown;
            cooldownCounter = cooldownTime;
        }
    }
    public enum DashState
    {
        Ready,
        Active,
        Cooldown
    }
}
