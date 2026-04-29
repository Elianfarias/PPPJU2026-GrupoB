using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public class StateAttack : StateBase
    {
        private bool isAttacking;
        private int attackHash;

        public override void Initialize(EnemyContext context)
        {
            base.Initialize(context);
            StateType = EnemyStateType.Attack;
            attackHash = Animator.StringToHash(context.Settings.AttackAnimationName);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            context.Agent.isStopped = true;
            isAttacking = false;
        }

        public override void OnUpdate()
        {
            if (TryTransitionToDying()) return;

            float distance = DistanceToPlayer();

            if (distance > context.Settings.AttackRange)
            {
                var chase = context.Manager.FindState(EnemyStateType.Chase);
                context.Manager.SwitchState(chase);
                return;
            }

            RotateToPlayer();

            if (!isAttacking)
                context.Manager.StartManagedCoroutine(AttackRoutine());
        }

        public override void OnExit()
        {
            isAttacking = false;
        }

        private void RotateToPlayer()
        {
            Vector3 direction = context.Player.position - context.Manager.transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude < 0.01f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            context.Manager.transform.rotation = Quaternion.RotateTowards(
                context.Manager.transform.rotation,
                targetRotation,
                context.Agent.angularSpeed * Time.deltaTime
            );
        }

        private IEnumerator AttackRoutine()
        {
            isAttacking = true;
            context.Animator.Play(attackHash, 0, 0f);

            yield return new WaitForSeconds(context.Settings.AttackCooldown);

            isAttacking = false;
        }
    }
}
