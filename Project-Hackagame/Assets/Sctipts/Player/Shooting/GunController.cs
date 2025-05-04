// GunController.cs
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float aimRotationSpeed = 30f;
    public float normalRotationSpeed = 100f;

    private bool isAiming = false;
    private Camera mainCamera;
    private PlayerRotation playerRotation;

    void Start()
    {
        mainCamera = Camera.main;
        playerRotation = FindObjectOfType<PlayerRotation>();
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    void HandleAiming()
    {
        if (Input.GetMouseButton(1)) // click derecho
        {
            isAiming = true;
            playerRotation.rotationSpeed = aimRotationSpeed;
        }
        else
        {
            isAiming = false;
            playerRotation.rotationSpeed = normalRotationSpeed;
        }
    }

    void HandleShooting()
    {
        if (isAiming && Input.GetMouseButtonDown(0)) // click izquierdo mientras apunta
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); // centro de la pantalla
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = (hit.point - firePoint.position).normalized;
                Shoot(direction);
            }
            else
            {
                Vector3 direction = ray.direction;
                Shoot(direction);
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponentInChildren<Rigidbody>();
        rb.linearVelocity = direction * projectileSpeed;
    }
}
