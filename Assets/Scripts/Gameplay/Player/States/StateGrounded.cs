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

            if (PlayerContext.DodgePressed && PlayerContext.NextDodgeTime <= Time.time)
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

            if (PlayerContext.AttackPressed)
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
            float angle = PlayerContext.LookInput.x * PlayerContext.Data.RotationSpeedX * Time.fixedDeltaTime;
            PlayerContext.FsmPlayerManager.transform.Rotate(Vector3.up, angle, Space.World);
        }

        private bool IsGrounded()
        {
            float radius = PlayerContext.CapsuleCollider.radius;
            Vector3 bottom = PlayerContext.CapsuleCollider.bounds.center - Vector3.up * (PlayerContext.CapsuleCollider.bounds.extents.y - radius);
            return Physics.SphereCast(bottom, radius * 0.9f, Vector3.down, out _, PlayerContext.Data.RaycastDistance, PlayerContext.Data.LayerCollision);
        }
    }
}