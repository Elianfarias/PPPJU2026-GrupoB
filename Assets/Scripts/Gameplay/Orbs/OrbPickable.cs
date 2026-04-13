
using Assets.Scripts.Gameplay.Orbs;
using Assets.Scripts.Gameplay.Systems;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OrbPickable : MonoBehaviour, IPickable, IPoolable
{
    [Header("Float Animation")]
    [SerializeField] private float floatAmplitude = 0.3f;
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 90f;

    private OrbSettingsSO orbSettings;
    private OrbPool ownerPool;
    private Vector3 startPosition;

    public event Action OnReturned;
    public OrbSettingsSO OrbSettings => orbSettings;

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPosition + Vector3.up * yOffset;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnPickup(other.gameObject);
    }

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
        startPosition = transform.position;
    }

    public void OnPickup(GameObject picker)
    {
        if (!picker.TryGetComponent<OrbInventory>(out var inventory)) return;
        if (!inventory.TryAddOrb(orbSettings)) return;

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        OnReturned?.Invoke();
        OnReturned = null;
        ownerPool.ReturnOrb(this);
    }
}