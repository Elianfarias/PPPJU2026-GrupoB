using Assets.Scripts.Data.Orb;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy
{
    [CreateAssetMenu(fileName = "EnemySettingsSO", menuName = "OrbMage/Enemy Settings")]
    public class EnemySettingsSO : ScriptableObject
    {
        [Header("Detection")]
        [SerializeField] private float playerDetectionRadius = 15f;
        [SerializeField] private float playerLoseRadius = 20f;
        [SerializeField] private LayerMask playerLayer;

        [Header("Combat")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackKnockback = 5f;
        [SerializeField] private string attackAnimationName = "Attack";

        [Header("Ranged")]
        [SerializeField] private SpellSettingsSO rangedProjectileSettings;

        public float PlayerDetectionRadius => playerDetectionRadius;
        public float PlayerLoseRadius => playerLoseRadius;
        public LayerMask PlayerLayer => playerLayer;

        public float AttackRange => attackRange;
        public float AttackCooldown => attackCooldown;
        public float AttackDamage => attackDamage;
        public float AttackKnockback => attackKnockback;
        public string AttackAnimationName => attackAnimationName;

        public SpellSettingsSO RangedProjectileSettings => rangedProjectileSettings;
    }
}
