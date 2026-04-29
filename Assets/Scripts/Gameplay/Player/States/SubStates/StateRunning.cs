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
            ApplyMovement(PlayerContext.Data.MaxHorizontalSpeed,
                          PlayerContext.Data.Acceleration);
        }

        public override void OnExit() { }
        public override void OnAnimatorIK(int layerIndex) { }

        private void ApplyMovement(float speed, float accel)
        {
            Vector3 worldDir = CameraRelativeDirection(PlayerContext.MoveInput);
            Vector3 target = worldDir * speed;

            Vector3 current = new(PlayerContext.Rb.linearVelocity.x, 0f, PlayerContext.Rb.linearVelocity.z);
            Vector3 newHoriz = Vector3.MoveTowards(current, target, accel * Time.fixedDeltaTime);

            PlayerContext.Rb.linearVelocity = new Vector3(newHoriz.x, PlayerContext.Rb.linearVelocity.y, newHoriz.z);
        }

        private Vector3 CameraRelativeDirection(Vector2 input)
        {
            Transform cam = PlayerContext.CameraTransform;

            Vector3 camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;

            return (camForward * input.y + camRight * input.x).normalized;
        }
    }
}