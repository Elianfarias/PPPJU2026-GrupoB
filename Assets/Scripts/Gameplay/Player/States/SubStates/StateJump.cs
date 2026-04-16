using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States.SubStates
{
    public class StateJump : StateBase
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Jump;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ApplyJumpImpulse();
        }

        public override void OnUpdate() { }
        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            PlayerContext.JumpPressed = false;
        }

        public override void OnAnimatorIK(int layerIndex) { }

        private void ApplyJumpImpulse()
        {
            var velocity = PlayerContext.Rb.linearVelocity;
            velocity.y = 0f;
            PlayerContext.Rb.linearVelocity = velocity;
            PlayerContext.Rb.AddForce(Vector3.up * PlayerContext.Data.JumpForce, ForceMode.Impulse);
        }
    }
}