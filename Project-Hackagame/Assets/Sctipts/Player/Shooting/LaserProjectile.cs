// LaserProjectile.cs
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // por si no colisiona con nada
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("�Impacto en enemigo!");
            // Aqu� puedes a�adir efectos, da�o, etc.
        }

        //Destroy(gameObject);
    }
}
