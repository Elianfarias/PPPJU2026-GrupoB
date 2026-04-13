
using UnityEngine;

[CreateAssetMenu(fileName = "OrbSettingsSO", menuName = "OrbMage/Orb Settings")]
public class OrbSettingsSO : ScriptableObject
{
    [field: SerializeField] public OrbElement Element { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}