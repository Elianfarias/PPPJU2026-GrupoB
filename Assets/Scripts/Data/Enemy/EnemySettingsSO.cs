using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/Enemy")]
public class EnemySettingsSO : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private float obstacleDetectionRadius = 2f;
    [SerializeField] private float ropeDetectionRadius = 10f;
    [SerializeField] private float playerDetectionRadius = 50f;
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private float wanderCooldown = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldown = 1.5f;

    public int Damage { get { return damage; } }
    public float ObstacleDetectionRadius { get { return obstacleDetectionRadius; } }
    public float RopeDetectionRadius { get { return ropeDetectionRadius; } }
    public float PlayerDetectionRadius { get { return playerDetectionRadius; } }
    public float WanderRadius { get { return wanderRadius; } }
    public float WanderCooldown { get { return wanderCooldown; } }
    public LayerMask PlayerLayer { get { return playerLayer; } }
    public LayerMask EnemyLayer { get { return enemyLayer; } }
    public float AttackCooldown { get { return attackCooldown; } }
}
