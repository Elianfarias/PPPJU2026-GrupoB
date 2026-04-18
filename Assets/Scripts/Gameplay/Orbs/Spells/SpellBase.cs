using Assets.Scripts.Data.Orb;
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
        }

        public virtual void ApplyStatusEffect(Collider target)
        {
            if (spellSettings.Stun.Apply && target.TryGetComponent<IStunnable>(out var stun))
                stun.ApplyStun(spellSettings.Stun.Duration);

            if (spellSettings.Knockback.Apply && target.TryGetComponent<IKnockbackable>(out var knockback))
                knockback.ApplyKnockback(direction, spellSettings.Knockback.Force);

            if (spellSettings.Slow.Apply && target.TryGetComponent<ISlowable>(out var slow))
                slow.ApplySlow(spellSettings.Slow.Multiplier, spellSettings.Slow.Duration);

            if (spellSettings.Burn.Apply && target.TryGetComponent<IBurnable>(out var burn))
                burn.ApplyBurn(spellSettings.Burn.DamagePerSecond, spellSettings.Burn.Duration);

            if (spellSettings.Root.Apply && target.TryGetComponent<IRootable>(out var root))
                root.ApplyRoot(spellSettings.Root.Duration);
        }

        protected virtual void OnHit() { }

        protected void ReturnToPool()
        {
            ownerPool.ReturnSpell(this);
        }
    }
}