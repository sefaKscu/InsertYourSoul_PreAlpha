using InsertYourSoul.CharacterSystem;
using System.Linq;
using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    public class AbilityHandler : MonoBehaviour, IReciveInputPackage
    {
        [SerializeField] AbilityReferences references;
        [SerializeField] IProvideAimData aimIndicator;
        [SerializeField] ICharacter character;
        public bool IsAlive => character.IsAlive;

        public bool IsCasting
        {
            get
            {
                foreach (AbilitySlot slot in AbilitySlots)
                {
                    if (slot.IsActive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public AbilitySlot[] AbilitySlots = new AbilitySlot[4];
        private void SetSlots()
        {
            foreach (var slot in AbilitySlots.Where(slot => slot != null))
            {
                slot.Initialize(character);
            }
        }
        private void TickSlots()
        {
            foreach (var slot in AbilitySlots.Where(slot => slot != null))
            {
                slot.Tick();
            }
        }

        private void Awake()
        {
            character = GetComponent<ICharacter>();
            aimIndicator = GameObject.FindGameObjectWithTag("TargetIndicator").GetComponent<IProvideAimData>();
            SetSlots();
        }
        private void Update()
        {
            UpdateTargetPosition();
            TickSlots();
            CheckInputs();
        }

        private void UpdateTargetPosition()
        {
            references.AimData = aimIndicator.GetAimData;
        }

        #region Inputs
        private InputStreamDataPackage inputs;
        public void CacheInputs(InputStreamDataPackage _inputStream, bool _isAlive) 
        {
            inputs = _inputStream;
        }

        private void CheckInputs()
        {
            if (!IsAlive)
                return;
            if (IsCasting)
                return;

            if (inputs.IsAbility0ButtonDown)
                AbilitySlots[0].Use(references);
            if (inputs.IsAbility1ButtonDown)
                AbilitySlots[1].Use(references);
            if (inputs.IsAbility2ButtonDown)
                AbilitySlots[2].Use(references);
            if (inputs.IsAbility3ButtonDown)
                AbilitySlots[3].Use(references);
        }
        #endregion
    }
}
