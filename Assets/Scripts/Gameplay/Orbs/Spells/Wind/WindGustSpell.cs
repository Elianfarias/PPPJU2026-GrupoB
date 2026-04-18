using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Wind
{
    [RequireComponent(typeof(MMF_Player))]
    public class WindGustSpell : DirectionalSpell
    {
        [Header("Feel")]
        [SerializeField] private MMF_Player impactFeedbacks;

        protected override void OnHit()
        {
            impactFeedbacks.PlayFeedbacks(transform.position);
        }
    }
}
