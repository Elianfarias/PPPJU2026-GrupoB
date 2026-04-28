using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.System.Elemental;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class AreaSpell : SpellBase
    {
        [Header("Area Config")]
        [SerializeField] protected float radius = 3f;
        [SerializeField] protected LayerMask enemyLayer;

        private readonly HashSet<GameObject> hitTargets = new();

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            hitTargets.Clear();
        }

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            transform.position = origin;
            Explode();
        }

        protected virtual void Explode()
        {
            var hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

            foreach (var collider in hits)
            {
                GameObject hitRoot = collider.attachedRigidbody != null
                    ? collider.attachedRigidbody.gameObject
                    : collider.gameObject;

                if (!hitTargets.Add(hitRoot)) continue;

                if (hitRoot.TryGetComponent<HealthSystem>(out var health))
                    health.DoDamage(spellSettings.Damage);

                if (hitRoot.TryGetComponent<ElementalStateHandler>(out var handler))
                    ApplyStatusEffect(handler, hitRoot);
            }

            OnHit();
            ReturnAfterDelay(spellSettings.VfxDuration);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
