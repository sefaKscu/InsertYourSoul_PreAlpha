using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    /// <summary>
    /// Finds an input provider on the gameObject and distributes to the receivers.
    /// </summary>
    [RequireComponent(typeof(InputHandler))]
    public class InputDistributor : MonoBehaviour
    {
        private IProvideInputData InputHandler;
        private IReciveInputPackage[] inputReceivers;

        private void Awake()
        {
            InputHandler = GetComponent<IProvideInputData>();
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
