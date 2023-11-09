using InsertYourSoul.CharacterSystem;
using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    public enum AbilitySlotState
    {
        Ready,
        Active,
        Cooldown,
        InsufficientMana
    }

    [System.Serializable]
    public class AbilitySlot
    {
        public void Initialize(ICharacter _character)
        {
            this.character = _character;
            state = AbilitySlotState.Ready;
            UpdateAbilityInSlotData();
        }

        private ICharacter character;
        public Ability ability;
        public AbilitySlotState state;
        

        // Counters
        public float ActiveTimeCounter = 0f;
        public float CooldownTimeCounter = 0f;

        public void Use(AbilityReferences _references)
        {
            if (ability == null)
            {
                Debugger("No ability assigned.");
                return;
            }
            if (state == AbilitySlotState.Ready)
            {
                if (character.SpendMana(ability.ManaCost))
                {
                    ability.Activate(_references);
                    state = AbilitySlotState.Active;
                    ActiveTimeCounter = ability.ActiveSeconds;
                }
                else
                {
                    state = AbilitySlotState.InsufficientMana;
                }
                Debugger(ability.name + " " + state);
            }
        }

        public void Tick()
        {
            if (ability == null)
                return;

            SwitchStates();
            UpdateValuesInSlotData();
        }

        private void SwitchStates()
        {
            switch (state)
            {
                case AbilitySlotState.Ready:
                    if (ability.ManaCost > character.CurrentMana)
                    {
                        state = AbilitySlotState.InsufficientMana;
                        Debugger(ability.Name + " " + state);
                    }
                    break;

                case AbilitySlotState.InsufficientMana:
                    Debugger(state + " for " + ability.name);
                    if (character.CurrentMana > ability.ManaCost)
                    {
                        state = AbilitySlotState.Ready;
                        Debugger(ability.Name + " " + state);
                    }
                    break;

                case AbilitySlotState.Active:
                    if (ActiveTimeCounter > 0f)
                    {
                        ActiveTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        ActiveTimeCounter = 0f;
                        state = AbilitySlotState.Cooldown;
                        CooldownTimeCounter = ability.CooldownSeconds;
                        Debugger(ability.Name + " " + state);
                    }
                    break;

                case AbilitySlotState.Cooldown:
                    if (CooldownTimeCounter > 0f)
                    {
                        CooldownTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        if (ability.ManaCost > character.CurrentMana)
                        {
                            state = AbilitySlotState.InsufficientMana;
                            Debugger(ability.Name + " " + state);
                        }
                        else
                        {
                            CooldownTimeCounter = 0f;
                            state = AbilitySlotState.Ready;
                            Debugger(ability.Name + " " + state);
                        }
                    }
                    break;
            }
        }

        public void AddAbility(Ability _ability)
        {
            if (ability != null)
                RemoveAbility();

            ability = _ability;
            UpdateAbilityInSlotData();
        }
        public void RemoveAbility()
        {
            ability = null;
            UpdateAbilityInSlotData();
        }

        #region SO SlotData
        // UI SlotData
        public AbilitySlotData SlotData;
        private float activeProgPercent;
        private float cooldownProgPercent;
        private void UpdateAbilityInSlotData()
        {
            if (ability != null)
                SlotData.ChangeAbility(ability);
        }

        private void UpdateValuesInSlotData()
        {
            SlotData.State = state;
            activeProgPercent = 1f - (ActiveTimeCounter / ability.ActiveSeconds);
            cooldownProgPercent = CooldownTimeCounter / ability.CooldownSeconds;
            SlotData.UpdateData(activeProgPercent, cooldownProgPercent, state);
        }
        #endregion

        #region Debugger
        public bool IsDebuggin;
        private void Debugger(string _message)
        {
            if (!IsDebuggin)
                return;

            Debug.Log(_message);
        }
        #endregion
    }
}
