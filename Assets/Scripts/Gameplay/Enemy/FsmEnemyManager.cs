using Assets.Scripts.Gameplay.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy
{
    public class FsmEnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemySettingsSO enemySettingsSO;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator animator;
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private Transform firePoint;

        private readonly IList<StateBase> stateBases = new List<StateBase>();
        private StateBase currentState;

        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");


            stateBases.Add(new StateRunning());
            stateBases.Add(new StateHiting());
            stateBases.Add(new StateDying());

            foreach (var state in stateBases)
                state.Initialize(this, animator, enemySettingsSO, _agent, player.transform, healthSystem, capsuleCollider, firePoint);

            currentState = FindState(EnemyStateType.Running);
        }

        private void Update()
        {
            currentState.OnUpdate();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            currentState.OnAnimatorIK(layerIndex);
        }

        public void SwitchState(StateBase state)
        {
            if (currentState == state) return;

            currentState.OnExit();
            currentState = state;
            currentState.OnEnter();
        }

        public StateBase FindState(EnemyStateType stateType)
        {
            foreach (var state in stateBases)
            {
                if (state.stateType == stateType)
                    return state;
            }

            return null;
        }

        public Coroutine StartManagedCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
    }
}