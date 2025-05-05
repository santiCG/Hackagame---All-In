using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueScript;
    [SerializeField] private int totalObjectives = 3;
    private int completedObjectives = 0;

    [Header("Death UI")]
    [SerializeField] private DamageUI_VFX damageUIVFX; // Reference to the DamageUI_VFX script
    [SerializeField] private GameObject deathPanel; // Reference to the death panel

    void Awake()
    {
        if (dialogueScript == null)
        {
            dialogueScript = FindFirstObjectByType<Dialogue>();
        }
    }

    void Start()
    {
        Debug.Log($"Objectives to complete: {totalObjectives}");
        if (deathPanel != null)
        {
            deathPanel.SetActive(false); // Ensure the death panel is hidden at the start
        }

        dialogueScript.TriggerDialogue(0);
    }

    public void CompleteObjective()
    {
        completedObjectives++;
        Debug.Log($"Objective completed! {completedObjectives}/{totalObjectives}");

        if (completedObjectives >= totalObjectives)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("All objectives completed! You win!");
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died!");

        dialogueScript.TriggerDialogue(7);

        if (damageUIVFX != null)
        {
            damageUIVFX.FadeToBlack(() =>
            {
                if (deathPanel != null)
                {
                    Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                    Cursor.visible = true; // Make the cursor visible
                    deathPanel.SetActive(true); // Show the death panel after fading to black
                
                    FadeInDeathPanel();
                }
            });
        }
    }

        private void FadeInDeathPanel()
    {
        // Get all Image components in the death panel (including children)
        Image[] images = deathPanel.GetComponentsInChildren<Image>(true);

        foreach (Image image in images)
        {
            Color initialColor = image.color;
            initialColor.a = 0f; // Start fully transparent
            image.color = initialColor;

            // Fade the alpha to 1 over 1 second
            image.DOFade(1f, 1f);
        }
    }
}