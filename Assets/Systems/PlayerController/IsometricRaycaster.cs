using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    public class IsometricRaycaster
    {
        public IsometricRaycaster(Camera mainCam, Transform raycastReference)
        {
            this.mainCam = mainCam;
            this.raycastReference = raycastReference;
            this.maxRaycastDistance = Mathf.Infinity;
            this.isometricAngleSin = TakeSinusOfEulerAngle(mainCam.transform.rotation.eulerAngles.x);
        }
        public IsometricRaycaster(Camera mainCam, Transform raycastReference, float maxRaycastDistance)
        {
            this.mainCam = mainCam;
            this.raycastReference = raycastReference;
            this.maxRaycastDistance = maxRaycastDistance;
            this.isometricAngleSin = TakeSinusOfEulerAngle(mainCam.transform.rotation.eulerAngles.x);
        }
        public IsometricRaycaster(Camera mainCam, Transform raycastReference, float maxRaycastDistance, float isometricAngle)
        {
            this.mainCam = mainCam;
            this.raycastReference = raycastReference;
            this.maxRaycastDistance = maxRaycastDistance;
            this.isometricAngleSin = TakeSinusOfEulerAngle(isometricAngle);
        }

        // Required References
        private readonly Camera mainCam;
        private readonly Transform raycastReference;
        private readonly float maxRaycastDistance;
        private readonly float isometricAngleSin;

        // Public Values For Debugging
        public Vector3 RequiredHitPoint => requiredHitPoint;
        public Vector3 PlayerHeight => playerHeight;
        public Vector3 HitPoint => hitPoint;

        // Pythagorian Raycast Triangle Points
        private Vector3 requiredHitPoint;
        private Vector3 playerHeight;
        private Vector3 hitPoint;




        public Vector3 GetHitPoint()
        {
            return requiredHitPoint;
        }
        public Vector3 GetDirection()
        {
            CastIsometricRay();
            return requiredHitPoint - raycastReference.position;
        }
        public Vector3 GetDirectionYZero()
        {
            CastIsometricRay();
            Vector3 _tmpVector = requiredHitPoint - raycastReference.position;
            return new Vector3(_tmpVector.x, 0f, _tmpVector.z);
        }

        private void CastIsometricRay()
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRaycastDistance))
            {
                requiredHitPoint = PhytogorianCorrection(ray, hit);
            }
            else
            {
                requiredHitPoint = Vector3.zero;
            }
        }

        private Vector3 PhytogorianCorrection(Ray ray, RaycastHit hit)
        {

            // Triangle
            hitPoint = hit.point;
            playerHeight = new Vector3(hitPoint.x, raycastReference.position.y, hitPoint.z);

            // x = isometricAngle, b = playerHeight.y - hitPoint.y, c = hypotenuse
            // sinx = b/c
            // sinx.c = b
            // c = b/sinx

            float hypotenuse = Vector3.Distance(playerHeight, hitPoint) / isometricAngleSin;

            // Player is at above the hit point
            if (raycastReference.position.y > hitPoint.y)
                return ray.GetPoint(hit.distance - hypotenuse);

            // Player is at below the hit point
            else if (raycastReference.position.y < hitPoint.y)
                return ray.GetPoint(hit.distance + hypotenuse);

            // Player and hit point are at the same height
            else
                return ray.GetPoint(hit.distance);

        }

        private float TakeSinusOfEulerAngle(float orthographicAngle)
        {
            return Mathf.Sin(orthographicAngle * Mathf.Deg2Rad);
        }
    }
}
