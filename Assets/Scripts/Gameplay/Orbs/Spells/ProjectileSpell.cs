using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.System.Elemental;
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
        protected Vector3 hitPoint;
        protected Vector3 hitNormal;
        protected bool hitWasAffectable;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            elapsedTime = 0f;
            hitWasAffectable = false;
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

            ContactPoint contact = collision.GetContact(0);
            hitPoint = contact.point;
            hitNormal = contact.normal;

            GameObject hitRoot = collision.rigidbody != null
                ? collision.rigidbody.gameObject
                : collision.collider.gameObject;

            ResolveImpact(hitRoot);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasHit) return;

            GameObject hitRoot = other.attachedRigidbody != null
                ? other.attachedRigidbody.gameObject
                : other.gameObject;

            if (!hitRoot.TryGetComponent<ElementalStateHandler>(out _)) return;

            hitPoint = transform.position;
            hitNormal = -direction;

            ResolveImpact(hitRoot);
        }

        private void ResolveImpact(GameObject hitRoot)
        {
            hitWasAffectable = hitRoot.TryGetComponent<ElementalStateHandler>(out var handler);

            if (hitRoot.TryGetComponent<HealthSystem>(out var health))
                health.DoDamage(spellSettings.Damage);

            if (hitWasAffectable)
                ApplyStatusEffect(handler, hitRoot);

            hasHit = true;
            OnHit();
            ReturnToPool();
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