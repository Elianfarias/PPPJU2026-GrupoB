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
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask aimColliderMask;
        [SerializeField] private float aimMaxDistance = 100f;

        private PlayerContext ctx;
        private readonly List<StateBase> rootStates = new();
        private StateBase currentState;
        private float currentPitch;

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
                CameraTransform = mainCamera.transform,
                DodgeFeedback = dodgeFeedback,
                JumpFeedback = jumpFeedback,
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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateAim();
            ApplyLookRotation();
            currentState.OnUpdate();
        }

        private void FixedUpdate() => currentState.OnFixedUpdate();
        private void OnAnimatorIK(int layerIndex) => currentState.OnAnimatorIK(layerIndex);

        // Raycast desde el CENTRO de la pantalla (donde está el crosshair fijo).
        // Si pega contra geometría usamos ese punto; si no, un punto lejano hacia
        // donde apunta la cámara. La dirección del hechizo va desde el FirePoint
        // hasta ese punto.
        private void UpdateAim()
        {
            Vector2 screenCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, aimMaxDistance, aimColliderMask))
                ctx.AimPoint = hit.point;
            else
                ctx.AimPoint = ray.GetPoint(aimMaxDistance);

            Vector3 rawDir = ctx.AimPoint - firePoint.position;
            ctx.AimDirection = rawDir.sqrMagnitude > 0.01f ? rawDir.normalized : mainCamera.transform.forward;
        }

        private void OnMove(InputValue value) { ctx.MoveInput = value.Get<Vector2>(); }
        private void OnLook(InputValue value) { ctx.LookInput = value.Get<Vector2>(); }
        private void OnRightAttack(InputValue value) { if (value.isPressed) ctx.RightAttackPressed = true; }
        private void OnLeftAttack(InputValue value) { if (value.isPressed) ctx.LeftAttackPressed = true; }
        private void OnJump(InputValue value) { if (value.isPressed) ctx.JumpPressed = true; }
        private void OnDodge(InputValue value) { if (value.isPressed) ctx.DodgePressed = true; }
        private void OnSprint(InputValue value) { ctx.SprintPressed = value.isPressed; }

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
            if (FindState(StateType.Airborne) is not StateAirborne airborne) return;

            currentState.OnExit();
            currentState = airborne;
            airborne.EnterFall();
        }

        private void ApplyLookRotation()
        {
            float yaw = ctx.LookInput.x * data.RotationSpeedX * Time.deltaTime;
            float pitch = ctx.LookInput.y * data.RotationSpeedY * Time.deltaTime;

            transform.Rotate(Vector3.up, yaw, Space.World);
            currentPitch = Mathf.Clamp(currentPitch - pitch, -60f, 60f);
            cameraPivot.localEulerAngles = new Vector3(currentPitch, 0f, 0f);
        }

        private void OnDrawGizmos()
        {
            if (data == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * data.RaycastDistance);

            if (!Application.isPlaying) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(ctx.AimPoint, 0.2f);
            Gizmos.DrawLine(firePoint.position, ctx.AimPoint);
        }
    }
}