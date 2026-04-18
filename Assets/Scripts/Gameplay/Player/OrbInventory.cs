using System;
using UnityEngine;

public class OrbInventory : MonoBehaviour
{
    [Header("Orb Prefabs")]


    public const int SlotCount = 2;
    private OrbSettingsSO[] slots = new OrbSettingsSO[SlotCount];
    private int currentSlotIndex = 0;

    public event Action<int, OrbSettingsSO> OnSlotChanged;

    public OrbSettingsSO GetSlot(int index) => slots[index];

    public bool TryAddOrb(OrbSettingsSO orbData)
    {
        if (currentSlotIndex >= SlotCount) return false;

        slots[currentSlotIndex] = orbData;
        OnSlotChanged?.Invoke(currentSlotIndex, orbData);
        currentSlotIndex++;
        return true;
    }

    public bool TryConsumeOrbs(out OrbSettingsSO first, out OrbSettingsSO second)
    {
        first = slots[0];
        second = slots[1];

        if (first == null || second == null) return false;

        ClearSlots();
        return true;
    }

    private void ClearSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            slots[i] = null;
            OnSlotChanged?.Invoke(i, null);
        }

        currentSlotIndex = 0;
    }
}