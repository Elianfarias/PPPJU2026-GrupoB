using UnityEngine;

[CreateAssetMenu(fileName = "SpellSettingsSO", menuName = "OrbMage/Spell Settings")]
public class SpellSettingsSO : ScriptableObject
{
    [field: SerializeField] public string SpellName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public GameObject VFXPrefab { get; private set; }
}
