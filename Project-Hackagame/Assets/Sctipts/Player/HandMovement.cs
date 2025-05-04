using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    [Header("Movimiento de la mano")]
    public float moveSpeed = 0.005f;
    public float interactionYOffset = 0.1f; // Antes era en Z, ahora en Y
    public float interactionDistance = 1f;
    public float smoothTime = 0.1f;

    [Header("Límites de movimiento (locales)")]
    public Vector2 xLimits = new Vector2(-0.5f, 0.5f);
    public Vector2 yLimits = new Vector2(-0.5f, 0.5f);

    [Header("Referencias")]
    public Camera playerCamera;
    public Image crossImage;

    private Vector3 originalLocalPosition;
    private Vector3 targetLocalPosition;
    private Vector3 currentVelocity;

    private GameObject interactableObj;

    private bool isControllingHand = false;
    private bool isInteracting = false;

    public bool PlayerInteracting => isInteracting;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        targetLocalPosition = originalLocalPosition;
    }

    private void Update()
    {
        HandleRaycast();
        MoveHandWithMouse();
        SmoothFollow();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        isControllingHand = context.performed || context.started;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed && isInteracting)
        {
            // "Pegar" la mano al objeto
            targetLocalPosition = interactableObj.transform.position;
            
        }
        if(context.canceled)
        {
            targetLocalPosition = originalLocalPosition;
        }
    }

    private void HandleRaycast()
    {
        isInteracting = false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                crossImage.color = new Color(0f, 1f, 0f);

                isInteracting = true;

                interactableObj = hit.collider.gameObject;
               
                // ejecutar animacion de abrir la mano o algo
                return;
            }
        }

        // Si no hay interacción
        crossImage.color = new Color(1f, 1f, 1f);
    }

    private void MoveHandWithMouse()
    {
        if (!isControllingHand) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        targetLocalPosition += new Vector3(
            mouseDelta.x * moveSpeed,
            mouseDelta.y * moveSpeed,
            0f
        );

        // Limitar la posición de la mano en X e Y (localmente)
        targetLocalPosition.x = Mathf.Clamp(targetLocalPosition.x, xLimits.x, xLimits.y);
        targetLocalPosition.y = Mathf.Clamp(targetLocalPosition.y, yLimits.x, yLimits.y);
    }

    private void SmoothFollow()
    {
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetLocalPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}
