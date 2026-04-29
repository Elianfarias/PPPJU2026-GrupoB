using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Water
{
    public class WaterProjectileSpell : ProjectileSpell
    {
        [Header("Puddle Payload")]
        [SerializeField] private SpellSettingsSO puddleSettings;
        [SerializeField] private float puddleSurfaceOffset = 0.05f;

        [Header("Splash VFX")]
        [SerializeField] private GameObject splashVfxPrefab;
        [SerializeField] private float splashLifetime = 1.5f;

        protected override void OnHit()
        {
            base.OnHit();

            SpawnSplash(hitPoint, hitNormal);

            if (!hitWasAffectable)
                SpawnPuddle(hitPoint + hitNormal * puddleSurfaceOffset);
        }

        private void SpawnPuddle(Vector3 position)
        {
            if (puddleSettings == null) return;

            var pool = puddleSettings.GetPool();
            if (pool == null)
            {
                Debug.LogError($"No hay pool registrado para SpellId: {puddleSettings.SpellId}");
                return;
            }

            var puddle = pool.GetSpell();
            puddle.Execute(position, Vector3.up, puddleSettings);
        }

        private void SpawnSplash(Vector3 position, Vector3 normal)
        {
            if (splashVfxPrefab == null) return;

            var splash = Instantiate(splashVfxPrefab, position, Quaternion.LookRotation(normal));
            Destroy(splash, splashLifetime);
        }
    }
}