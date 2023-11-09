using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    [RequireComponent(typeof(InputHandler))]
    public class InputDistributor : MonoBehaviour
    {
        private InputHandler InputHandler;
        private IReciveInputPackage[] inputReceivers;

        private void Awake()
        {
            InputHandler = GetComponent<InputHandler>();
            inputReceivers = GetComponents<IReciveInputPackage>();
        }
        private void Update()
        {
            foreach (var inputReceiver in inputReceivers)
            {
                inputReceiver.CacheInputs(InputHandler.GetInputData, true);
            }
        }
    }
}
