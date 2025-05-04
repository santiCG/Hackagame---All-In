using UnityEngine;

public class DialogueTriggerBox : MonoBehaviour
{
    [SerializeField] private DialogueTriggerManager triggerManager;
    [SerializeField] private int dialogueIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerManager.TriggerDialogueFromCollision(dialogueIndex);
        }
    }
}