using InsertYourSoul.AbilitySystem;
using InsertYourSoul.CharacterSystem;
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


        private ICharacter character;
        private AbilityHandler abilityHandler;
        private IProvideAimData aimIndicator;

        public AimDataPackage AimData => aimIndicator.GetAimData;
        public bool IsAlive => character.IsAlive;
        public bool IsCasting => abilityHandler.IsCasting;
        public bool IsMoving => CharacterVelocity > 0f;
        public bool IsDashing => movementHandler.IsDashing;
        public Transform SelfTransform => this.transform;


        private List<ITickable> handlers = new List<ITickable>();

        RotationHandler rotationHandler;
        GravityHandler gravityHandler;
        MovementHandler movementHandler;
        AnimationHandler animationHandler;
        private void DeclareHandlers()
        {
            handlers.Add(rotationHandler = new RotationHandler(this));
            handlers.Add(gravityHandler = new GravityHandler(this));
            handlers.Add(movementHandler = new MovementHandler(this));
            handlers.Add(animationHandler = new AnimationHandler(this));
        }


        public float CharacterVelocity
        {
            get
            {
                Vector3 velocityVector = model.CharacterController.velocity;
                velocityVector.y = 0f;

                return velocityVector.magnitude;
            }
        }
        public float GravityAxisValue => gravityHandler.GravityAxis;

       

        private void Awake()
        {
            character = GetComponent<ICharacter>();
            abilityHandler = GetComponent<AbilityHandler>();
            aimIndicator = GameObject.FindGameObjectWithTag("TargetIndicator").GetComponent<IProvideAimData>();
            model.Initialize(GetComponent<CharacterController>(), this.transform, GetComponent<Animator>());

            DeclareHandlers();
        }


        private void FixedUpdate()
        {
            if (!IsAlive)
            {
                animationHandler.DieAnimation();
                return;
            }

            foreach (ITickable handler in handlers)
            {
                handler.Tick();
            }
        }

    }
}
