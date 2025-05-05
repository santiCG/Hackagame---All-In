using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public bool lockRotation;
    public float rotationSpeed = 2f;

    public bool lockXZRotation = false;
    public bool isRotating = false;
    private Vector2 lookInput;

    public bool IsRotationLocked { get; private set; } = false;

    public void SetRotationLock(bool locked)
    {
        IsRotationLocked = locked;
    }


    public void SetRotationXYLock(bool state)
    {
        lockXZRotation = state;
    }


    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
        }
        else if (context.canceled)
        {
            isRotating = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (IsRotationLocked) return;

        if (lookInput == Vector2.zero) return;

        float defaultRotation = lookInput.x * rotationSpeed * Time.deltaTime;  // Horizontal mouse → Yaw
        float pitch = -lookInput.y * rotationSpeed * Time.deltaTime; // Vertical mouse → Pitch
        float yaw = -lookInput.x * rotationSpeed * Time.deltaTime;  // Horizontal mouse → Yaw

        if (!isRotating) 
        {
            transform.Rotate(Vector3.up, defaultRotation, Space.Self); // Yaw (Y axis)
            transform.Rotate(Vector3.right, pitch, Space.Self); // Pitch (X axis)
        }
        else
        {
            transform.Rotate(Vector3.right, pitch, Space.Self); // Pitch (X axis)
            transform.Rotate(Vector3.forward, yaw, Space.Self); // Yaw (Z axis)
        }
    }
}
