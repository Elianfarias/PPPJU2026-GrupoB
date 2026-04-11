using Assets.Scripts.Gameplay.GameSystem;
using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [Header("Config")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private PoolType poolType;

    [Header("VFX")]
    [SerializeField] private GameObject rocketExplosion;
    [SerializeField] private ParticleSystem disableOnHit;

    [Header("References")]
    [SerializeField] private MeshRenderer projectileMesh;
    [SerializeField] private AudioSource inFlightAudioSource;

    private float damage;
    private float elapsedTime;
    private bool targetHit;
    private Vector3 origin;
    private Vector3 initialVelocity;
    private bool useGravity = true;
    public void OnGetFromPool()
    {
        targetHit = false;
        elapsedTime = 0f;

        if (projectileMesh != null) projectileMesh.enabled = true;
        if (inFlightAudioSource != null) inFlightAudioSource.Play();
        if (disableOnHit != null) disableOnHit.Play();

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = true;
    }

    public void OnReturnToPool()
    {
    }

    private void Update()
    {
        if (targetHit) return;

        elapsedTime += Time.deltaTime;

        transform.SetPositionAndRotation(CalculatePosition(elapsedTime), CalculateRotation(elapsedTime));

        if (elapsedTime >= lifetime)
            ReturnToPool();
    }

    private Vector3 CalculatePosition(float t)
    {
        if (useGravity)
            return origin + initialVelocity * t + (t * t) * 0.5f * Physics.gravity;

        return origin + initialVelocity * t;
    }

    private Quaternion CalculateRotation(float t)
    {
        Vector3 velocity = initialVelocity + Physics.gravity * t;
        return velocity != Vector3.zero
            ? Quaternion.LookRotation(velocity)
            : transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit) return;

        if (collision.collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            healthSystem.DoDamage(damage);

        targetHit = true;
        Explode();
        ReturnToPool();
    }

    private void Explode()
    {
        Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation);

        if (projectileMesh != null)
            projectileMesh.enabled = false;

        inFlightAudioSource.Stop();
        disableOnHit.Stop();

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;
    }

    private void ReturnToPool()
    {
        switch (poolType)
        {
            case PoolType.PlayerBullet:
                PlayerBulletPool.Instance.Return(this);
                break;
            default:
                break;
        }
    }

    public void Launch(Vector3 direction, float damage, bool useGravity = true)
    {
        this.useGravity = useGravity;
        this.damage = damage;
        origin = transform.position;
        initialVelocity = direction * speed;
    }
}