using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.System.Elemental;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Fire
{
    public class FireballSpell : ProjectileSpell
    {
        [Header("Explosion")]
        [SerializeField] private SpellSettingsSO explosionSettings;

        public override void ApplyStatusEffect(ElementalStateHandler handler, GameObject hitRoot)
        {
        }

        protected override void OnHit()
        {
            base.OnHit();
            SpawnExplosion();
        }

        private void SpawnExplosion()
        {
            if (explosionSettings == null) return;

            var pool = explosionSettings.GetPool();
            if (pool == null)
            {
                Debug.LogError($"No hay pool registrado para SpellId: {explosionSettings.SpellId}");
                return;
            }

            var explosion = pool.GetSpell();
            explosion.Execute(transform.position, Vector3.up, explosionSettings);
        }
    }
}
