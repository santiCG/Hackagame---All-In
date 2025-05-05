using UnityEngine;

public class OxygenTank : MonoBehaviour, Iinteractable
{
    [SerializeField] private float refillAmount = 50f; // Amount of oxygen to refill
    [SerializeField] private int maxUses = 3; // Maximum number of uses
    private int currentUses;

    [SerializeField] private OxygenSystem oxygenSystem;

    public Outline outline;
    private Dialogue dialogueScript;
    
    private void Awake()
    {
        if (oxygenSystem == null)
        {
            oxygenSystem = FindFirstObjectByType<OxygenSystem>();
        }

        if (dialogueScript == null)
        {
            dialogueScript = FindFirstObjectByType<Dialogue>();
        }

        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    
    private void Start()
    {
        currentUses = maxUses;
    }

    public void Interact()
    {
        if (currentUses > 0)
        {

            if(currentUses == 1){
                dialogueScript.TriggerDialogue(5);
            }

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