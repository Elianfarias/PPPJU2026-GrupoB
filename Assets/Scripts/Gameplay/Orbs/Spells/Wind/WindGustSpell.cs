using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Wind
{
    [RequireComponent(typeof(MMF_Player))]
    public class WindGustSpell : ProjectileSpell
    {
        [Header("Feel")]
        [SerializeField] private MMF_Player impactFeedbacks;

        [Header("VFX")]
        [SerializeField] private ParticleSystem[] spellParticles;

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            foreach (var ps in spellParticles)
            {
                ps.Clear();
                ps.Play();
            }
        }

        protected override void OnHit()
        {
            impactFeedbacks?.PlayFeedbacks(transform.position);
            ReturnAfterDelay(spellSettings.VfxDuration);
        }
    }
}