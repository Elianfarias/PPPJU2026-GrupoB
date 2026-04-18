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
        [field: SerializeField] public AudioClip CastSpellSound { get; private set; }
        [field: SerializeField] public AudioClip SpellSound { get; private set; }
        [field: SerializeField] public AudioClip ImpactSpellSound { get; private set; }

        [Header("Status Effects")]
        [field: SerializeField] public StunEffect Stun { get; private set; }
        [field: SerializeField] public KnockbackEffect Knockback { get; private set; }
        [field: SerializeField] public SlowEffect Slow { get; private set; }
        [field: SerializeField] public BurnEffect Burn { get; private set; }
        [field: SerializeField] public RootEffect Root { get; private set; }

        public PlayerSpellPool GetPool() => PlayerSpellPool.Get(SpellId);
    }
}