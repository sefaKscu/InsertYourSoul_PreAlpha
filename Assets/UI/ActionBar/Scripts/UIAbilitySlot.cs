using UnityEngine;
using UnityEngine.UI;
using InsertYourSoul.AbilitySystem;

namespace InsertYourSoul.UI
{
    public class UIAbilitySlot : MonoBehaviour
    {
        [Header("Defaults")]
        [SerializeField] private Sprite DefaultIcon;
        [SerializeField] private Color DefaultColor;
        [Header("References")]
        [SerializeField] private Image AbilityIcon;
        [SerializeField] private Image Filler;
        [SerializeField] private AbilitySlotData SlotData;

        private bool isSlotEmpty;

        private void Awake() => SlotData.Subscribe(this);
        private void Start() => OnAbilityChanged();
        private void Update() => CheckAbilityState();
        private void OnDestroy() => SlotData.Unsubscribe(this);


        public void OnAbilityChanged()
        {
            isSlotEmpty = SlotData.IsSlotEmpty;
            Filler.fillAmount = 0;

            if (isSlotEmpty)
            {
                AbilityIcon.sprite = DefaultIcon;
                AbilityIcon.color = DefaultColor;
                return;
            }

            AbilityIcon.sprite = SlotData.Icon;
            AbilityIcon.color = Color.white;
        }

        private void CheckAbilityState()
        {
            if (isSlotEmpty)
                return;

            switch (SlotData.State)
            {
                case AbilitySlotState.Ready:
                    Filler.fillAmount = 0f;
                    break;

                case AbilitySlotState.InsufficientMana:
                    Filler.fillAmount = 1f;
                    break;

                case AbilitySlotState.Active:
                    Filler.fillClockwise = true;
                    Filler.fillAmount = SlotData.ActiveFillerPercent;
                    break;

                case AbilitySlotState.Cooldown:
                    Filler.fillClockwise = false;
                    Filler.fillAmount = SlotData.CooldownFillerPercent;
                    break;
            }
        }
    }
}
