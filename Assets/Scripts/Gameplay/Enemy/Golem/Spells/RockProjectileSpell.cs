using Assets.Scripts.Gameplay.Orbs.Spells;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy.Golem.Spells
{
    public class RockProjectileSpell : ProjectileSpell
    {
        [Header("Splash VFX")]
        [SerializeField] private GameObject splashVfxPrefab;
        [SerializeField] private float splashLifetime = 1.5f;

        protected override void OnHit()
        {
            base.OnHit();

            SpawnSplash(hitPoint, hitNormal);
        }


        private void SpawnSplash(Vector3 position, Vector3 normal)
        {
            if (splashVfxPrefab == null) return;

            var splash = Instantiate(splashVfxPrefab, position, Quaternion.LookRotation(normal));
            Destroy(splash, splashLifetime);
        }
    }
}