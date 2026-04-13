using UnityEngine;

[CreateAssetMenu(fileName = "SpellBookSO", menuName = "OrbMage/Spell Book")]
public class SpellBookSO : ScriptableObject
{
    [SerializeField] private SpellRecipeSO[] recipes;

    public bool TryGetSpell(OrbElement a, OrbElement b, out SpellSettingsSO result)
    {
        foreach (var recipe in recipes)
        {
            if (!recipe.Matches(a, b)) continue;

            result = recipe.Result;
            return true;
        }

        result = null;
        return false;
    }
}
