namespace InsertYourSoul.StatSystem
{
    public class ActiveStatProcessor : PassiveStatProcessor
    {
        public ActiveStatProcessor(Stat _baseStat) : base(_baseStat)
        {
            currentValue = TotalValue;
        }


        // Public Values
        public float TotalValue => TotalStat.Value;
        public float CurrentValue
        {
            get
            {
                if (currentValue > TotalValue)
                {
                    currentValue = TotalValue;
                }
                return currentValue;
            }
            private set
            {
                if (value > TotalValue)
                {
                    currentValue = TotalValue;
                }
                else if (value < 0f)
                {
                    currentValue = 0f;
                }
                else
                {
                    currentValue = value;
                }
            }
        }
        public float CurrentPercentValue
        {
            get
            {
                if (CurrentValue <= 0f)
                    currentPercentValue = 0f;
                else if (CurrentValue > 0f)
                    currentPercentValue = CurrentValue / TotalValue;

                return currentPercentValue;
            }
        }
        public bool OnFullAmount => currentPercentValue == 1f;


        // Private Values
        protected float currentValue;
        protected float currentPercentValue;

        // Functions
        public void GainValue(float _value)
        {
            CurrentValue += _value;
        }
        public bool LooseValue(float _value)
        {
            if (CurrentValue <= _value && statType == StatType.Life)
            {
                currentValue = 0;
                return false;
            }
            else if (CurrentValue < _value && statType == StatType.Mana)
            {
                return false;
            }
            else
            {
                CurrentValue -= _value;
                return true;
            }
        }

        #region UI Stat
        public ActiveStatViewData StatData
        {
            get
            {
                statData.TotalValue = TotalValue;
                statData.CurrentValue = CurrentValue;
                statData.CurrentPercentValue = CurrentPercentValue;
                return statData;
            }
        }
        protected ActiveStatViewData statData;
        #endregion

    }
}
