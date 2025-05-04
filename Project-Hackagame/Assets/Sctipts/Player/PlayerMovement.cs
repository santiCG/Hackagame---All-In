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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentGas = maxGas;

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

        if (rb.linearVelocity.magnitude < stopMagnitude)
        { 
            rb.linearVelocity = Vector3.zero;
        }
    }

    public float GetCurrentGasPercentage()
    {
        return currentGas / maxGas;
    }
}
