using UnityEngine;
using InsertYourSoul.StatSystem;

namespace InsertYourSoul.CharacterSystem
{
    [CreateAssetMenu(fileName = "new_CharacterTemplate", menuName = "Insert Your Soul/Character System/Character Template")]
    public class CharacterTemplate : ScriptableObject
    {
        [SerializeField] string characterName;
        [SerializeField] float life;
        [SerializeField] float mana;
        [SerializeField] float lifeRegenPerSec;
        [SerializeField] float manaRegenPerSec;
        [SerializeField] float moveSpeed;
        [SerializeField] float lightRadius;

        public string Name => characterName;
        public Stat Life => new Stat { Type = StatType.Life, Value = life };
        public Stat Mana => new Stat { Type = StatType.Mana, Value = mana };
        public Stat LifeRegen => new Stat { Type = StatType.LifeRegenPerSec, Value = lifeRegenPerSec };
        public Stat ManaRegen => new Stat { Type = StatType.ManaRegenPerSec, Value = manaRegenPerSec };
        public Stat MoveSpeed => new Stat { Type = StatType.MoveSpeed, Value = moveSpeed };
        public Stat LightRadius => new Stat { Type = StatType.LightRadius, Value = lightRadius };
    }
}
