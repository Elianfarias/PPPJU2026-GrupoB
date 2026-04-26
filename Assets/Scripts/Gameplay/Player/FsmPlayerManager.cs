using Assets.Scripts.Gameplay.Player.States;
using Assets.Scripts.Gameplay.Player.States.SubStates;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Gameplay.Player
{
    public class FsmPlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerSettingsSO data;
        [SerializeField] private MMF_Player dodgeFeedback;
        [SerializeField] private MMF_Player jumpFeedback;
        [SerializeField] private Animator animator;
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject cameraThirdPerson;
        [SerializeField] private GameObject cameraFirstPerson;

        private PlayerContext ctx;
        private readonly List<StateBase> rootStates = new();
        private StateBase currentState;

        private void Awake()
        {
            ctx = new PlayerContext
            {
                Rb = GetComponent<Rigidbody>(),
                Animator = animator,
                Data = data,
                HealthSystem = healthSystem,
                CapsuleCollider = capsuleCollider,
                FirePoint = firePoint,
                FsmPlayerManager = this,
                OrbInventory = GetComponent<OrbInventory>(),
                AttackPressed = false,
                DodgePressed = false,
                JumpPressed = false,
                SprintPressed = false,
                DodgeFeedback = dodgeFeedback,
                JumpFeedback = jumpFeedback
            };

            rootStates.Add(new StateGrounded());
            rootStates.Add(new StateAirborne());
            rootStates.Add(new StateAttack());
            rootStates.Add(new StateDodge());
            rootStates.Add(new StateJumpingDown());

            foreach (var state in rootStates)
                state.Initialize(this, ctx);

            currentState = FindState(StateType.Grounded);
            currentState.OnEnter();
        }

        private void Update()
        {
            currentState.OnUpdate();
        }

        private void FixedUpdate() { currentState.OnFixedUpdate(); }
        private void OnAnimatorIK(int layerIndex) { currentState.OnAnimatorIK(layerIndex); }

        private void OnDrawGizmos()
        {
            if (data == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * data.RaycastDistance);
        }

        private void OnMove(InputValue value) { ctx.MoveInput = value.Get<Vector2>(); }
        private void OnLook(InputValue value) { ctx.LookInput = value.Get<Vector2>(); }
        private void OnAttack(InputValue value) { if (value.isPressed) ctx.AttackPressed = true; }
        private void OnJump(InputValue value) { if (value.isPressed) ctx.JumpPressed = true; }
        private void OnDodge(InputValue value) { if (value.isPressed) ctx.DodgePressed = true; }
        private void OnSprint(InputValue value) { ctx.SprintPressed = value.isPressed; }

        private void OnSwitchCamera(InputValue value)
        {
            if (!value.isPressed) return;
            bool isThird = cameraThirdPerson.activeSelf;
            cameraThirdPerson.SetActive(!isThird);
            cameraFirstPerson.SetActive(isThird);
        }

        public void SwitchState(StateType stateType)
        {
            var next = FindState(stateType);
            if (next == null || next == currentState) return;

            currentState.OnExit();
            currentState = next;
            currentState.OnEnter();
        }

        public void OnDodgeFinished()
        {
            if (currentState.StateType != StateType.Dodge) return;
            SwitchState(StateType.Grounded);
        }

        public StateBase FindState(StateType stateType)
        {
            foreach (var state in rootStates)
            {
                if (state is CompositeState composite)
                {
                    var sub = composite.FindSubState(stateType);
                    if (sub != null) return state;
                }

                if (state.StateType == stateType) return state;
            }

            return null;
        }

        public Coroutine StartManagedCoroutine(IEnumerator routine) => StartCoroutine(routine);

        public void ReturnFromAttack()
        {
            var next = FindState(ctx.PreviousRootState);
            if (next == null) return;

            currentState.OnExit();
            currentState = next;

            if (next is StateAirborne airborne)
                airborne.EnterFall();
            else if (next is CompositeState composite)
                composite.ReEnter();
            else
                next.OnEnter();
        }

        public void SwitchToFall()
        {
            StateAirborne airborne = FindState(StateType.Airborne) as StateAirborne;
            if (airborne == null) return;

            currentState.OnExit();
            currentState = airborne;
            airborne.EnterFall();
        }
    }
}