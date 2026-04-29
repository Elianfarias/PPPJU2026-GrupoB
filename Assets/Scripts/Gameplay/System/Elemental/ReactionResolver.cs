using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.Enemy;
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

            if (recipe.Radius <= 0f) return;

            var hits = Physics.OverlapSphere(transform.position, recipe.Radius, recipe.AffectedLayers);
            foreach (var hit in hits)
            {
                ApplyDamage(hit, recipe);
                ApplyBlind(hit, recipe);
            }
        }

        private static void ApplyDamage(Collider hit, ReactionRecipeSO recipe)
        {
            if (recipe.Damage <= 0f) return;
            if (hit.TryGetComponent<HealthSystem>(out var health))
                health.DoDamage(recipe.Damage);
        }

        private static void ApplyBlind(Collider hit, ReactionRecipeSO recipe)
        {
            if (!recipe.BlindsTargets) return;

            GameObject root = hit.attachedRigidbody != null
                ? hit.attachedRigidbody.gameObject
                : hit.gameObject;

            if (root.TryGetComponent<FsmEnemyManager>(out var enemy))
                enemy.LoseAggro(recipe.BlindDuration);
        }
    }
}