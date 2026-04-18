using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States.SubStates
{
    public class StateJumpingDown : StateBase
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.JumpingDown;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate() {
            Manager.SwitchState(StateType.Idle);
        }

        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
        }

        public override void OnAnimatorIK(int layerIndex) { }
    }
}