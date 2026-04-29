using Assets.Scripts.Data.Orb;
using Assets.Scripts.Gameplay.System.Elemental;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Water
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ElementalStateHandler))]
    [RequireComponent(typeof(ReactionResolver))]
    public class WaterPuddle : SpellBase
    {
        [Header("Puddle")]
        [SerializeField] private float lifeTime = 8f;

        private ElementalStateHandler handler;
        private float spawnTime;
        private bool initialized;

        protected virtual void Awake()
        {
            handler = GetComponent<ElementalStateHandler>();
            handler.OnStateRemoved += OnHandlerStateRemoved;
        }

        private void OnDestroy()
        {
            if (handler != null)
                handler.OnStateRemoved -= OnHandlerStateRemoved;
        }

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            initialized = false;
        }

        public override void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings)
        {
            base.Execute(origin, direction, spellSettings);

            transform.position = origin;
            spawnTime = Time.time;

            if (spellSettings.AppliedState != null)
                handler.ApplyState(spellSettings.AppliedState, gameObject);

            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            if (Time.time - spawnTime >= lifeTime)
                ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!initialized) return;
            if (spellSettings.AppliedState == null) return;

            GameObject root = other.attachedRigidbody != null
                ? other.attachedRigidbody.gameObject
                : other.gameObject;

            if (root == gameObject) return;

            if (root.TryGetComponent<ElementalStateHandler>(out var otherHandler))
                otherHandler.ApplyState(spellSettings.AppliedState, gameObject);
        }

        private void OnHandlerStateRemoved(ActiveElementalState state)
        {
            if (!initialized) return;
            if (spellSettings.AppliedState == null) return;
            if (state.Type != spellSettings.AppliedState.Type) return;

            initialized = false;
            ReturnToPool();
        }
    }
}