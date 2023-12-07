using UnityEngine;

namespace InsertYourSoul.CameraController
{
    /// <summary>
    /// CameraScript version Alpha
    /// </summary>
    [ExecuteInEditMode]
    public class CameraController : MonoBehaviour
    {
        #region Required References
        //references
        [Header("Required References")]
        [SerializeField] Transform target;
        [SerializeField] Camera cam;

        private void SetReferences()
        {
            if (cam == null)
            { cam = GetComponent<Camera>(); }
            if (target == null)
            { target = GameObject.FindGameObjectWithTag("Player").transform; }
        }
        #endregion

        #region Settings
        //position settings
        [Header("Position Settings")]
        [SerializeField] float smoothTime;
        [SerializeField] Vector3 offset;
        private Vector3 currentVelocity = Vector3.zero;

        //zoom settings
        [Header("Zoom"), SerializeField]
        private float zoom;
        [SerializeField] float zoomMax;
        [SerializeField] float zoomMin;
        #endregion

        #region Monobehaviour Methods

        private void Awake()
        {
            SetReferences();
            cam.orthographic = true;

            cam.transform.position = AddOffsetToTargetPosition(offset);
        }
        private void LateUpdate()
        {
            HandleZoom();
        }
        private void FixedUpdate()
        {
            transform.position = FollowTarget();
        }


        private Vector3 FollowTarget()
        {
            return Vector3.SmoothDamp(transform.position, AddOffsetToTargetPosition(offset), ref currentVelocity, smoothTime);
        }
        private Vector3 AddOffsetToTargetPosition(Vector3 _offset)
        {
            Vector3 vectorToReturn = target.position;
            //vectorToReturn.y = 0f;
            return vectorToReturn + _offset;
        }

        #endregion

        #region Zoom
        /// <summary>
        /// Handles HandleZoom
        /// </summary>
        private void HandleZoom()
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel"); //Will change this one later..
            zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
            cam.orthographicSize = zoom;
        }
        #endregion
    }
}
