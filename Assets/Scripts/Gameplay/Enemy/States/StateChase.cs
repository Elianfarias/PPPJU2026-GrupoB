using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateChase : StateBase
    {
        private float nextPathUpdateTime;
        private const float PathUpdateInterval = 0.2f;

        public override void Initialize(EnemyContext context)
        {
            base.Initialize(context);
            StateType = EnemyStateType.Chase;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            context.Agent.isStopped = false;
            nextPathUpdateTime = 0f;
        }

        public override void OnUpdate()
        {
            if (!context.Agent.isOnNavMesh || !context.Agent.isActiveAndEnabled) return;
            if (TryTransitionToDying()) return;

            float distance = DistanceToPlayer();

            if (distance > context.Settings.PlayerLoseRadius)
            {
                var idle = context.Manager.FindState(EnemyStateType.Idle);
                context.Manager.SwitchState(idle);
                return;
            }

            if (distance <= context.Settings.AttackRange)
            {
                var attack = context.Manager.FindState(EnemyStateType.Attack);
                context.Manager.SwitchState(attack);
                return;
            }

            if (Time.time >= nextPathUpdateTime)
            {
                context.Agent.SetDestination(context.Player.position);
                nextPathUpdateTime = Time.time + PathUpdateInterval;
            }
        }

        public override void OnExit()
        {
            context.Agent.isStopped = true;
        }
    }
}
