using UnityEngine;

public class SpaceBarnacle : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Effects")]
    public GameObject deathEffect;

    private Rigidbody rb;

    public delegate void BarnacleEliminated();
    public event BarnacleEliminated OnBarnacleEliminated;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger the death effect
        if (deathEffect)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }

        rb.constraints = RigidbodyConstraints.None;

        // Notify the AlienManager
        OnBarnacleEliminated?.Invoke();

        // Destroy the barnacle after a delay
        Destroy(gameObject, 3f);
    }
}