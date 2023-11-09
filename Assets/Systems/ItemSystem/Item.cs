using UnityEngine;

namespace InsertYourSoul.ItemSystem
{
    public abstract class Item : ScriptableObject
    {
        #region Base Properties
        [Header("General Definition")]
        public string ItemName;
        public Sprite Icon;
        public GameObject itemPrefab;
        [Range(1, 999)] public int maximumStack = 1;
        [TextArea]
        public string description = "This is the default description of an equippable item. It will roar like a bear or it will hum like a bird";
        #endregion
    }
}
