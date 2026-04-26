using Assets.Scripts.Data.Orb;
using UnityEngine;

public interface ISpell
{
    void Execute(Vector3 origin, Vector3 direction, SpellSettingsSO spellSettings);
}