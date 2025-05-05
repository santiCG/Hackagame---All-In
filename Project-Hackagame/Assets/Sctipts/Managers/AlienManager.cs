using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class AlienManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameManager gameManager; // Reference to the GameManager script
    [SerializeField] private TextMeshProUGUI descriptionText; // Text for the description of the objective
    [SerializeField] private TextMeshProUGUI objectivesText; // Text for the number of objectives (e.g., "0/5")
    [SerializeField] private RectTransform objectiveUI; // The UI element to push to the side when completed

    [Header("Objectives")]
    [SerializeField] private List<SpaceBarnacle> barnaclesToEliminate; // List of barnacles to eliminate
    private int totalBarnacles;
    private int eliminatedBarnacles;
    public AudioSource primerPercebe;
    public AudioSource todosPercebes;

    private Dialogue dialogueScript;

    private void Awake()
    {
        // Ensure the GameManager is assigned
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        // Ensure the Dialogue script is assigned
        dialogueScript = FindFirstObjectByType<Dialogue>();
    }

    private void Start()
    {
        // Initialize the total barnacles and update the UI
        totalBarnacles = barnaclesToEliminate.Count;
        eliminatedBarnacles = 0;
        UpdateObjectiveUI();

        // Subscribe to each barnacle's death event
        foreach (SpaceBarnacle barnacle in barnaclesToEliminate)
        {
            barnacle.OnBarnacleEliminated += HandleBarnacleEliminated;
        }
    }

    private void HandleBarnacleEliminated()
    {
        // Increment the eliminated barnacles count
        eliminatedBarnacles++;

        if(eliminatedBarnacles == 1){
            dialogueScript.TriggerDialogue(2);
            primerPercebe.Play();
        }

        UpdateObjectiveUI();

        // Check if all barnacles are eliminated
        if (eliminatedBarnacles >= totalBarnacles)
        {
            dialogueScript.TriggerDialogue(4);
            todosPercebes.Play();
            CompleteAllObjectives();
        }
    }

    private void UpdateObjectiveUI()
    {
        // Update the objectives text (e.g., "2/5")
        objectivesText.text = $"({eliminatedBarnacles}/{totalBarnacles})";
    }

    private void CompleteAllObjectives()
    {
        // Change the color of the text to green
        objectivesText.color = Color.green;
        descriptionText.color = Color.green;

        // Push the objective UI to the side with a smooth animation
        if (objectiveUI != null)
        {
            objectiveUI.DOAnchorPos(objectiveUI.anchoredPosition + new Vector2(200f, 0f), 1f)
                .SetEase(Ease.InOutQuad); // Smooth easing for the animation
        }

        gameManager.CompleteObjective();
        Debug.Log("All barnacles eliminated! Objectives completed.");
    }
}