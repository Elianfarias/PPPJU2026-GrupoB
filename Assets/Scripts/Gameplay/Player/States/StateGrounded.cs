using Assets.Scripts.Gameplay.Player.States.SubStates;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States
{
    public class StateGrounded : CompositeState
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Grounded;
        }

        protected override void RegisterSubStates()
        {
            SubStates.Add(new StateIdle());
            SubStates.Add(new StateWalking());
            SubStates.Add(new StateRunning());
        }

        protected override StateBase ResolveInitialSubState()
        {
            return SubStates.Find(s => s.StateType == StateType.Idle);
        }

        protected override void HandleParentTransitions()
        {
            if (!IsGrounded())
            {
                Manager.SwitchToFall();
                return;
            }

            if (PlayerContext.DodgePressed)
            {
                Manager.SwitchState(StateType.Dodge);
                return;
            }

            if (PlayerContext.JumpPressed)
            {
                PlayerContext.JumpPressed = false;
                Manager.SwitchState(StateType.Jump);
                return;
            }

            if (PlayerContext.LeftAttackPressed || PlayerContext.RightAttackPressed)
            {
                PlayerContext.PreviousRootState = StateType.Grounded;
                Manager.SwitchState(StateType.Attack);
                return;
            }


            if (PlayerContext.MoveInput == Vector2.zero)
                SwitchSubState(StateType.Idle);
            else if (PlayerContext.SprintPressed)
                SwitchSubState(StateType.Running);
            else
                SwitchSubState(StateType.Walking);
        }

        protected override void OnParentFixedUpdate()
        {
            ApplyRotation();
        }

        private void ApplyRotation()
        {
            if (PlayerContext.MoveInput == Vector2.zero) return;

            Vector3 moveDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
            Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(moveDirection);

            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            PlayerContext.FsmPlayerManager.transform.rotation = Quaternion.Lerp(
                PlayerContext.FsmPlayerManager.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * PlayerContext.Data.RotationSpeedX
            );
        }

        private bool IsGrounded()
        {
            float radius = PlayerContext.CapsuleCollider.radius;
            Vector3 bottom = PlayerContext.CapsuleCollider.bounds.center - Vector3.up * (PlayerContext.CapsuleCollider.bounds.extents.y - radius);
            return Physics.SphereCast(bottom, radius * 0.9f, Vector3.down, out _, PlayerContext.Data.RaycastDistance, PlayerContext.Data.LayerCollision);
        }
    }
}