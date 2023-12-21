namespace InsertYourSoul.StatSystem
{
    public interface IDamagable
    {
        void TakeDamage(DamageData damage);
        void HealLife(float amount);
    }
    public interface IHealable
    {
        void HealLife(float amount);
    }
}
