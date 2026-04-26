using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ProjectileSpell : SpellBase
    {
        protected Rigidbody rb;
        protected Vector3 origin;
        protected Vector3 initialVelocity;
        protected float elapsedTime;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            elapsedTime = 0f;
        }

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            this.origin = origin;
            initialVelocity = direction * spellSettings.Speed;
            transform.position = origin;
        }

        protected virtual void Update()
        {
            if (hasHit) return;
            elapsedTime += Time.deltaTime;
            rb.MovePosition(CalculatePosition(elapsedTime));
            transform.rotation = CalculateRotation(elapsedTime);
            if (elapsedTime >= spellSettings.LifeTime)
            {
                hasHit = true;
                OnHit();
                ReturnToPool();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (hasHit) return;
            foreach (ContactPoint hit in collision.contacts)
            {
                if (hit.thisCollider.TryGetComponent<HealthSystem>(out var health))
                    health.DoDamage(spellSettings.Damage);

                ApplyStatusEffect(hit.thisCollider);
            }
        }

        protected virtual Vector3 CalculatePosition(float t)
        {
            return origin + initialVelocity * t;
        }

        protected virtual Quaternion CalculateRotation(float t)
        {
            return initialVelocity != Vector3.zero
                ? Quaternion.LookRotation(initialVelocity)
                : transform.rotation;
        }
    }
}