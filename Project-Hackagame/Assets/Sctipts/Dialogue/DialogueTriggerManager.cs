using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueTrigger
{
    public int dialogueIndex; // The index of the dialogue to trigger
    public GameObject triggerBox; // The trigger box associated with this dialogue
}

public class DialogueTriggerManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueManager; // Reference to the Dialogue script
    [SerializeField] private List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>();

    private int currentTriggerIndex = 0; // Tracks the current active trigger

    private void Start()
    {
        // Deactivate all trigger boxes initially
        foreach (DialogueTrigger trigger in dialogueTriggers)
        {
            if (trigger.triggerBox != null)
            {
                trigger.triggerBox.SetActive(false);
            }
        }

        // Activate the first trigger box
        if (dialogueTriggers.Count > 0 && dialogueTriggers[0].triggerBox != null)
        {
            dialogueTriggers[0].triggerBox.SetActive(true);
        }
    }

    public void TriggerDialogueFromObjective(int dialogueIndex)
    {
        // Trigger a dialogue programmatically (e.g., when an objective is completed)
        dialogueManager.TriggerDialogue(dialogueIndex);

        // Handle trigger box activation/deactivation
        HandleTriggerBoxes(dialogueIndex);
    }

    public void TriggerDialogueFromCollision(int dialogueIndex)
    {
        // Trigger a dialogue from a collision
        dialogueManager.TriggerDialogue(dialogueIndex);

        // Handle trigger box activation/deactivation
        HandleTriggerBoxes(dialogueIndex);
    }

    private void HandleTriggerBoxes(int dialogueIndex)
    {
        // Deactivate the current trigger box
        if (currentTriggerIndex < dialogueTriggers.Count && dialogueTriggers[currentTriggerIndex].triggerBox != null)
        {
            dialogueTriggers[currentTriggerIndex].triggerBox.SetActive(false);
        }

        // Activate the next trigger box
        currentTriggerIndex++;
        if (currentTriggerIndex < dialogueTriggers.Count && dialogueTriggers[currentTriggerIndex].triggerBox != null)
        {
            dialogueTriggers[currentTriggerIndex].triggerBox.SetActive(true);
        }
    }
}