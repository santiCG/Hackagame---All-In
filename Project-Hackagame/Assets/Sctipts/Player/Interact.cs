using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRange = 5f;

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
