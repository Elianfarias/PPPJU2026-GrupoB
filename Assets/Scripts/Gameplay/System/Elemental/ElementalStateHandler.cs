using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.System.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.System.Elemental
{
    [RequireComponent(typeof(HealthSystem))]
    public class ElementalStateHandler : MonoBehaviour
    {
        private readonly List<ActiveElementalState> activeStates = new();

        private HealthSystem healthSystem;

        public event Action<ActiveElementalState> OnStateApplied;
        public event Action<ActiveElementalState> OnStateRemoved;

        public IReadOnlyList<ActiveElementalState> ActiveStates => activeStates;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        private void Update()
        {
            TickActiveStates();
        }

        public void ApplyState(ElementalStateData data, GameObject source)
        {
            if (data == null || data.Type == ElementalStateType.None) return;

            if (TryGetState(data.Type, out var existing))
            {
                existing.Refresh();
                return;
            }

            GameObject vfx = SpawnVfx(data);
            var newState = new ActiveElementalState(data, source, vfx);
            activeStates.Add(newState);
            OnStateApplied?.Invoke(newState);

        }

        public bool RemoveState(ElementalStateType type)
        {
            if (!TryGetState(type, out var state)) return false;

            activeStates.Remove(state);
            DespawnVfx(state);
            OnStateRemoved?.Invoke(state);

            return true;
        }

        public bool HasState(ElementalStateType type)
        {
            return TryGetState(type, out _);
        }

        public bool TryGetState(ElementalStateType type, out ActiveElementalState state)
        {
            foreach (var active in activeStates)
            {
                if (active.Type != type) continue;
                state = active;
                return true;
            }
            state = null;
            return false;
        }

        private void TickActiveStates()
        {
            for (int i = activeStates.Count - 1; i >= 0; i--)
            {
                var state = activeStates[i];

                if (state.ShouldTickDamage())
                    healthSystem.DoDamage(state.Data.DamagePerTick);

                state.Tick(Time.deltaTime);

                if (state.IsExpired)
                {
                    activeStates.RemoveAt(i);
                    DespawnVfx(state);
                    OnStateRemoved?.Invoke(state);
                }
            }
        }

        private GameObject SpawnVfx(ElementalStateData data)
        {
            if (data.VfxPrefab == null) return null;
            return Instantiate(data.VfxPrefab, transform.position, Quaternion.identity, transform);
        }

        private void DespawnVfx(ActiveElementalState state)
        {
            if (state.VfxInstance != null)
                Destroy(state.VfxInstance);
        }
    }
}
