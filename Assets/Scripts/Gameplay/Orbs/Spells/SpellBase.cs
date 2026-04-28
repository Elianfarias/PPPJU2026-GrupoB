using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.Player;
using Assets.Scripts.Gameplay.System.Elemental;
using Assets.Scripts.Gameplay.System.Interfaces;
using Assets.Scripts.Gameplay.Systems.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class SpellBase : MonoBehaviour, IPoolable, ISpell
    {
        [SerializeField] protected SpellSettingsSO spellSettings;

        protected bool hasHit;
        protected Vector3 direction;
        private PlayerSpellPool ownerPool;

        public void SetPool(PlayerSpellPool pool) => ownerPool = pool;

        public virtual void OnGetFromPool()
        {
            hasHit = false;
            foreach (var col in GetComponents<Collider>())
                col.enabled = true;
        }

        public virtual void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            this.spellSettings = spellSettings;
            this.direction = direction;
        }

        public virtual void ApplyStatusEffect(ElementalStateHandler handler, GameObject hitRoot)
        {
            ApplyKnockback(hitRoot);
            ApplyElemental(handler, hitRoot);
        }

        private void ApplyKnockback(GameObject hitRoot)
        {
            if (!spellSettings.Knockback.Apply) return;
            if (!hitRoot.TryGetComponent<IKnockbackable>(out var knockback)) return;

            knockback.ApplyKnockback(direction, spellSettings.Knockback.Force);
        }

        private void ApplyElemental(ElementalStateHandler handler, GameObject hitRoot)
        {
            if (hitRoot.TryGetComponent<ReactionResolver>(out var resolver))
            {
                if (resolver.TryResolve(spellSettings.Nature, gameObject)) return;
            }

            if (spellSettings.AppliedState != null)
                handler.ApplyState(spellSettings.AppliedState, gameObject);
        }

        protected virtual void OnHit() { }

        protected void ReturnToPool()
        {
            ownerPool.ReturnSpell(this);
        }

        protected void ReturnAfterDelay(float delay)
        {
            if (delay <= 0f)
            {
                ReturnToPool();
                return;
            }
            StartCoroutine(ReturnAfterDelayRoutine(delay));
        }

        private IEnumerator ReturnAfterDelayRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool();
        }
    }
}
