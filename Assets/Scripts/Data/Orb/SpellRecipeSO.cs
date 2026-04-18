using Assets.Scripts.Data.Orb;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellRecipeSO", menuName = "OrbMage/Spell Recipe")]
public class SpellRecipeSO : ScriptableObject
{
    [field: SerializeField] public OrbElement ElementA { get; private set; }
    [field: SerializeField] public OrbElement ElementB { get; private set; }
    [field: SerializeField] public SpellSettingsSO Result { get; private set; }

    public bool Matches(OrbElement a, OrbElement b)
    {
        return (ElementA == a && ElementB == b) ||
               (ElementA == b && ElementB == a);
    }
}
