using Assets.Scripts.Gameplay.Enemy.States;
using Assets.Scripts.Gameplay.System.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy.Skeleton
{
    public class SkeletonManager : FsmEnemyManager
    {
        public override EnemyStateType CombatStateType => EnemyStateType.Attack;

        protected override void RegisterStates(List<StateBase> states)
        {
            states.Add(new StateIdle());
            states.Add(new StateChase());
            states.Add(new StateAttack());
            states.Add(new StateDying());
        }

        public void OnAttackHit()
        {
            if (currentState.StateType != EnemyStateType.Attack) return;
            if (context.Player == null) return;

            float distance = Vector3.Distance(transform.position, context.Player.position);
            if (distance > enemySettings.AttackRange) return;

            if (context.Player.TryGetComponent<HealthSystem>(out var playerHealth))
                playerHealth.DoDamage(enemySettings.AttackDamage);

            if (context.Player.TryGetComponent<IKnockbackable>(out var knockback))
            {
                Vector3 direction = (transform.position - context.Player.position).normalized;
                knockback.ApplyKnockback(direction, enemySettings.AttackKnockback);
            }
        }
    }
}
