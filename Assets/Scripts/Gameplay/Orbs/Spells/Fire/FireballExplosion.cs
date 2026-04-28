using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Fire
{
    public class FireballExplosion : AreaSpell
    {
        [Header("Feel")]
        [SerializeField] private MMF_Player impactFeedbacks;

        protected override void OnHit()
        {
            impactFeedbacks.PlayFeedbacks(transform.position);
            ReturnAfterDelay(spellSettings.VfxDuration);
        }
    }
}
