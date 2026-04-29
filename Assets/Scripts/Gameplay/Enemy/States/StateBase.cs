using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public abstract class StateBase
    {
        protected static readonly int StateHash = Animator.StringToHash("State");

        public EnemyStateType StateType { get; protected set; }

        protected EnemyContext context;

        public virtual void Initialize(EnemyContext context)
        {
            this.context = context;
        }

        public virtual void OnEnter()
        {
            context.Animator.SetInteger(StateHash, (int)StateType);
        }

        public abstract void OnUpdate();
        public abstract void OnExit();

        public virtual void OnAnimatorIK(int layerIndex) { }

        protected bool IsPlayerInRange(float radius)
        {
            return Physics.CheckSphere(
                context.Manager.transform.position,
                radius,
                context.Settings.PlayerLayer
            );
        }

        protected float DistanceToPlayer()
        {
            return Vector3.Distance(context.Manager.transform.position, context.Player.position);
        }

        protected bool TryTransitionToDying()
        {
            if (context.HealthSystem.GetCurrentLife() > 0) return false;

            var dying = context.Manager.FindState(EnemyStateType.Dying);
            context.Manager.SwitchState(dying);
            return true;
        }
    }
}
