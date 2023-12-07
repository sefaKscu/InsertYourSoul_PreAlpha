using UnityEngine;

namespace InsertYourSoul
{
    public class Hover : MonoBehaviour
    {
        public float hoverHeight = 1f; // Adjust this value to change the hover height
        public float hoverSpeed = 1f; // Adjust this value to change the hover speed
        public Transform hoverTransform;
        private void Update()
        {
            // Calculate the new Y position based on a sine wave
            float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

            // Apply the new position to the object
            hoverTransform.position = new Vector3(hoverTransform.position.x, newY , hoverTransform.position.z);
        }
    }
}
