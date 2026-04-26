using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy.States
{
    public abstract class StateBase
    {
        protected static readonly int State = Animator.StringToHash("State");
        public EnemyStateType stateType;

        protected FsmEnemyManager fsmManager;
        protected Animator animator;
        protected EnemySettingsSO enemySettingsSO;
        protected NavMeshAgent agent;
        protected HealthSystem healthSystem;
        protected Transform player;
        protected CapsuleCollider capsuleCollider;
        protected Transform firePoint;

        public virtual void Initialize(FsmEnemyManager fsmManager,
            Animator animator,
            EnemySettingsSO enemySettingsSO,
            NavMeshAgent agent,
            Transform player,
            HealthSystem healthSystem,
            CapsuleCollider capsuleCollider,
            Transform firePoint
            )
        {
            this.fsmManager = fsmManager;
            this.animator = animator;
            this.enemySettingsSO = enemySettingsSO;
            this.agent = agent;
            this.player = player;
            this.healthSystem = healthSystem;
            this.capsuleCollider = capsuleCollider;
            this.firePoint = firePoint;
        }

        public virtual void OnEnter()
        {
            animator.SetInteger(State, (int)stateType);
        }
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void OnAnimatorIK(int layerIndex);

        protected bool IsPlayerNearby()
        {
            return Physics.CheckSphere(fsmManager.transform.position, enemySettingsSO.PlayerDetectionRadius, enemySettingsSO.PlayerLayer);
        }
    }
}