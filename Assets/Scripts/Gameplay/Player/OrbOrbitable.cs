using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class OrbOrbitable : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float orbitRadius = 1.5f;
        [SerializeField] private float orbitSpeed = 90f;
        [SerializeField] private float orbitHeight = 0.5f;

        private OrbPickable[] orbitingOrbs = new OrbPickable[OrbInventory.SlotCount];
        private OrbInventory orbInventory;
        private float currentAngle;

        private void Awake()
        {
            orbInventory = GetComponent<OrbInventory>();
            orbInventory.OnSlotChanged += OnSlotChanged;
        }

        private void OnDestroy()
        {
            orbInventory.OnSlotChanged -= OnSlotChanged;
        }

        private void Update()
        {
            currentAngle += orbitSpeed * Time.deltaTime;

            for (int i = 0; i < orbitingOrbs.Length; i++)
            {
                if (orbitingOrbs[i] == null) continue;

                float angleOffset = (360f / OrbInventory.SlotCount) * i;
                float radians = (currentAngle + angleOffset) * Mathf.Deg2Rad;

                var orbitPosition = transform.position + new Vector3(
                    Mathf.Cos(radians) * orbitRadius,
                    orbitHeight,
                    Mathf.Sin(radians) * orbitRadius
                );

                orbitingOrbs[i].transform.position = orbitPosition;
            }
        }

        public void RegisterOrb(OrbPickable orb)
        {
            for (int i = 0; i < orbitingOrbs.Length; i++)
            {
                if (orbitingOrbs[i] != null) continue;

                orbitingOrbs[i] = orb;
                return;
            }
        }

        private void OnSlotChanged(int slotIndex, OrbSettingsSO orbData)
        {
            if (orbData != null) return;

            var orb = orbitingOrbs[slotIndex];
            if (orb == null) return;

            orb.ReturnToPool();
            orbitingOrbs[slotIndex] = null;
        }
    }
}