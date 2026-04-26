using Assets.Scripts.Gameplay.System.Enums;
using UnityEngine;

namespace Assets.Scripts.Data.Elemental
{
    [CreateAssetMenu(fileName = "ElementalStateData", menuName = "OrbMage/Elemental State")]
    public class ElementalStateData : ScriptableObject
    {
        [field: SerializeField] public ElementalStateType Type { get; private set; }
        [field: SerializeField] public float DefaultDuration { get; private set; } = 3f;
        [field: SerializeField] public float TickInterval { get; private set; } = 1f;
        [field: SerializeField] public float DamagePerTick { get; private set; }
        [field: SerializeField] public float SlowMultiplier { get; private set; } = 1f;
        [field: SerializeField] public GameObject VfxPrefab { get; private set; }
    }
}
