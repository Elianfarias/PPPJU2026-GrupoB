using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States.SubStates
{
    public class StateRunning : StateBase
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Running;
        }

        public override void OnEnter() => base.OnEnter();
        public override void OnUpdate() { }

        public override void OnFixedUpdate()
        {
            ApplyMovement();
        }

        public override void OnExit() { }
        public override void OnAnimatorIK(int layerIndex) { }

        private void ApplyMovement()
        {
            Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);
            Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);
            Vector3 targetVelocity = worldDirection * PlayerContext.Data.MaxHorizontalSpeed;

            Vector3 currentHorizontal = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
            Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetVelocity, PlayerContext.Data.Acceleration * Time.fixedDeltaTime);

            PlayerContext.Rb.linearVelocity = new Vector3(newHorizontal.x, PlayerContext.Rb.linearVelocity.y, newHorizontal.z);
        }
    }
}