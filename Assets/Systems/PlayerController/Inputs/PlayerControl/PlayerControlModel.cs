using UnityEngine;

namespace InsertYourSoul
{
    [System.Serializable]
    public class PlayerControlModel
    {

        [Header("Speed")]
        [SerializeField, Range(1f, 20f)] float walkSpeedConstant = 2f;
        [SerializeField, Range(1f, 20f)] float runSpeedConstant = 5f;
        [SerializeField, Range(1f, 20f)] float sprintSpeedConstant = 8f;
        [SerializeField, Range(1f, 20f)] float defaultDeceleration = 16f;

        [Header("Gravity Settings")]
        [SerializeField, Range(0f, 30f)] float gravity = 9.8f;
        [SerializeField, Range(0.1f, 1f)] float groundedGravity = 0.5f;

        [Header("Other Setting")]
        [SerializeField, Range(0.02f, 20f)] float rotationFactorPerFrame = 10f;

        public void Initialize(CharacterController parentController, Transform parentTransform, Animator parentAnimator)
        {
            CharacterController = parentController;
            CharacterController.center = new Vector3(0f, parentController.height / 2, 0f);
            Transform = parentTransform;
            Animator = parentAnimator;
        }

        // Property Getters
        public CharacterController CharacterController { get; private set; }
        public Transform Transform { get; private set; }
        public Animator Animator { get; private set; }
        public float WalkSpeedConstant => walkSpeedConstant;
        public float RunSpeedConstant => runSpeedConstant;
        public float SprintSpeedConstant => sprintSpeedConstant;
        public float DefaultDeceleration => defaultDeceleration;
        public float Gravity => gravity;
        public float GroundedGravity => groundedGravity;
        public float RotationFactorPerFrame => rotationFactorPerFrame;

    }
}
