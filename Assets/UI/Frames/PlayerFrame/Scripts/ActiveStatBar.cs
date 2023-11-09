using InsertYourSoul.StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace InsertYourSoul.UI
{
    public class ActiveStatBar : MonoBehaviour
    {
        [SerializeField] private Image filler;
        [SerializeField] private Text fillerText;
        [SerializeField] private float smothenLerp;
        [SerializeField] ActiveStatDataSO statToShow;


        private int totalValue;
        private int currentValue;
        private float currentPercentValue;
        private string textValue;

        private void Update()
        {
            if (statToShow == null)
                return;

            CacheData();
            UpdateFiller();
            UpdateText();
        }

        private void CacheData()
        {
            totalValue = (int)statToShow.StatData.TotalValue;
            currentValue = (int)statToShow.StatData.CurrentValue;
            currentPercentValue = statToShow.StatData.CurrentPercentValue;
            textValue = ActiveStatToString(currentValue, totalValue);
        }

        private void UpdateFiller()
        {
            if (filler == null)
                return;
            filler.fillAmount = Mathf.Lerp(filler.fillAmount, currentPercentValue, smothenLerp);
        }

        private void UpdateText()
        {
            if (fillerText == null)
                return;
            fillerText.text = textValue;
        }

        private string ActiveStatToString(int _currentValue, int _totalValue)
        {
            return _currentValue.ToString() + "/" + _totalValue.ToString();
        }
    }
}
