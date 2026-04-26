using Assets.Scripts.Data.Orb;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellBookSO", menuName = "OrbMage/Spell Book")]
public class SpellBookSO : ScriptableObject
{
    [Serializable]
    private struct ElementSpellEntry
    {
        public OrbElement Element;
        public SpellSettingsSO Spell;
    }

    [SerializeField] private ElementSpellEntry[] entries;

    public bool TryGetSpell(OrbElement element, out SpellSettingsSO spell)
    {
        foreach (var entry in entries)
        {
            if (entry.Element != element) continue;
            spell = entry.Spell;
            return true;
        }
        spell = null;
        return false;
    }
}