using UnityEngine;

[CreateAssetMenu(fileName = "SpellSettingsSO", menuName = "OrbMage/Spell Settings")]
public class SpellSettingsSO : ScriptableObject
{
    [field: SerializeField] public string SpellName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string SpellId { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    public PlayerSpellPool GetPool() => PlayerSpellPool.Get(SpellId);
}
