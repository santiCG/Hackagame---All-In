using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f; 
    [SerializeField] private float oxygenDepletionRate = 1f;
    [SerializeField] private Image oxygenFillImage;

    private float currentOxygen;
    private bool isInSafeZone = true;

    public delegate void OxygenDepleted();
    public event OxygenDepleted OnOxygenDepleted;

    private void Start()
    {
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
        if(!isInSafeZone){
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;

            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        }

        UpdateOxygenUI();

        // Check if oxygen is depleted
        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            OnOxygenDepleted?.Invoke();
        }
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0, maxOxygen);
        UpdateOxygenUI();
    }

    public float GetCurrentOxygen()
    {
        return currentOxygen;
    }

    private void UpdateOxygenUI()
    {
        if (oxygenFillImage != null)
        {
            oxygenFillImage.fillAmount = currentOxygen / maxOxygen;
        }
    }

    public void EnterSafeZone()
    {
        isInSafeZone = true;
    }

    public void ExitSafeZone()
    {
        isInSafeZone = false;
    }
}