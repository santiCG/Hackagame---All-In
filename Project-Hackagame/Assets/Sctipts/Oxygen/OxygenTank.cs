using UnityEngine;
using System.Collections.Generic;

public class OxygenTank : MonoBehaviour, Iinteractable
{
    [SerializeField] private float refillAmount = 50f; // Amount of oxygen to refill
    [SerializeField] private int maxUses = 3; // Maximum number of uses
    private int currentUses;

    [SerializeField] private OxygenSystem oxygenSystem;

    [Header("Meshes to Disable")]
    [SerializeField] private List<GameObject> meshesToDisable;

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

                DisableNextMesh();
            }
        }
        else
        {
            GetComponent<MeshCollider>().enabled = false;
            Debug.Log("Oxygen tank is empty!");
        }
    }

    private void DisableNextMesh()
    {
        if (meshesToDisable.Count > 0)
        {
            // Disable the first mesh in the list
            GameObject meshToDisable = meshesToDisable[0];
            if (meshToDisable != null)
            {
                meshToDisable.SetActive(false);
                Debug.Log($"Disabled mesh: {meshToDisable.name}");
            }

            // Remove the mesh from the list
            meshesToDisable.RemoveAt(0);
        }
        else
        {
            Debug.Log("No more meshes to disable!");
        }
    }
}