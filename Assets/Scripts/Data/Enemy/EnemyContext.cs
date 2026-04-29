using Assets.Scripts.Gameplay.System.Elemental;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Gameplay.Enemy
{
    public class EnemyContext
    {
        public FsmEnemyManager Manager;
        public Animator Animator;
        public EnemySettingsSO Settings;
        public NavMeshAgent Agent;
        public Transform Player;
        public HealthSystem HealthSystem;
        public CapsuleCollider CapsuleCollider;
        public ElementalStateHandler StateHandler;
        public ReactionResolver Resolver;
    }
}
