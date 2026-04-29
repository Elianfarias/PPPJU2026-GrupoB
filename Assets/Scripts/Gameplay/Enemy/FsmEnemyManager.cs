using Assets.Scripts.Gameplay.Enemy.States;
using Assets.Scripts.Gameplay.System.Elemental;
using Assets.Scripts.Gameplay.System.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(ElementalStateHandler))]
    public class FsmEnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemySettingsSO enemySettings;

        private EnemyContext context;
        private readonly List<StateBase> stateBases = new();
        private StateBase currentState;
        private float blindUntil;

        public bool IsBlind => Time.time < blindUntil;

        private void Awake()
        {
            context = BuildContext();
            RegisterStates();
            InitializeStates();
            currentState = FindState(EnemyStateType.Idle);
            currentState.OnEnter();
        }

        private void Update() => currentState.OnUpdate();

        private void OnAnimatorIK(int layerIndex) => currentState.OnAnimatorIK(layerIndex);

        public void SwitchState(StateBase newState)
        {
            if (newState == null || currentState == newState) return;

            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        public StateBase FindState(EnemyStateType stateType)
        {
            foreach (var state in stateBases)
                if (state.StateType == stateType) return state;
            return null;
        }

        public Coroutine StartManagedCoroutine(IEnumerator routine) => StartCoroutine(routine);

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

        public void LoseAggro(float duration)
        {
            blindUntil = Time.time + duration;

            var idle = FindState(EnemyStateType.Idle);
            if (idle != null)
                SwitchState(idle);
        }

        private EnemyContext BuildContext()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            return new EnemyContext
            {
                Manager = this,
                Animator = GetComponent<Animator>(),
                Settings = enemySettings,
                Agent = GetComponent<NavMeshAgent>(),
                Player = player != null ? player.transform : null,
                HealthSystem = GetComponent<HealthSystem>(),
                CapsuleCollider = GetComponent<CapsuleCollider>(),
                StateHandler = GetComponent<ElementalStateHandler>(),
                Resolver = GetComponent<ReactionResolver>()
            };
        }

        private void RegisterStates()
        {
            stateBases.Add(new StateIdle());
            stateBases.Add(new StateChase());
            stateBases.Add(new StateAttack());
            stateBases.Add(new StateDying());
        }

        private void InitializeStates()
        {
            foreach (var state in stateBases)
                state.Initialize(context);
        }
    }
}