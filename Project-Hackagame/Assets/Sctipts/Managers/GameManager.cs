using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueScript;
    [SerializeField] private int totalObjectives = 3;
    private int completedObjectives = 0;
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
    }

    void Update()
    {
        // Epa
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
}