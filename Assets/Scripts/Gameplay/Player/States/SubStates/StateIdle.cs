using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Gameplay.Player.States.SubStates
{
    public class StateIdle : StateBase
    {
        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            StateType = StateType.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate() { }
        public override void OnFixedUpdate() { }
        public override void OnExit() { }
        public override void OnAnimatorIK(int layerIndex) { }
    }
}