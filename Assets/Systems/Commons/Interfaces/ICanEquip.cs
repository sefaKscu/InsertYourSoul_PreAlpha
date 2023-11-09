using InsertYourSoul.ItemSystem;

namespace InsertYourSoul
{
    public interface ICanEquip
    {
        void EquipItem(EquippableItem _item);
        void UnEquipItem(EquippableItem _item);
    }
}