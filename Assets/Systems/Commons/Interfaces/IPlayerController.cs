using UnityEngine;

namespace InsertYourSoul
{
    public interface IPlayerController
    {
        InputStreamDataPackage InputData { get; }
        PlayerControlModel Model { get; }
        float GravityAxisValue { get; }
        bool IsMoving { get; }
        bool IsDashing { get; }
        bool IsCasting { get; }
        bool IsAlive { get; }
        AimDataPackage AimData { get; }
        float CharacterVelocity { get; }
        Transform SelfTransform { get; }
    }
}