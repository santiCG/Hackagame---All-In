using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private SafeZoneButton safeZoneButton;
    [SerializeField] private OxygenSystem oxygenSystem;
    [SerializeField] private Collider safeZoneCollider;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (safeZoneButton == null)
        {
            safeZoneButton = FindFirstObjectByType<SafeZoneButton>();
        }

        if (oxygenSystem == null)
        {
            oxygenSystem = FindFirstObjectByType<OxygenSystem>();
        }

        safeZoneCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        // Initialize the safe zone state at the start
        if (safeZoneButton.isSafeZoneActive)
        {
            ActivateSafeZone();
        }
        else
        {
            DeactivateSafeZone();
        }
    }

    public void ToggleSafeZone()
    {
        if (safeZoneButton.isSafeZoneActive)
        {
            ActivateSafeZone();
        }
        else if (safeZoneButton.isSafeZoneActive == false)
        {
            DeactivateSafeZone();
        }
    }

    private void ActivateSafeZone()
    {
        oxygenSystem.EnterSafeZone();
        DisablePlayerGravity();
        Debug.Log("Safe zone activated. Gravity disabled.");
    }

    private void DeactivateSafeZone()
    {
        oxygenSystem.ExitSafeZone();
        EnablePlayerGravity();
        Debug.Log("Safe zone deactivated. Gravity enabled.");
    }

    private void DisablePlayerGravity()
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.useGravity = true; // Using Gravity
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void EnablePlayerGravity()
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.useGravity = false; // NOT using gravity
            playerRigidbody.constraints = RigidbodyConstraints.None; // Unfreeze all rotations and position
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;

        }
    }
}