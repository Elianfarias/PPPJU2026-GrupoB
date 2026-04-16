using Assets.Scripts.Gameplay.Player.States.SubStates;
using Unity.VisualScripting;
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
            if (PlayerContext.AttackPressed)
            {
                PlayerContext.PreviousRootState = StateType.Airborne;
                Manager.SwitchState(StateType.Attack);
                return;
            }

            if (PlayerContext.Rb.linearVelocity.y > 0.1f) return;
            if (!IsGrounded()) return;

            Manager.SwitchState(StateType.Idle);
        }

        protected override void OnParentFixedUpdate()
        {
            ApplyAerialMovement();
            ApplyRotation();
        }

        private void ApplyAerialMovement()
        {
            Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
            Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);
            Vector3 targetVelocity = AerialMultiplier * PlayerContext.Data.MaxHorizontalSpeed * worldDirection;

            Vector3 currentHorizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
            Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetVelocity, PlayerContext.Data.Acceleration * AerialMultiplier * Time.fixedDeltaTime);

            PlayerContext.Rb.linearVelocity = new Vector3(newHorizontal.x, PlayerContext.Rb.linearVelocity.y, newHorizontal.z);
        }

        private void ApplyRotation()
        {
            float angle = PlayerContext.LookInput.x * PlayerContext.Data.RotationSpeedX * Time.fixedDeltaTime;
            PlayerContext.FsmPlayerManager.transform.Rotate(Vector3.up, angle, Space.World);
        }

        private bool IsGrounded()
        {
            float radius = PlayerContext.CapsuleCollider.radius;
            Vector3 bottom = PlayerContext.CapsuleCollider.bounds.center - Vector3.up * (PlayerContext.CapsuleCollider.bounds.extents.y - radius);
            return Physics.SphereCast(bottom, radius * 0.9f, Vector3.down, out _, PlayerContext.Data.RaycastDistance, PlayerContext.Data.LayerCollision);
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