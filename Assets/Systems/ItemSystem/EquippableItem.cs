using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul.ItemSystem
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Item System/Items/Equippable Item", fileName = "new_Item")]
    public class EquippableItem : Item
    {
        public List<StatModifier> ModifierList = new List<StatModifier>();

        public void Equip(ICanEquip c)
        {
            c.EquipItem(this);
        }
        public void UnEquip(ICanEquip c)
        {
            c.UnEquipItem(this);
        }
    }
}
