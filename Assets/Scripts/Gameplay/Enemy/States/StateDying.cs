using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateDying : StateBase
    {
        public override void Initialize(FsmEnemyManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, Transform player, HealthSystem healthSystem, CapsuleCollider capsuleCollider, Transform firePoint)
        {
            base.Initialize(fsmManager, animator, enemySettingsSO, agent, player, healthSystem, capsuleCollider, firePoint);

            this.stateType = EnemyStateType.Dying;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            capsuleCollider.enabled = false;
            agent.isStopped = true;
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnAnimatorIK(int layerIndex)
        {
        }
    }
}