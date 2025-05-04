using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRange = 5f;

    private Outline currentOutline;

    private void Update()
    {
        HandleOutline();
    }

    private void HandleOutline()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
            Outline outline = hit.collider.GetComponent<Outline>();

            if (interactable != null && outline != null)
            {
                // Enable the outline for the object the player is looking at
                if (currentOutline != outline)
                {
                    DisableCurrentOutline();
                    currentOutline = outline;
                    currentOutline.enabled = true;
                }
            }
            else
            {
                // Disable the outline if the object is not interactable
                DisableCurrentOutline();
            }
        }
        else
        {
            // Disable the outline if no object is hit
            DisableCurrentOutline();
        }
    }

    private void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            //Debug.Log("Interact button pressed");

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
                if (interactable != null)
                {
                    //Debug.Log("Interacting with");
                    interactable.Interact();
                }
            }
        }
    }
}