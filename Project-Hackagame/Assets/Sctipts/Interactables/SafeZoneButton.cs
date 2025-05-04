using UnityEngine;

public class SafeZoneButton : MonoBehaviour, Iinteractable
{
    [SerializeField] private Animator safeZoneAnim; 
    public bool isSafeZoneActive = false;
    public void Interact()
    {
        if(!isSafeZoneActive)
        {
            safeZoneAnim.SetTrigger("ActivateSafeZone");
            isSafeZoneActive = true;
        }
        else
        {
            safeZoneAnim.SetTrigger("DeactivateSafeZone");
            isSafeZoneActive = false;
        }
    }
}
