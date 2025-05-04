using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandMovement : MonoBehaviour
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
    private RaycastHit lastHit;

    private GameObject interactableObj;
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;
    private Transform playerTransform;
    private Transform positioningPoint;

    private bool isPositioningPlayer = false;
    private bool playerCanMove = true;
    private Vector3 targetPlayerPosition;
    private Quaternion targetPlayerRotation;
    private float positioningSpeed = 2f;

    private bool IsControllingHand { get; set; } = false;
    private bool isInteracting = false;

    public bool PlayerInteracting => isInteracting;

    public bool IsPlayerInteracting()
    {
        return isInteracting && !playerCanMove;
    }

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        targetLocalPosition = originalLocalPosition;

        playerTransform = transform.root;
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRotation = playerTransform.GetComponentInParent<PlayerRotation>();
    }

    private void Update()
    {
        HandleRaycast();
        MoveHandWithMouse();
        SmoothFollow();

        if (isPositioningPlayer)
        {
            float step = positioningSpeed * Time.deltaTime;

            // Interpolación hacia la posición y rotación deseadas
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPlayerPosition, step);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetPlayerRotation, step * 100f);

            // Verifica si ya llegó
            if (Vector3.Distance(playerTransform.position, targetPlayerPosition) < 0.01f &&
                Quaternion.Angle(playerTransform.rotation, targetPlayerRotation) < 1f)
            {
                isPositioningPlayer = false; // Detiene la transición
                playerMovement.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        //isControllingHand = context.performed || context.started;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed && isInteracting)
        {
            if (positioningPoint != null)
            {
                // Bloquea movimiento y rotación antes de iniciar transición
                playerMovement.SetMovementLock(true);
                playerRotation.SetRotationLock(true);

                // Configura destino
                targetPlayerPosition = positioningPoint.position;
                targetPlayerRotation = positioningPoint.rotation;

                // Activa la transición
                isPositioningPlayer = true;
                playerCanMove = false;
                IsControllingHand = true;
            }

            // "Pegar" la mano al objeto
            targetLocalPosition = interactableObj.transform.position;
            //playerMovement.MoveToInteractionPoint();
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
                lastHit = hit;
                
                // Busca el PositioningPoint dentro del objeto interactuable
                positioningPoint = lastHit.collider.gameObject.transform.Find("PositioningPoint");

                interactableObj = hit.collider.gameObject;
               
                // ejecutar animacion de abrir la mano o algo
                return;
            }
        }

        // Si no hay interacción
        crossImage.color = new Color(1f, 1f, 1f);
        IsControllingHand = false;
    }

    private void MoveHandWithMouse()
    {
        if (!IsControllingHand) return;

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
