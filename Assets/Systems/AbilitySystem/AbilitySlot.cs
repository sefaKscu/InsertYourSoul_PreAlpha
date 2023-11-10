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
            State = AbilitySlotState.Ready;
            UpdateAbilityInSlotData();
        }

        public bool IsActive => State == AbilitySlotState.Active;

        private ICharacter character;
        public Ability Ability;
        public AbilitySlotState State;
        

        // Counters
        public float ActiveTimeCounter = 0f;
        public float CooldownTimeCounter = 0f;

        public void Use(AbilityReferences _references)
        {
            if (Ability == null)
            {
                Debugger("No ability assigned.");
                return;
            }
            if (State == AbilitySlotState.Ready)
            {
                if (character.SpendMana(Ability.ManaCost))
                {
                    Ability.Activate(_references);
                    State = AbilitySlotState.Active;
                    ActiveTimeCounter = Ability.ActiveSeconds;
                }
                else
                {
                    State = AbilitySlotState.InsufficientMana;
                }
                Debugger(Ability.name + " " + State);
            }
        }

        public void Tick()
        {
            if (Ability == null)
                return;

            SwitchStates();
            UpdateValuesInSlotData();
        }

        private void SwitchStates()
        {
            switch (State)
            {
                case AbilitySlotState.Ready:
                    if (Ability.ManaCost > character.CurrentMana)
                    {
                        State = AbilitySlotState.InsufficientMana;
                        Debugger(Ability.Name + " " + State);
                    }
                    break;

                case AbilitySlotState.InsufficientMana:
                    Debugger(State + " for " + Ability.name);
                    if (character.CurrentMana > Ability.ManaCost)
                    {
                        State = AbilitySlotState.Ready;
                        Debugger(Ability.Name + " " + State);
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
                        State = AbilitySlotState.Cooldown;
                        CooldownTimeCounter = Ability.CooldownSeconds;
                        Debugger(Ability.Name + " " + State);
                    }
                    break;

                case AbilitySlotState.Cooldown:
                    if (CooldownTimeCounter > 0f)
                    {
                        CooldownTimeCounter -= Time.deltaTime;
                    }
                    else
                    {
                        if (Ability.ManaCost > character.CurrentMana)
                        {
                            State = AbilitySlotState.InsufficientMana;
                            Debugger(Ability.Name + " " + State);
                        }
                        else
                        {
                            CooldownTimeCounter = 0f;
                            State = AbilitySlotState.Ready;
                            Debugger(Ability.Name + " " + State);
                        }
                    }
                    break;
            }
        }

        public void AddAbility(Ability _ability)
        {
            if (Ability != null)
                RemoveAbility();

            Ability = _ability;
            UpdateAbilityInSlotData();
        }
        public void RemoveAbility()
        {
            Ability = null;
            UpdateAbilityInSlotData();
        }

        #region SO SlotData
        // UI SlotData
        public AbilitySlotData SlotData;
        private float activeProgPercent;
        private float cooldownProgPercent;
        private void UpdateAbilityInSlotData()
        {
            if (Ability != null)
                SlotData.ChangeAbility(Ability);
        }

        private void UpdateValuesInSlotData()
        {
            SlotData.State = State;
            activeProgPercent = 1f - (ActiveTimeCounter / Ability.ActiveSeconds);
            cooldownProgPercent = CooldownTimeCounter / Ability.CooldownSeconds;
            SlotData.UpdateData(activeProgPercent, cooldownProgPercent, State);
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
