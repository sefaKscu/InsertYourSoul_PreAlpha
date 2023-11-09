using UnityEngine;
using UnityEngine.InputSystem;

namespace InsertYourSoul.PlayerController
{
    public class InputHandler : MonoBehaviour, IProvideInputData
    {
        InputActions inputActions;

        IsometricRaycaster isoRaycaster;

        [Header("Mouse Raycast Options")]
        [SerializeField] Camera mainCam;
        [SerializeField] float orthographicAngle = 26.565f;
        [SerializeField] Transform raycastReference;

        private void SetupIsometricRaycaster()
        {
            if (isoRaycaster == null)
            {
                // If these references is not assigned in the inspector, assign them to default values
                if (mainCam == null)
                    mainCam = Camera.main;
                if (orthographicAngle == 0f)
                    orthographicAngle = mainCam.transform.rotation.eulerAngles.x;
                if (raycastReference == null)
                    raycastReference = this.transform;

                isoRaycaster = new IsometricRaycaster(mainCam, raycastReference, 300f, orthographicAngle);
            }
        }


        #region InputData
        // InputData
        public InputStreamDataPackage GetInputData
        {
            get
            {
                if (isInputDataDirty)
                    PackInputData();

                return inputData;
            }
        }
        private InputStreamDataPackage inputData;
        private bool isInputDataDirty;
        private void PackInputData()
        {
            inputData.IsAbility0ButtonDown = isAbility0ButtonDown;
            inputData.IsAbility1ButtonDown = isAbility1ButtonDown;
            inputData.IsAbility2ButtonDown = isAbility2ButtonDown;
            inputData.IsAbility3ButtonDown = isAbility3ButtonDown;
            inputData.IsAimingWithMouse = isAimingWithMouse;
            inputData.IsAiming = isAiming;
            inputData.IsMovementPressed = CanMove;
            inputData.IsSprintPressed = isSprinting;
            inputData.IsRunToggled = isRunning;
            inputData.MovementDirection = movementDirectionRaw.normalized;
            inputData.AimDirectionRaw = AimDirection;
            isInputDataDirty = false;
        }
        #endregion

        #region Private Values
        // Buttons
        private bool isAbility0ButtonDown;
        private bool isAbility1ButtonDown;
        private bool isAbility2ButtonDown;
        private bool isAbility3ButtonDown;
        private bool isAiming;
        private bool isAimingWithMouse;
        private bool isMoving;
        private bool isSprinting;
        private bool isRunning;

        // Move & Aim (Raw means vector is not a normalized vector)
        private Vector2 movementInputRaw;
        private Vector3 movementDirectionRaw;
        private Vector3 aimDirectionRaw;
        // Conditioned Property Getters
        private Vector3 AimDirection
        {
            get
            {
                if (isAiming || isAimingWithMouse)
                    return aimDirectionRaw;
                else
                    return movementDirectionRaw;
            }
        }
        private bool CanMove => isMoving && !isAiming && !isAimingWithMouse;
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            SetupIsometricRaycaster();
            SubscribeToInputsActions();
        }

        private void OnEnable() => inputActions.Enable();
        private void OnDisable() => inputActions.Disable();
        private void Update() => SetAimDirection();
        #endregion

