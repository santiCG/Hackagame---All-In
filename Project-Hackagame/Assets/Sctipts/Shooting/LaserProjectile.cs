// LaserProjectile.cs
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public float damage = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // por si no colisiona con nada
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("¡Impacto en enemigo!");

            SpaceBarnacle barnacle = collision.gameObject.GetComponent<SpaceBarnacle>();
            if (barnacle != null)
            {
                barnacle.TakeDamage(damage);
                Destroy(gameObject, 0.2f);
            }
        }
        else
        {
            Destroy(gameObject, 0.2f);
        }

    }
}
