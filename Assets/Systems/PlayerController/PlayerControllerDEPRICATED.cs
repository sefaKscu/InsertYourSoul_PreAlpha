using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControllerDEPRICATED : MonoBehaviour, IReciveInputPackage
    {
        #region References
        CharacterController characterController;
        Animator animator;
        private void SetReferences()
        {
            characterController = GetComponent<CharacterController>();
            characterController.center = new Vector3 (0f, characterController.height / 2f, 0f);
            animator = GetComponent<Animator>();
        }

        #endregion

        #region Populate in Inspector
        [Header("Speed")]
        [SerializeField, Range(1f, 20f)] float walkSpeedConstant = 2f;
        [SerializeField, Range(1f, 20f)] float runSpeedConstant = 5f;
        [SerializeField, Range(1f, 20f)] float sprintSpeedConstant = 8f;
        [SerializeField] float defaultDeceleration = 16f;

        [Header("Gravity Settings")]
        [SerializeField, Range(0f, 30f)]
        float gravity = 9.8f;
        [SerializeField, Range(0.1f, 1f)]
        float groundedGravity = 0.5f;

        [Header("Other Setting")]
        [SerializeField, Range(0.02f, 20f)]
        float rotationFactorPerFrame = 10f;
        #endregion

        #region Input Data
        InputStreamDataPackage inputData;
        public void CacheInputs(InputStreamDataPackage package, bool isAlive)
        {
            this.inputData = package;
        }
        #endregion

        #region MonoBehaviour
        private void Awake() => SetReferences();

        private void FixedUpdate()
        {
            // This methods are order-sensitive
            HandleRotation();
            HandleGravity();
            HandleMovement();
            HandleAnimation();
        }
        #endregion

        #region Animation Handling
        // This Section is going to seperated when refactoring
        private void HandleAnimation()
        {
            if (animator == null)
                return;
            animator.SetBool("isMoving", IsMoving);
            animator.SetFloat("Velocity", currentMovementSpeed, 0.1f, Time.fixedDeltaTime);
            animator.SetBool("isAiming", inputData.IsAiming || inputData.IsAimingWithMouse);
        }
        #endregion

        #region Rotation Handling
        Vector3 positionToLookAt;
        Quaternion currentRotation;
        Quaternion targetRotation;
        private void HandleRotation()
        {
            if (animator != null)
            {
                if (animator.GetBool("isMoving") && animator.GetBool("isAiming"))
                    return;
            }

            // Cache normalized aim
            positionToLookAt = inputData.AimDirectionRaw.normalized;

            // Check if there is rotation input
            if (positionToLookAt == Vector3.zero)
                return;

            // Begin & End values of rotation
            currentRotation = transform.rotation;
            targetRotation = Quaternion.LookRotation(positionToLookAt);


            // Interpolate Rotation
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        #endregion

        #region Gravity Handling
        private float gravityAxis;
        private float appliedGravityAxis;
        private void HandleGravity()
        {
            Debug.Log(characterController.isGrounded);
            if (characterController.isGrounded)
            {
                // applying small static gravity just to keep player grounded.
                appliedGravityAxis = -groundedGravity;
            }
            else
            {
                FallToGround();
            }
        }

        private void FallToGround()
        {
            // Velocity Verlet Integration (This makes the gravity framerate-independent)
            float previousVelocityY = gravityAxis;
            gravityAxis -= gravity;
            appliedGravityAxis = (previousVelocityY + gravityAxis) * 0.5f;
        }
        #endregion

        #region Movement Handling

        protected bool IsMoving => CharacterVelocity > 0f;
        protected float CharacterVelocity
        {
            get
            {
                Vector3 velocityVector = characterController.velocity;
                velocityVector.y = 0f;

                return velocityVector.magnitude;
            }
        }

        // Private values which movement calculations processed on
        private Vector3 cachedMovementInput;
        private float currentMovementSpeed;
        private float MovementSpeedConstant
        {
            get
            {
                if (inputData.IsSprintPressed)
                    return sprintSpeedConstant;
                else if (inputData.IsRunToggled)
                    return runSpeedConstant;

                return walkSpeedConstant;
            }
        }

        private void HandleMovement()
        {
            CacheMovementInput();
            HandleAcceleration();
            Move(cachedMovementInput, appliedGravityAxis, currentMovementSpeed);
            HandleAcceleration();
        }
        private void CacheMovementInput()
        {
            if (!inputData.IsMovementPressed)
                return;

            // cache input if moving
            cachedMovementInput.x = inputData.MovementDirection.x;
            cachedMovementInput.z = inputData.MovementDirection.z;
        }

        /// <summary>
        /// This method designed to be called before and after the movement method calls.
        /// It applies half of accelaration each time it called.
        /// This approach makes acceleration frame-rate independent.
        /// </summary>
        private void HandleAcceleration()
        {
            // start movement with a constant speed
            if (inputData.IsMovementPressed && (currentMovementSpeed < 2f || !IsMoving))
                currentMovementSpeed = 2f;

            // Accelerate or Decelerate
            if (currentMovementSpeed > 0f)
                currentMovementSpeed += GetAcceleration() * Time.fixedDeltaTime * 0.5f;

            // speed can't go lower than zero
            else if (currentMovementSpeed < 0f)
                currentMovementSpeed = 0f;
        }
        private void Move(Vector3 _movementVector, float _gravityAxis, float _movementSpeed)
        {
            Vector3 _vectorToApply;
            // Apply values to movement vector
            _vectorToApply.x = _movementVector.x * _movementSpeed;
            _vectorToApply.y = _gravityAxis;
            _vectorToApply.z = _movementVector.z * _movementSpeed;
            characterController.Move(_vectorToApply * Time.fixedDeltaTime);
        }

        private float GetAcceleration()
        {
            if (inputData.IsMovementPressed && currentMovementSpeed < MovementSpeedConstant)
                return MovementSpeedConstant;
            else if (inputData.IsMovementPressed && currentMovementSpeed > MovementSpeedConstant)
                return -MovementSpeedConstant * 2f;
            else if (!inputData.IsMovementPressed && currentMovementSpeed > 0f)
                return -defaultDeceleration;
            else
                return 0f;
        }
        #endregion
    }
}
