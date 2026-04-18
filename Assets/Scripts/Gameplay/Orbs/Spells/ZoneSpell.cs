using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class ZoneSpell : SpellBase
    {
        [Header("Zone Config")]
        [SerializeField] protected float duration = 3f;
        [SerializeField]
        protected float tickInterval = 0.5f;

        protected float elapsedTime;
        protected float nextTick;

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            elapsedTime = 0f;
            nextTick = 0f;
        }

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            transform.position = origin;
        }

        protected virtual void Update()
        {
            elapsedTime += Time.deltaTime;

            if (Time.time >= nextTick)
            {
                nextTick = Time.time + tickInterval;
                OnTick();
            }

            if (elapsedTime >= duration)
            {
                OnHit();
                ReturnToPool();
            }
        }

        protected virtual void OnTick() { }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<HealthSystem>(out var healthSystem))
                healthSystem.DoDamage(spellSettings.Damage * tickInterval);
        }
    }
}