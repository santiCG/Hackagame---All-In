using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private SafeZoneButton safeZoneButton;

    private void Awake()
    {
        if (safeZoneButton == null)
        {
            safeZoneButton = FindFirstObjectByType<SafeZoneButton>();
        }
    }

    private void Start()
    {
        OverLapBox();
    }

    private void OverLapBox()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        foreach (Collider collider in colliders)
        {
            OxygenSystem oxygenSystem = collider.GetComponent<OxygenSystem>();
            if (oxygenSystem != null)
            {
                if(safeZoneButton.isSafeZoneActive)
                {
                    oxygenSystem.EnterSafeZone();
                }
                else
                {
                    oxygenSystem.ExitSafeZone();
                }
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has an OxygenSystem component
        OxygenSystem oxygenSystem = other.GetComponent<OxygenSystem>();
        if (oxygenSystem != null)
        {
            oxygenSystem.EnterSafeZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player has an OxygenSystem component
        OxygenSystem oxygenSystem = other.GetComponent<OxygenSystem>();
        if (oxygenSystem != null)
        {
            oxygenSystem.ExitSafeZone();
        }
    }
    */
}
