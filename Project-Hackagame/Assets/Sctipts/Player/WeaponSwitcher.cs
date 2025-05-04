// WeaponSwitcher.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject handObject;
    public GameObject gunObject;

    private bool isGunEquipped = false;

    public void EquipHand(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            handObject.SetActive(true);
            gunObject.SetActive(false);
            isGunEquipped = false;
        }
    }

    public void EquipGun(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            handObject.SetActive(false);
            gunObject.SetActive(true);
            isGunEquipped = true;
        }
    }

    public bool IsGunEquipped()
    {
        return isGunEquipped;
    }
}
