using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("State");
    public event Action<float, float> OnLifeUpdated;
    public event Action<float, float> OnHealing;
    public event Action OnDie;

    [SerializeField] private float maxLife = 100f;
    [SerializeField] private Rigidbody rb;

    private float life = 100f;
    private bool isTakingDamage;

    private void Start()
    {
        life = maxLife;
        OnLifeUpdated?.Invoke(life, maxLife);
    }

    public void DoDamage(float damage)
    {
        if (damage < 0 || isTakingDamage)
            return;

        life -= damage;

        if (life <= 0)
            StartCoroutine(nameof(Die));
        else
        {
            StartCoroutine(nameof(TakeDamage));

            OnLifeUpdated?.Invoke(life, maxLife);
        }
    }

    public void Heal(float plus)
    {
        if (plus < 0)
            return;

        life += plus;

        if (life > maxLife)
            life = maxLife;

        OnHealing?.Invoke(life, maxLife);
    }

    private IEnumerator TakeDamage()
    {
        isTakingDamage = true;

        yield return new WaitForSeconds(0.3f);

        isTakingDamage = false;
    }

    private IEnumerator Die()
    {
        life = 0;
        OnDie?.Invoke();
        Destroy(gameObject);
        yield return new WaitForSeconds(0.5f);
    }

    public float GetCurrentLife()
    {
        return life;
    }
}