using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public class FireballSpell : ProjectileSpell
    {
        [SerializeField] private GameObject explosionPrefab;

        protected override void OnHit()
        {
            Explode();
        }

        private void Explode()
        {
            hasHit = true;

            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            ReturnToPool();
        }
    }
}