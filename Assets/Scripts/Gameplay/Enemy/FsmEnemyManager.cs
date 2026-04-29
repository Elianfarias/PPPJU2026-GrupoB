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
    public abstract class FsmEnemyManager : MonoBehaviour
    {
        [SerializeField] protected EnemySettingsSO enemySettings;
        [SerializeField] protected Transform firePoint;

        protected EnemyContext context;
        protected readonly List<StateBase> stateBases = new();
        protected StateBase currentState;
        protected float blindUntil;

        public bool IsBlind => Time.time < blindUntil;
        public Transform FirePoint => firePoint;

        public abstract EnemyStateType CombatStateType { get; }

        protected virtual void Awake()
        {
            context = BuildContext();
            RegisterStates(stateBases);
            InitializeStates();
            currentState = FindState(EnemyStateType.Idle);
            currentState.OnEnter();
        }

        protected virtual void Update() => currentState.OnUpdate();

        protected virtual void OnAnimatorIK(int layerIndex) => currentState.OnAnimatorIK(layerIndex);

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

        public void LoseAggro(float duration)
        {
            blindUntil = Time.time + duration;

            var idle = FindState(EnemyStateType.Idle);
            if (idle != null)
                SwitchState(idle);
        }

        protected abstract void RegisterStates(List<StateBase> states);

        protected virtual EnemyContext BuildContext()
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

        private void InitializeStates()
        {
            foreach (var state in stateBases)
                state.Initialize(context);
        }
    }
}
