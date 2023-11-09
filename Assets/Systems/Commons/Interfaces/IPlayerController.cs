namespace InsertYourSoul
{
    public interface IPlayerController
    {
        InputStreamDataPackage InputData { get; }
        PlayerControlModel Model { get; }
        float GravityAxis { get; }
        bool IsMoving { get; }
        float CharacterVelocity { get; }
    }
}