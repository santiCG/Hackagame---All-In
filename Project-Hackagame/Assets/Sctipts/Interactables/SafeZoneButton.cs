using UnityEngine;

public class SafeZoneButton : MonoBehaviour, Iinteractable
{
    [SerializeField] private SafeZone safeZone; 
    [SerializeField] private Animator safeZoneAnim;
    public bool isSafeZoneActive = false;

    [HideInInspector] public Outline outline;

    private void Awake()
    {
        safeZone = FindFirstObjectByType<SafeZone>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact()
    {
        if (!isSafeZoneActive)
        {
            safeZoneAnim.SetTrigger("ActivateSafeZone");
            isSafeZoneActive = true;
            safeZone.ToggleSafeZone();
        }
        else
        {
            safeZoneAnim.SetTrigger("DeactivateSafeZone");
            isSafeZoneActive = false;
            safeZone.ToggleSafeZone();
        }
    }
}