        #region InputActionSubscribers
        private void SubscribeToInputsActions()
        {
            if (inputActions == null)
                inputActions = new InputActions();

            // Movement Actions
            inputActions.Default.Move.started += OnMove;
            inputActions.Default.Move.performed += OnMove;
            inputActions.Default.Move.canceled += OnMove;

            // Aim Actions (Gamepad)
            inputActions.Default.AimWithGamepad.started += OnAimGamepad;
            inputActions.Default.AimWithGamepad.performed += OnAimGamepad;
            inputActions.Default.AimWithGamepad.canceled += OnAimGamepad;
            // Aim Actions (Mouse)
            inputActions.Default.AimWithMouse.started += OnAimWithMouse;
            inputActions.Default.AimWithMouse.performed += OnAimWithMouse;
            inputActions.Default.AimWithMouse.canceled += OnAimWithMouse;

            // Ability0 Actions
            inputActions.Default.Ability0.started += OnAbility0;
            inputActions.Default.Ability0.performed += OnAbility0;
            inputActions.Default.Ability0.canceled += OnAbility0;
            // Ability1 Actions
            inputActions.Default.Ability1.started += OnAbility1;
            inputActions.Default.Ability1.performed += OnAbility1;
            inputActions.Default.Ability1.canceled += OnAbility1;
            // Ability2 Actions
            inputActions.Default.Ability2.started += OnAbility2;
            inputActions.Default.Ability2.performed += OnAbility2;
            inputActions.Default.Ability2.canceled += OnAbility2;
            // Ability0 Actions
            inputActions.Default.Ability3.started += OnAbility3;
            inputActions.Default.Ability3.performed += OnAbility3;
            inputActions.Default.Ability3.canceled += OnAbility3;
            // Sprint Actions
            inputActions.Default.Sprint.started += OnSprint;
            inputActions.Default.Sprint.performed += OnSprint;
            inputActions.Default.Sprint.canceled += OnSprint;
            // ToggleWalk Action
            inputActions.Default.ToggleRun.started += OnToggleRun;
        }


        // Vectoral Inputs
        private void OnMove(InputAction.CallbackContext _context)
        {
            movementInputRaw = _context.ReadValue<Vector2>();
            movementDirectionRaw.x = movementInputRaw.x;
            movementDirectionRaw.z = movementInputRaw.y;
            aimDirectionRaw = movementDirectionRaw;
            isMoving = movementInputRaw != Vector2.zero;
            isInputDataDirty = true;
        }

        // Aiming
        private void OnAimWithMouse(InputAction.CallbackContext context)
        {
            isAimingWithMouse = context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnAimGamepad(InputAction.CallbackContext _context)
        {
            isAiming = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        // Abilities
        private void OnAbility0(InputAction.CallbackContext _context)
        {
            isAbility0ButtonDown = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnAbility1(InputAction.CallbackContext _context)
        {
            isAbility1ButtonDown = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnAbility2(InputAction.CallbackContext _context)
        {
            isAbility2ButtonDown = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnAbility3(InputAction.CallbackContext _context)
        {
            isAbility3ButtonDown = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnSprint(InputAction.CallbackContext _context)
        {
            isSprinting = _context.ReadValueAsButton();
            isInputDataDirty = true;
        }
        private void OnToggleRun(InputAction.CallbackContext context)
        {
            isRunning = !isRunning;
            isInputDataDirty = true;
        }
        #endregion

        private void SetAimDirection()
        {
            if (isAimingWithMouse)
            {
                aimDirectionRaw = isoRaycaster.GetDirectionYZero();
                isInputDataDirty = true;
            }
            else if (!isAiming)
            {
                aimDirectionRaw = movementDirectionRaw;
            }
            DebugMousePointers(isoRaycaster);
        }

        #region Debugging
        // Debuggin
        [Header("Debugging Options")]
        [SerializeField] private bool isDebugging;
        [SerializeField] private GameObject hitPointSphere;
        [SerializeField] private GameObject playerHeightSphere;
        [SerializeField] private GameObject requiredHitPointSphere;

        private void DebugMousePointers(IsometricRaycaster _isoRaycaster)
        {
            if (hitPointSphere == null || playerHeightSphere == null || requiredHitPointSphere == null)
            {
                Debugger("Aim reference spheres has not been assigned.");
                return;
            }

            if (isDebugging && isAimingWithMouse)
            {
                hitPointSphere.SetActive(true);
                playerHeightSphere.SetActive(true);
                requiredHitPointSphere.SetActive(true);
                hitPointSphere.transform.position = _isoRaycaster.HitPoint;
                playerHeightSphere.transform.position = _isoRaycaster.PlayerHeight;
                requiredHitPointSphere.transform.position = _isoRaycaster.RequiredHitPoint;
            }
            else
            {
                hitPointSphere.SetActive(false);
                playerHeightSphere.SetActive(false);
                requiredHitPointSphere.SetActive(false);
            }

        }
        private void Debugger(string message)
        {
            if (!isDebugging)
                return;

            Debug.Log(message);
        }
        #endregion
    }
}
