using Assets.Scripts.Gameplay.System.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerKnockbackReceiver : MonoBehaviour, IKnockbackable
    {
        [SerializeField] private float recoveryTime = 0.25f;

        private Rigidbody rb;
        private FsmPlayerManager manager;
        private bool isRecovering;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            manager = GetComponent<FsmPlayerManager>();
        }

        public void ApplyKnockback(Vector3 direction, float force)
        {
            if (isRecovering) return;
            StartCoroutine(KnockbackRoutine(direction, force));
        }

        private IEnumerator KnockbackRoutine(Vector3 direction, float force)
        {
            isRecovering = true;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(-direction.normalized * force, ForceMode.Impulse);

            yield return new WaitForSeconds(recoveryTime);

            isRecovering = false;
        }
    }
}