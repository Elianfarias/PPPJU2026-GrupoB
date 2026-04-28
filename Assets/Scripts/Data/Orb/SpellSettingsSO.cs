using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.Player;
using Assets.Scripts.Gameplay.System.Enums;
using UnityEngine;

namespace Assets.Scripts.Data.Orb
{
    [CreateAssetMenu(fileName = "SpellSettingsSO", menuName = "OrbMage/Spell Settings")]
    public class SpellSettingsSO : ScriptableObject
    {
        [field: SerializeField] public string SpellName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public string SpellId { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public float VfxDuration { get; private set; } = 0.5f;

        [Header("Elemental")]
        [field: SerializeField] public ElementalNature Nature { get; private set; }
        [field: SerializeField] public ElementalStateData AppliedState { get; private set; }

        [Header("Knockback")]
        [field: SerializeField] public KnockbackEffect Knockback { get; private set; }

        [Header("Audio")]
        [field: SerializeField] public AudioClip CastSpellSound { get; private set; }
        [field: SerializeField] public AudioClip SpellSound { get; private set; }
        [field: SerializeField] public AudioClip ImpactSpellSound { get; private set; }

        public PlayerSpellPool GetPool() => PlayerSpellPool.Get(SpellId);
    }
}
