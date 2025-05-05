using Unity.VisualScripting;
using UnityEngine;

public class DialogueTriggerBox : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueManager;
    [SerializeField] private int dialogueIndex;
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private ScenesManager scenesManager;
    [SerializeField] private float winFadeDuration = 35f; // Duration for the fade to main menu

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.TriggerDialogue(dialogueIndex);

            shipAnimator.SetTrigger("ActivateSafeZone");
            shipAnimator.SetTrigger("Escape");

            scenesManager.FadeToMainMenu(winFadeDuration); // 5-second fade-in to the main menu
        }
    }
}