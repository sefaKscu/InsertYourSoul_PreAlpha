using InsertYourSoul.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Ability System/AbilityData/AbilitySlotData", fileName = "SlotData")]
    public class AbilitySlotData : ScriptableObject
    {
        // Ability Definition
        public Sprite Icon;
        public string Name;
        public string Description;
        public bool IsSlotEmpty;

        // Values
        public AbilitySlotState State;
        public float ActiveFillerPercent;
        public float CooldownFillerPercent;

        public void ChangeAbility(Ability ability)
        {
            Icon = ability.Icon;
            Name = ability.Name;
            Description = ability.Description;
            InvokeListeners();
        }

        public void UpdateData(float _activeFillerPercent, float _cooldownFillerPercent, AbilitySlotState _state)
        {
            ActiveFillerPercent = _activeFillerPercent;
            CooldownFillerPercent = _cooldownFillerPercent;
        }

        HashSet<UIAbilitySlot> listeners = new HashSet<UIAbilitySlot>();

        public void InvokeListeners()
        {
            foreach (var listener in listeners)
            {
                listener.OnAbilityChanged();
            }
        }

        public void Subscribe(UIAbilitySlot _newListener) => listeners.Add(_newListener);
        public void Unsubscribe(UIAbilitySlot _destroyedListener) => listeners.Remove(_destroyedListener);


    }
}
