using MoreMountains.Feedbacks;
using System.Collections;
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
        [SerializeField] private float vfxDuration = 1f;

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
            StartCoroutine(ReturnAfterVFX());
        }

        private IEnumerator ReturnAfterVFX()
        {
            yield return new WaitForSeconds(vfxDuration);
            ReturnToPool();
        }
    }
}
