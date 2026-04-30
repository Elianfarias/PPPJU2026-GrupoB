using Assets.Scripts.Data.Orb;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells.Fire
{
    public class FireballExplosion : AreaSpell
    {
        [Header("Feel")]
        [SerializeField] private MMF_Player impactFeedbacks;

        [Header("Fire Patch Payload")]
        [SerializeField] private SpellSettingsSO firePatchSettings;
        [SerializeField] private float patchSurfaceOffset = 0.05f;

        protected override void OnHit()
        {
            impactFeedbacks.PlayFeedbacks(transform.position);

            if (hitGround)
                SpawnFirePatch();

            ReturnAfterDelay(spellSettings.VfxDuration);
        }

        private void SpawnFirePatch()
        {
            if (firePatchSettings == null) return;

            var pool = firePatchSettings.GetPool();
            if (pool == null)
            {
                Debug.LogError($"No hay pool registrado para SpellId: {firePatchSettings.SpellId}");
                return;
            }

            Vector3 position = groundPoint + groundNormal * patchSurfaceOffset;

            var patch = pool.GetSpell();
            patch.Execute(position, Vector3.up, firePatchSettings);
        }
    }
}
