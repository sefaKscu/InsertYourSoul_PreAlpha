using UnityEngine;

namespace InsertYourSoul.StatSystem
{
    [CreateAssetMenu(fileName = "new_ActiveStatData", menuName = "Insert Your Soul/Stat System/Data/Active Stat Data")]
    public class ActiveStatDataSO : ScriptableObject
    {
        public ActiveStatViewData StatData;
    }

}
