using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.Player;
using Assets.Scripts.Gameplay.System.Elemental;
using Assets.Scripts.Gameplay.System.Interfaces;
using Assets.Scripts.Gameplay.Systems.Interfaces;
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

        public virtual void ApplyStatusEffect(Collider target)
        {
            ApplyKnockback(target);
            ApplyElemental(target);
        }

        private void ApplyKnockback(Collider target)
        {
            if (!spellSettings.Knockback.Apply) return;
            if (!target.TryGetComponent<IKnockbackable>(out var knockback)) return;

            knockback.ApplyKnockback(direction, spellSettings.Knockback.Force);
        }

        private void ApplyElemental(Collider target)
        {
            if (!target.TryGetComponent<ElementalStateHandler>(out var handler)) return;

            if (target.TryGetComponent<ReactionResolver>(out var resolver))
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
    }
}
