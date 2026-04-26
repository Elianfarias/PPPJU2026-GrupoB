using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class AreaSpell : SpellBase
    {
        [Header("Area Config")]
        [SerializeField] protected float radius = 3f;
        [SerializeField] protected LayerMask enemyLayer;

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            transform.position = origin;
            Explode();
        }

        protected virtual void Explode()
        {
            var hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<HealthSystem>(out var healthSystem))
                    healthSystem.DoDamage(spellSettings.Damage);
            }
            OnHit();
            ReturnToPool();
        }
    }
}