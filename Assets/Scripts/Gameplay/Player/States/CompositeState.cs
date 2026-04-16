using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.States
{
    public abstract class CompositeState : StateBase
    {
        protected readonly List<StateBase> SubStates = new();
        protected StateBase CurrentSubState;

        public override void Initialize(FsmPlayerManager manager, PlayerContext playerContext)
        {
            base.Initialize(manager, playerContext);
            RegisterSubStates();

            foreach (var state in SubStates)
                state.Initialize(manager, playerContext);
        }

        protected abstract void RegisterSubStates();
        protected abstract StateBase ResolveInitialSubState();

        public override void OnEnter()
        {
            base.OnEnter();
            CurrentSubState = ResolveInitialSubState();
            CurrentSubState.OnEnter();
        }

        public override void OnUpdate()
        {
            HandleParentTransitions();
            CurrentSubState?.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            OnParentFixedUpdate();
            CurrentSubState?.OnFixedUpdate();
        }

        public override void OnExit()
        {
            CurrentSubState?.OnExit();
            CurrentSubState = null;
        }

        public override void OnAnimatorIK(int layerIndex)
        {
            CurrentSubState?.OnAnimatorIK(layerIndex);
        }

        protected abstract void HandleParentTransitions();
        protected virtual void OnParentFixedUpdate() { }

        protected void SwitchSubState(StateType stateType)
        {
            var next = SubStates.Find(s => s.StateType == stateType);
            if (next == null || next == CurrentSubState) return;

            CurrentSubState?.OnExit();
            CurrentSubState = next;
            CurrentSubState.OnEnter();
        }

        public StateBase FindSubState(StateType stateType)
        {
            return SubStates.Find(s => s.StateType == stateType);
        }

        public void ReEnter()
        {
            PlayerContext.Animator.SetInteger(StateHash, (int)StateType);
        }
    }
}