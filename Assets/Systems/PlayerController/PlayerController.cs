using System.Collections.Generic;
using UnityEngine;

namespace InsertYourSoul.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IReciveInputPackage, IPlayerController
    {

        public PlayerControlModel Model => model;
        public PlayerControlModel model;

        public InputStreamDataPackage InputData => inputData;
        private InputStreamDataPackage inputData;
        public void CacheInputs(InputStreamDataPackage package, bool isAlive)
        {
            inputData = package;
        }


        private List<ITickable> handlers = new List<ITickable>();

        RotationHandler rotationHandler;
        GravityHandler gravityHandler;
        MovementHandlerSnappy movementHandler;
        AnimationHandler animationHandler;
        private void DeclareHandlers()
        {
            handlers.Add(rotationHandler = new RotationHandler(this));
            handlers.Add(gravityHandler = new GravityHandler(this));
            handlers.Add(movementHandler = new MovementHandlerSnappy(this));
            handlers.Add(animationHandler = new AnimationHandler(this));
        }

        public bool IsMoving => CharacterVelocity > 0f;
        public float CharacterVelocity
        {
            get
            {
                Vector3 velocityVector = model.CharacterController.velocity;
                velocityVector.y = 0f;

                return velocityVector.magnitude;
            }
        }
        public float GravityAxis => gravityHandler.GravityAxis;

        private void Awake()
        {
            model.Initialize(GetComponent<CharacterController>(), this.transform, GetComponent<Animator>());

            DeclareHandlers();
        }


        private void FixedUpdate()
        {
            foreach (ITickable handler in handlers)
            {
                handler.Tick();
            }
        }

    }
}
