using UnityEngine;

namespace InsertYourSoul.TargettingSystem
{
    public class AimIndicator : MonoBehaviour, IReciveInputPackage, IProvideAimData
    {
        [SerializeField] float aimSpeed = 10f;
        [SerializeField] float aimWithMouseSpeed = 40f;
        [SerializeField] Transform indicator;
        [SerializeField] Transform spellExit;
        [SerializeField] Transform parentTransform;
        [SerializeField] bool snapToTarget;

        public bool HasValidTarget => validTarget != null;
        public AimDataPackage GetAimData => dataPackage;


        private AimDataPackage dataPackage;
        public void HandleData()
        {
            if(HasValidTarget)
            {
                dataPackage.TargetPosition = validTarget.position;
                dataPackage.TargetDirection = (validTarget.position - spellExit.position).normalized;
            }
            else if(!HasValidTarget)
            {
                dataPackage.TargetPosition = transform.position;
                dataPackage.TargetDirection = spellExit.forward;
            }
        }

        InputStreamDataPackage inputs;
        public void CacheInputs(InputStreamDataPackage package, bool isAlive)
        {
            inputs = package;
        }


        private Transform validTarget;
        private Vector3 defaultIndicatorPosition = new Vector3(0f, 0.01f, 0f);

        private void Awake()
        {
            parentTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            HandleIndicatorActivation();
            HandleTargetterMovement();
            HandleData();
        }

        private void HandleIndicatorActivation()
        {
            if (!inputs.IsAiming && !inputs.IsAimingWithMouse)
            {
                DeactivateIndicator();
                return;
            }

            ActivateIndicator();
        }
        private void HandleTargetterMovement()
        {
            if (inputs.IsAimingWithMouse)
            {
                transform.Translate(inputs.AimDirectionRaw * Time.fixedDeltaTime * aimWithMouseSpeed);
                HandleSnapToTarget();
                return;
            }
            else if (inputs.IsAiming)
            {
                transform.Translate(inputs.AimDirectionRaw.normalized * Time.fixedDeltaTime * aimSpeed);
                HandleSnapToTarget();
                return;
            }
            else
            {
                DeactivateIndicator();
                return;
            }
        }
        private void HandleSnapToTarget()
        {
            if (!snapToTarget)
                return;

            if (validTarget != null)
                indicator.transform.position = new Vector3(validTarget.position.x, 0.01f, validTarget.position.z);

            else if (validTarget == null)
                indicator.transform.localPosition = defaultIndicatorPosition;
        }
        private void ActivateIndicator()
        {
            if (!indicator.gameObject.activeSelf)
            {
                indicator.gameObject.SetActive(true);
            }
        }
        private void DeactivateIndicator()
        {
            transform.position = parentTransform.position;
            if (indicator.gameObject.activeSelf)
            {
                indicator.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            validTarget = other.transform;
        }
        private void OnTriggerExit(Collider other)
        {
            validTarget = null;
        }

    }
}
