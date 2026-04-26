using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.System.Enums;
using UnityEngine;

namespace Assets.Scripts.Gameplay.System.Elemental
{
    public class ActiveElementalState
    {
        public ElementalStateData Data { get; private set; }
        public GameObject Source { get; private set; }
        public GameObject VfxInstance { get; private set; }

        public float RemainingDuration { get; private set; }
        public float NextTickTime { get; private set; }

        public ElementalStateType Type => Data.Type;
        public bool IsExpired => RemainingDuration <= 0f;

        public ActiveElementalState(ElementalStateData data, GameObject source, GameObject vfxInstance)
        {
            Data = data;
            Source = source;
            VfxInstance = vfxInstance;
            RemainingDuration = data.DefaultDuration;
            NextTickTime = Time.time + data.TickInterval;
        }

        public void Tick(float deltaTime)
        {
            RemainingDuration -= deltaTime;
        }

        public bool ShouldTickDamage()
        {
            if (Data.DamagePerTick <= 0f) return false;
            if (Time.time < NextTickTime) return false;

            NextTickTime = Time.time + Data.TickInterval;
            return true;
        }

        public void Refresh()
        {
            float incoming = Data.DefaultDuration;
            if (incoming > RemainingDuration)
                RemainingDuration = incoming;
        }
    }
}
