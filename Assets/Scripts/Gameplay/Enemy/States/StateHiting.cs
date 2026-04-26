using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateHiting : StateBase
    {
        private bool isAttacking = false;
        private readonly string animationName = "Throw";
        public override void Initialize(FsmEnemyManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, Transform player, HealthSystem healthSystem, CapsuleCollider capsuleCollider, Transform firePoint)
        {
            base.Initialize(fsmManager, animator, enemySettingsSO, agent, player, healthSystem, capsuleCollider, firePoint);

            this.stateType = EnemyStateType.Hiting;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            isAttacking = false;
        }

        public override void OnUpdate()
        {
            if (healthSystem.GetCurrentLife() <= 0)
            {
                fsmManager.SwitchState(fsmManager.FindState(EnemyStateType.Dying));
                return;
            }

            if (!IsPlayerNearby())
            {
                fsmManager.SwitchState(fsmManager.FindState(EnemyStateType.Running));
                return;
            }

            Vector3 direction = GetDirectionToPlayer();
            RotateToPlayer(direction);

            if (!isAttacking)
                fsmManager.StartManagedCoroutine(AttackRoutine());
        }



        public override void OnExit()
        {
            isAttacking = false;
        }

        public override void OnAnimatorIK(int layerIndex)
        {
            if (player == null) return;

            animator.SetLookAtWeight(1f, 0.3f, 0.6f, 1f, 0.5f);
            animator.SetLookAtPosition(player.transform.position);
        }

        private Vector3 GetDirectionToPlayer()
        {
            Vector3 direction = (player.position - firePoint.transform.position).normalized;
            return direction;
        }

        private void RotateToPlayer(Vector3 direction)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            firePoint.rotation = Quaternion.RotateTowards(
                firePoint.rotation,
                 targetRotation,
                 agent.angularSpeed * Time.deltaTime
             );
        }

        private IEnumerator AttackRoutine()
        {
            animator.Play(animationName, 0, 0f);
            isAttacking = true;

            yield return new WaitForSeconds(enemySettingsSO.AttackCooldown);

            isAttacking = false;
        }
    }
}