using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States.SubStates
{
    public class StateFall : StateBase
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Fall;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate() { }
        public override void OnFixedUpdate() { }

        public override void OnExit()
        {
            PlayerContext.JumpPressed = false;
        }

        public override void OnAnimatorIK(int layerIndex) { }
    }
}