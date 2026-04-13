using Assets.Scripts.Gameplay.GameSystem;
using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using Assets.Scripts.Gameplay.Orbs.Spells;
using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

public class FireballSpell : SpellBase
{
    [Header("Config")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 25f;

    [Header("VFX")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem flightParticles;

    [Header("References")]
    [SerializeField] private MeshRenderer projectileMesh;

    private Vector3 origin;
    private Vector3 initialVelocity;
    private float elapsedTime;
    private bool hasHit;

    private void Update()
    {
        if (hasHit) return;

        elapsedTime += Time.deltaTime;
        transform.SetPositionAndRotation(CalculatePosition(elapsedTime), CalculateRotation(elapsedTime));

        if (elapsedTime >= lifetime)
            Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        if (collision.collider.TryGetComponent<HealthSystem>(out var healthSystem))
            healthSystem.DoDamage(damage);

        Explode();
    }

    public override void OnGetFromPool()
    {
        hasHit = false;
        elapsedTime = 0f;

        if (projectileMesh != null) projectileMesh.enabled = true;
        if (flightParticles != null) flightParticles.Play();

        foreach (var col in GetComponents<Collider>())
            col.enabled = true;
    }

    public override void Execute(Vector3 origin, Vector3 direction, float damage)
    {
        this.damage = damage;
        this.origin = origin;
        initialVelocity = direction * speed;
        transform.position = origin;
    }

    private void Explode()
    {
        hasHit = true;

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (projectileMesh != null) projectileMesh.enabled = false;
        if (flightParticles != null) flightParticles.Stop();

        foreach (var col in GetComponents<Collider>())
            col.enabled = false;

        ReturnToPool();
    }

    private Vector3 CalculatePosition(float t)
    {
        return origin + initialVelocity * t;
    }

    private Quaternion CalculateRotation(float t)
    {
        return initialVelocity != Vector3.zero
            ? Quaternion.LookRotation(initialVelocity)
            : transform.rotation;
    }
}