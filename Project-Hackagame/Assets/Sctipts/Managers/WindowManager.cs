using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class WindowManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI descriptionText; // Text for the description of the objective
    [SerializeField] private TextMeshProUGUI objectivesText; // Text for the number of objectives (e.g., "0/4")
    [SerializeField] private RectTransform objectiveUI; // The UI element to push to the side when completed

    [Header("Objectives")]
    [SerializeField] private List<windowCleaningScript> windowsToClean; // List of windows to clean
    private int totalWindows;
    private int cleanedWindows;

    private void Start()
    {
        // Initialize the total windows and update the UI
        totalWindows = windowsToClean.Count;
        cleanedWindows = 0;
        UpdateObjectiveUI();

        // Subscribe to each window's completion event
        foreach (windowCleaningScript window in windowsToClean)
        {
            window.OnWindowCleaned += HandleWindowCleaned;
        }
    }

    private void HandleWindowCleaned()
    {
        // Increment the cleaned windows count
        cleanedWindows++;
        UpdateObjectiveUI();

        // Check if all windows are cleaned
        if (cleanedWindows >= totalWindows)
        {
            CompleteAllObjectives();
        }
    }

    private void UpdateObjectiveUI()
    {
        // Update the objectives text (e.g., "2/4")
        objectivesText.text = $"({cleanedWindows}/{totalWindows})";
    }

    private void CompleteAllObjectives()
    {
        // Change the color of the text to green
        objectivesText.color = Color.green;
        descriptionText.color = Color.green;

        // Push the objective UI to the side
        if (objectiveUI != null)
        {
        objectiveUI.DOAnchorPos(objectiveUI.anchoredPosition + new Vector2(200f, 0f), 1f)
            .SetEase(Ease.InOutQuad);        
        }

        Debug.Log("All windows cleaned! Objectives completed.");
    }
}