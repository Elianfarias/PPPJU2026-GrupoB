using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class DirectionalSpell : SpellBase
    {
        [Header("Directional Config")]
        [SerializeField] protected float range = 5f;
        [SerializeField] protected LayerMask enemyLayer;

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            this.direction = direction;
            transform.position = origin;
            ApplyEffect();
        }

        protected virtual void ApplyEffect()
        {
            var hits = Physics.SphereCastAll(
                transform.position,
                1f,
                direction,
                range,
                enemyLayer
            );

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<HealthSystem>(out var health))
                    health.DoDamage(spellSettings.Damage);

                ApplyStatusEffect(hit.collider);
            }

            OnHit();
            ReturnToPool();
        }
    }
}