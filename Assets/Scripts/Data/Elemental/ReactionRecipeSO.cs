using Assets.Scripts.Gameplay.System.Enums;
using UnityEngine;

namespace Assets.Scripts.Data.Elemental
{
    [CreateAssetMenu(fileName = "ReactionRecipeSO", menuName = "OrbMage/Reaction Recipe")]
    public class ReactionRecipeSO : ScriptableObject
    {
        [field: SerializeField] public string ReactionName { get; private set; }
        [field: SerializeField] public ElementalStateType RequiredState { get; private set; }
        [field: SerializeField] public ElementalNature IncomingNature { get; private set; }

        [Header("Output")]
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public LayerMask AffectedLayers { get; private set; }
        [field: SerializeField] public GameObject VfxPrefab { get; private set; }
        [field: SerializeField] public AudioClip Sound { get; private set; }

        public bool Matches(ElementalStateType existingState, ElementalNature incoming)
        {
            return RequiredState == existingState && IncomingNature == incoming;
        }
    }
}
