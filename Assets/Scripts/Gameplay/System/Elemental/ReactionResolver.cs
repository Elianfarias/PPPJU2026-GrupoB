using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.System.Enums;
using UnityEngine;

namespace Assets.Scripts.Gameplay.System.Elemental
{
    [RequireComponent(typeof(ElementalStateHandler))]
    public class ReactionResolver : MonoBehaviour
    {
        [SerializeField] private ReactionRecipeSO[] recipes;

        private ElementalStateHandler handler;

        private void Awake()
        {
            handler = GetComponent<ElementalStateHandler>();
        }

        public bool TryResolve(ElementalNature incomingNature, GameObject source)
        {
            if (incomingNature == ElementalNature.None) return false;
            if (recipes == null || recipes.Length == 0) return false;

            foreach (var existing in handler.ActiveStates)
            {
                foreach (var recipe in recipes)
                {
                    if (!recipe.Matches(existing.Type, incomingNature)) continue;

                    ExecuteReaction(recipe, source);
                    handler.RemoveState(existing.Type);
                    return true;
                }
            }
            return false;
        }

        private void ExecuteReaction(ReactionRecipeSO recipe, GameObject source)
        {
            if (recipe.VfxPrefab != null)
                Instantiate(recipe.VfxPrefab, transform.position, Quaternion.identity);

            if (recipe.Sound != null)
                AudioSource.PlayClipAtPoint(recipe.Sound, transform.position);

            if (recipe.Damage <= 0f || recipe.Radius <= 0f) return;

            var hits = Physics.OverlapSphere(transform.position, recipe.Radius, recipe.AffectedLayers);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<HealthSystem>(out var health))
                    health.DoDamage(recipe.Damage);
            }
        }
    }
}
