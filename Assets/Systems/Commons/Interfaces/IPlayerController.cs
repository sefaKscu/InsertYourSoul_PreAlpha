namespace InsertYourSoul
{
    public interface IPlayerController
    {
        InputStreamDataPackage InputData { get; }
        PlayerControlModel Model { get; }
        float GravityAxis { get; }
        bool IsMoving { get; }
        bool IsCasting { get; }
        bool IsAlive { get; }
        AimDataPackage AimData { get; }
        float CharacterVelocity { get; }
    }
}