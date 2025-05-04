using UnityEngine;

public class OutlineInteractuables : MonoBehaviour
{
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 5f;
    private GameObject[] interactuables;

    private void Awake()
    {
        interactuables = GameObject.FindGameObjectsWithTag("Interactable");

        foreach (GameObject interactable in interactuables)
        {
            if (interactable.GetComponent<Outline>() == null)
            {
                interactable.AddComponent<Outline>();
                interactable.GetComponent<Outline>().enabled = false;
                interactable.GetComponent<Outline>().OutlineColor = outlineColor;
                interactable.GetComponent<Outline>().OutlineWidth = outlineWidth;
            }
        }
    }
}
