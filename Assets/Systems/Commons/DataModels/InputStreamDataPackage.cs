using UnityEngine;

namespace InsertYourSoul
{
    [System.Serializable]
    public struct InputStreamDataPackage
    {
        public bool IsAbility0ButtonDown;
        public bool IsAbility1ButtonDown;
        public bool IsAbility2ButtonDown;
        public bool IsAbility3ButtonDown;
        public bool IsAiming;
        public bool IsAimingWithMouse;
        public bool IsMovementPressed;
        public bool IsSprintPressed;
        public bool IsRunToggled;
        public Vector2 MovementInputRaw;
        public Vector3 MovementDirection;
        public Vector3 AimDirectionRaw;
    }
}
