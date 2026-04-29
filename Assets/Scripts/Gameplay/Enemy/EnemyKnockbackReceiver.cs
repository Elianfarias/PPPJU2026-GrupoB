using Assets.Scripts.Gameplay.System.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyKnockbackReceiver : MonoBehaviour, IKnockbackable
    {
        [SerializeField] private float recoveryTime = 0.4f;

        private Rigidbody rb;
        private NavMeshAgent agent;
        private Coroutine activeRoutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            rb.isKinematic = true;
        }

        public void ApplyKnockback(Vector3 direction, float force)
        {
            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            activeRoutine = StartCoroutine(KnockbackRoutine(direction, force));
        }

        private IEnumerator KnockbackRoutine(Vector3 direction, float force)
        {
            agent.enabled = false;
            rb.isKinematic = false;

            direction.y = 0f;
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);

            yield return new WaitForSeconds(recoveryTime);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            agent.enabled = true;

            activeRoutine = null;
        }
    }
}
