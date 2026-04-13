using Assets.Scripts.Gameplay.Orbs;
using UnityEngine;

    public class OrbSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private int maxAliveOrbs = 5;
        [SerializeField] private float spawnRadius = 10f;
        [SerializeField] private float spawnHeightOffset = 1f;

        [Header("Element Weights")]
        [SerializeField] private ElementWeight[] elementWeights;

        private int aliveOrbCount = 0;
        private float timer = 0f;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer < spawnInterval) return;

            timer = 0f;
            TrySpawn();
        }

        private void TrySpawn()
        {
            if (aliveOrbCount >= maxAliveOrbs) return;

            var pool = PickPoolByWeight();
            if (pool == null) return;

            var orb = pool.GetOrb();
            orb.transform.position = GetRandomPositionInRadius();
            orb.OnGetFromPool();

            aliveOrbCount++;
            orb.OnReturned += OnOrbReturned;
        }

        private OrbPool PickPoolByWeight()
        {
            float totalWeight = 0f;
            foreach (var entry in elementWeights)
                totalWeight += entry.Weight;

            float roll = Random.Range(0f, totalWeight);
            float accumulated = 0f;

            foreach (var entry in elementWeights)
            {
                accumulated += entry.Weight;
                if (roll <= accumulated) return entry.Pool;
            }

            return null;
        }

        private Vector3 GetRandomPositionInRadius()
        {
            var randomCircle = Random.insideUnitCircle * spawnRadius;
            return transform.position + new Vector3(randomCircle.x, spawnHeightOffset, randomCircle.y);
        }

        private void OnOrbReturned()
        {
            aliveOrbCount--;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.2f);
            Gizmos.DrawSphere(transform.position, spawnRadius);
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 1f);
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }