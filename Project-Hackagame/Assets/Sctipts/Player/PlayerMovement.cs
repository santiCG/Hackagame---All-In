using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Jetpack Settings")]
    public float thrustForce = 5f;
    public float maxGas = 100f;
    public float gasConsumptionRate = 10f;
    public float refillDelay = 3f;
    public float refillRate = 20f;

    [Space]
    [Header("Movement Settings")]
    public bool isWalking;
    public bool lockMovement;
    public float stopMagnitude = 0.1f;

    [Header("Sound")]
    public AudioSource JetpackAudio;

    private float currentGas;
    private float lastInputTime;
    private Rigidbody rb;

    private Vector2 planarInput; // x = lateral, y = forward/backward
    private float verticalInput = 0f;

    private bool isUsingJetpack = false;

    private PlayerRotation playerRotation;

    public float baseMoveSpeed = 5f;
    public float rotationSlowFactor = 0.4f; // Cuánto se reduce al rotar
    public float smoothTime = 0.3f; // Suavidad del cambio de velocidad

    private float currentMoveSpeed;
    private float speedVelocity = 0f; // Ref. para SmoothDamp

    // Esto será actualizado desde el PlayerLook
    [HideInInspector]
    public float currentRotationInput = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerRotation = GetComponentInParent<PlayerRotation>();
        currentGas = maxGas;
        currentMoveSpeed = baseMoveSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        planarInput = context.ReadValue<Vector2>();
        CheckJetpackUsage();
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        if (context.performed) verticalInput = 1f;
        else if (context.canceled) verticalInput = 0f;
        CheckJetpackUsage();
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        if (context.performed) verticalInput = -1f;
        else if (context.canceled) verticalInput = 0f;
        CheckJetpackUsage();
    }

    public bool IsMovementLocked { get; private set; } = false;

    public void SetMovementLock(bool locked)
    {
        IsMovementLocked = locked;
    }

    private void CheckJetpackUsage()
    {
        isUsingJetpack = planarInput != Vector2.zero || verticalInput != 0f;
        if (isUsingJetpack)
        {
            lastInputTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (IsMovementLocked) return;

        if (isUsingJetpack && currentGas > 0f)
        {
            if(!JetpackAudio.isPlaying)
            {
                JetpackAudio.Play();
            }
            Vector3 moveDir = new Vector3(planarInput.x, verticalInput, planarInput.y);
            rb.AddRelativeForce(moveDir * thrustForce, ForceMode.Acceleration);
            currentGas -= gasConsumptionRate * Time.fixedDeltaTime;
            currentGas = Mathf.Clamp(currentGas, 0, maxGas);
        }
        else if (!isUsingJetpack && Time.time - lastInputTime >= refillDelay)
        {
            currentGas += refillRate * Time.fixedDeltaTime;
            currentGas = Mathf.Clamp(currentGas, 0, maxGas);
        }

        if (rb.linearVelocity.magnitude < stopMagnitude && rb.linearVelocity != Vector3.zero)
        { 
            rb.linearVelocity = Vector3.zero;
        }

        if(playerRotation.isRotating)
        {
            // Determinar la velocidad objetivo
            float targetSpeed = baseMoveSpeed * (Mathf.Abs(currentRotationInput) > 0.05f ? rotationSlowFactor : 1f);

            // Suavizar transición con SmoothDamp
            currentMoveSpeed = Mathf.SmoothDamp(currentMoveSpeed, targetSpeed, ref speedVelocity, smoothTime);

            // Calcular dirección de movimiento
            Vector3 moveDirection = transform.right * planarInput.x + transform.forward * planarInput.y + transform.up * verticalInput;
            moveDirection.Normalize();

            // Aplicar movimiento
            rb.linearVelocity = moveDirection * currentMoveSpeed + new Vector3(0, rb.linearVelocity.y, 0); // mantener eje Y si usas gravedad
        }
    }

    public float GetCurrentGasPercentage()
    {
        return currentGas / maxGas;
    }
}
