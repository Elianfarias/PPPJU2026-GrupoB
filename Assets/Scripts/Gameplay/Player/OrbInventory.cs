using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class OrbInventory : MonoBehaviour
    {
        public const int SlotCount = 2;
        private OrbSettingsSO[] slots = new OrbSettingsSO[SlotCount];

        public event Action<int, OrbSettingsSO> OnSlotChanged;

        public OrbSettingsSO GetSlot(int index) => slots[index];

        public bool TryAddOrb(OrbSettingsSO orbData)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (slots[i] != null) continue;

                slots[i] = orbData;
                OnSlotChanged?.Invoke(i, orbData);
                return true;
            }
            return false;
        }

        public bool TryConsumeSlot(int index, out OrbSettingsSO orb)
        {
            orb = slots[index];
            if (orb == null) return false;

            slots[index] = null;
            OnSlotChanged?.Invoke(index, null);
            return true;
        }
    }
}