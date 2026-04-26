using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States
{
    public class StateDodge : StateBase
    {

        public override void Initialize(FsmPlayerManager fsmManager, PlayerContext playerContext)
        {
            base.Initialize(fsmManager, playerContext);
            StateType = StateType.Dodge;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ApplyDodgeImpulse();
            PlayerContext.HealthSystem.SetInvulnerable(true);
            PlayerContext.DodgeFeedback?.PlayFeedbacks();
        }

        public override void OnUpdate() { }

        public override void OnFixedUpdate()
        {
            ApplyRotation();
        }

        public override void OnExit()
        {
            PlayerContext.HealthSystem.SetInvulnerable(false);
            PlayerContext.DodgePressed = false;
            PlayerContext.DodgeFeedback?.StopFeedbacks();
        }
        public override void OnAnimatorIK(int layerIndex) { }

        private void ApplyDodgeImpulse()
        {
            Vector3 localDirection = new(PlayerContext.MoveInput.x, 0f, PlayerContext.MoveInput.y);

            if (localDirection == Vector3.zero)
                localDirection = Vector3.back;

            Vector3 worldDirection = PlayerContext.FsmPlayerManager.transform.TransformDirection(localDirection.normalized);

            PlayerContext.Rb.linearVelocity = new Vector3(0f, PlayerContext.Rb.linearVelocity.y, 0f);
            PlayerContext.Rb.AddForce(worldDirection * PlayerContext.Data.DodgeForce, ForceMode.Impulse);
        }

        private void ApplyRotation()
        {
            float angle = PlayerContext.LookInput.x * PlayerContext.Data.RotationSpeedX * Time.fixedDeltaTime;
            PlayerContext.FsmPlayerManager.transform.Rotate(Vector3.up, angle, Space.World);
        }
    }
}