using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueGroup
{
    public AudioSource sound;
    public int index; // Unique identifier for the group
    public string[] lines; // Lines of dialogue in this group
    public float[] delays; // Delays for each line
}

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComp;
    [SerializeField] private float textSpeed;
    [SerializeField] private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();

    private int currentGroupIndex = -1; // Tracks the current group being displayed
    private int lineIndex = 0; // Tracks the current line in the group
    private bool finishDialogue = false;

    private void Start()
    {

    }

    public void TriggerDialogue(int groupIndex)
    {
        DialogueGroup group = dialogueGroups.Find(g => g.index == groupIndex);

        if (group != null)
        {
            currentGroupIndex = groupIndex;
            lineIndex = 0;
            group.sound.Play();
            finishDialogue = false;
            StartCoroutine(DialogDuration(group));
        }
        else
        {
            Debug.LogWarning($"No dialogue group found with index: {groupIndex}");
        }
    }

    IEnumerator TypeLine(string line)
    {
        textComp.text = string.Empty;

        foreach (char c in line.ToCharArray())
        {
            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine(DialogueGroup group)
    {
        lineIndex++;

        if (lineIndex < group.lines.Length)
        {
            textComp.text = string.Empty;
            StartCoroutine(DialogDuration(group));
        }
        else
        {
            StopAllCoroutines();
            finishDialogue = true;

            textComp.text = string.Empty;
        }
    }

IEnumerator DialogDuration(DialogueGroup group)
{
    yield return StartCoroutine(TypeLine(group.lines[lineIndex]));

    yield return new WaitForSeconds(group.delays[lineIndex]);
    
    NextLine(group);
}

    public bool IsDialogueFinished()
    {
        return finishDialogue;
    }
}