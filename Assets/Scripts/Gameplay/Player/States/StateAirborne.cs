using Assets.Scripts.Gameplay.Player.States.SubStates;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States
{
    public class StateAirborne : CompositeState
    {
        private const float AerialMultiplier = 0.7f;

        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Airborne;
        }

        protected override void RegisterSubStates()
        {
            SubStates.Add(new StateJump());
            SubStates.Add(new StateFall());
        }

        protected override StateBase ResolveInitialSubState()
        {
            return SubStates.Find(s => s.StateType == StateType.Jump);
        }

        protected override void HandleParentTransitions()
        {
            if (PlayerContext.LeftAttackPressed || PlayerContext.RightAttackPressed)
            {
                PlayerContext.PreviousRootState = StateType.Airborne;
                Manager.SwitchState(StateType.Attack);
                return;
            }

            if (PlayerContext.Rb.linearVelocity.y > 0.1f) return;
            if (!IsGrounded()) return;

            Manager.SwitchState(StateType.JumpingDown);
        }

        protected override void OnParentFixedUpdate()
        {
            ApplyAerialMovement();
            RotateWithCamera();
        }

        private void ApplyAerialMovement()
        {
            Vector3 worldDir = CameraRelativeDirection(PlayerContext.MoveInput);
            Vector3 target = worldDir * (PlayerContext.Data.MaxHorizontalSpeed * AerialMultiplier);

            Vector3 current = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
            Vector3 newHoriz = Vector3.MoveTowards(
                current, target,
                PlayerContext.Data.Acceleration * AerialMultiplier * Time.fixedDeltaTime
            );

            PlayerContext.Rb.linearVelocity = new Vector3(newHoriz.x, PlayerContext.Rb.linearVelocity.y, newHoriz.z);
        }

        private void RotateWithCamera()
        {
            Vector3 camForward = PlayerContext.CameraTransform.forward;
            camForward.y = 0f;
            if (camForward.sqrMagnitude < 0.01f) return;

            Quaternion target = Quaternion.LookRotation(camForward.normalized);
            Manager.transform.rotation = Quaternion.Lerp(
                Manager.transform.rotation,
                target,
                Time.fixedDeltaTime * PlayerContext.Data.RotationSpeedX
            );
        }

        private Vector3 CameraRelativeDirection(Vector2 input)
        {
            Transform cam = PlayerContext.CameraTransform;
            Vector3 camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
            return (camForward * input.y + camRight * input.x).normalized;
        }

        private bool IsGrounded()
        {
            float radius = PlayerContext.CapsuleCollider.radius;
            Vector3 bottom = PlayerContext.CapsuleCollider.bounds.center
                - Vector3.up * (PlayerContext.CapsuleCollider.bounds.extents.y - radius);
            return Physics.SphereCast(bottom, radius * 0.9f, Vector3.down, out _,
                PlayerContext.Data.RaycastDistance, PlayerContext.Data.LayerCollision);
        }

        public void EnterFall()
        {
            PlayerContext.Animator.SetInteger(StateHash, (int)StateType);
            CurrentSubState?.OnExit();
            CurrentSubState = SubStates.Find(s => s.StateType == StateType.Fall);
            CurrentSubState.OnEnter();
        }
    }
}