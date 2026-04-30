using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.System.Elemental;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Lightning
{
    public class LightningStrikeSpell : SpellBase
    {
        [Header("Strike Config")]
        [SerializeField] private float warningDelay = 0.6f;
        [SerializeField] private float impactRadius = 2.5f;
        [SerializeField] private LayerMask affectedLayers;

        [Header("VFX")]
        [SerializeField] private GameObject warningVfx;
        [SerializeField] private GameObject strikeVfx;

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            transform.position = origin;

            if (warningVfx != null) warningVfx.SetActive(true);
            if (strikeVfx != null) strikeVfx.SetActive(false);

            StartCoroutine(StrikeRoutine());
        }

        private IEnumerator StrikeRoutine()
        {
            yield return new WaitForSeconds(warningDelay);

            if (warningVfx != null) warningVfx.SetActive(false);
            if (strikeVfx != null) strikeVfx.SetActive(true);

            ResolveImpact();

            yield return new WaitForSeconds(spellSettings.VfxDuration);

            OnHit();
            ReturnToPool();
        }

        private void ResolveImpact()
        {
            var hits = Physics.OverlapSphere(transform.position, impactRadius, affectedLayers);

            foreach (var collider in hits)
            {
                GameObject hitRoot = collider.attachedRigidbody != null
                    ? collider.attachedRigidbody.gameObject
                    : collider.gameObject;

                if (hitRoot.TryGetComponent<HealthSystem>(out var health))
                    health.DoDamage(spellSettings.Damage);

                if (hitRoot.TryGetComponent<ElementalStateHandler>(out var handler))
                    ApplyStatusEffect(handler, hitRoot);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, impactRadius);
        }
    }
}