using Assets.Scripts.Gameplay.Enemy.States;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Enemy.Golem
{
    public class GolemManager : FsmEnemyManager
    {
        public override EnemyStateType CombatStateType => EnemyStateType.RangedAttack;

        protected override void RegisterStates(List<StateBase> states)
        {
            states.Add(new StateIdle());
            states.Add(new StateChase());
            states.Add(new StateRangedAttack());
            states.Add(new StateDying());
        }

        public void OnRangedAttackFire()
        {
            if (currentState is StateRangedAttack ranged)
                ranged.FireProjectile();
        }
    }
}
