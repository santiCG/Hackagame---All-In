using UnityEngine;

public class OxygenTank : MonoBehaviour, Iinteractable
{
    [SerializeField] private float refillAmount = 50f; // Amount of oxygen to refill
    [SerializeField] private int maxUses = 3; // Maximum number of uses
    private int currentUses;

    [SerializeField] private OxygenSystem oxygenSystem;
    
    private void Awake()
    {
        if (oxygenSystem == null)
        {
            oxygenSystem = FindFirstObjectByType<OxygenSystem>();
        }
    }
    
    private void Start()
    {
        currentUses = maxUses;
    }

    public void Interact()
    {
        if (currentUses > 0)
        {
            if (oxygenSystem != null)
            {
                oxygenSystem.RefillOxygen(refillAmount);
                currentUses--;

                Debug.Log($"Oxygen refilled. Remaining uses: {currentUses}");
            }
        }
        else
        {
            Debug.Log("Oxygen tank is empty!");
        }
    }
}