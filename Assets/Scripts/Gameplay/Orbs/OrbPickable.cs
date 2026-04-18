
using Assets.Scripts.Gameplay.Orbs;
using Assets.Scripts.Gameplay.Player;
using Assets.Scripts.Gameplay.Systems.Interfaces;
using MoreMountains.Feedbacks;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OrbPickable : MonoBehaviour, IPickable, IPoolable
{
    [SerializeField] private MMF_Player onGroundFeedbacks;
    [SerializeField] private MMWiggle wiggleFeedbacks;
    [SerializeField] private MMF_Player orbitingFeedbacks;

    private OrbSettingsSO orbSettings;
    private OrbPool ownerPool;

    public event Action OnReturned;
    public OrbSettingsSO OrbSettings => orbSettings;

    public void Setup(OrbSettingsSO settings)
    {
        orbSettings = settings;
    }

    public void SetPool(OrbPool pool)
    {
        ownerPool = pool;
    }

    public void OnGetFromPool()
    {
        GetComponent<Collider>().enabled = true;
        wiggleFeedbacks.enabled = true;
        onGroundFeedbacks.PlayFeedbacks();
    }

    public void OnPickup(GameObject picker)
    {
        if (!picker.TryGetComponent<OrbInventory>(out var inventory)) return;
        if (!inventory.TryAddOrb(orbSettings)) return;

        if (!picker.TryGetComponent<OrbOrbitable>(out var orbitController)) return;

        EnterOrbitMode(orbitController);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnPickup(other.gameObject);
    }

    private void EnterOrbitMode(OrbOrbitable orbitController)
    {
        onGroundFeedbacks.StopFeedbacks();
        wiggleFeedbacks.enabled = false;
        orbitingFeedbacks.PlayFeedbacks();
        GetComponent<Collider>().enabled = false;
        GetComponent<AudioSource>().volume = 0.2f;
        orbitController.RegisterOrb(this);
    }

    public void ReturnToPool()
    {
        orbitingFeedbacks.StopFeedbacks();
        OnReturned?.Invoke();
        OnReturned = null;
        ownerPool.ReturnOrb(this);
    }
}