using UnityEngine;

public class SafeZoneButton : MonoBehaviour, Iinteractable
{
    [SerializeField] private Animator safeZoneAnim;
    public bool isSafeZoneActive = false;

    public Outline outline;
    [SerializeField] private SafeZone safeZone; // Reference to the SafeZone script

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact()
    {
        if (!isSafeZoneActive)
        {
            safeZoneAnim.SetTrigger("ActivateSafeZone");
            isSafeZoneActive = true;
        }
        else
        {
            safeZoneAnim.SetTrigger("DeactivateSafeZone");
            isSafeZoneActive = false;
        }

        // Notify the SafeZone to toggle its state
        if (safeZone != null)
        {
            safeZone.ToggleSafeZone();
        }
    }
}