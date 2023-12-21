using InsertYourSoul.ItemSystem;
using InsertYourSoul.StatSystem;

namespace InsertYourSoul.CharacterSystem
{
    public interface ICharacter
    {
        public float CurrentLife { get; }
        public float CurrentMana { get; }
        public bool IsAlive { get; }
        void EquipItem(EquippableItem _item);
        void HealLife(float amount);
        void HealMana(float amount);
        void Regenerate();
        bool SpendMana(float amount);
        void TakeDamage(DamageData damage);
        void UnEquipItem(EquippableItem _item);
    }
}