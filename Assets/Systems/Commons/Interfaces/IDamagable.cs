namespace InsertYourSoul
{
    public interface IDamagable
    {
        void TakeDamage(DamageData damage);
        void HealLife(float amount);
    }
}
