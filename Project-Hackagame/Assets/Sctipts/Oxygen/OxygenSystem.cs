using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f; 
    [SerializeField] private float oxygenDepletionRate = 1f;
    [SerializeField] private Image oxygenFillImage;

    public float currentOxygen;
    private bool isInSafeZone = true;

    public delegate void OxygenDepleted();
    public event OxygenDepleted OnOxygenDepleted;

    [SerializeField] private DamageUI_VFX damageUIVFX;
    [SerializeField] private GameManager gameManager;

    private Dialogue dialogueScript;

    private void Awake()
    {
        if (oxygenFillImage == null)
        {
            oxygenFillImage = FindFirstObjectByType<Image>();
        }

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        if (dialogueScript == null)
        {
            dialogueScript = FindFirstObjectByType<Dialogue>();
        }
    }

    private void Start()
    {
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
        if (!isInSafeZone)
        {
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

            // Update the damage effect based on oxygen level
            if (damageUIVFX != null)
            {
                damageUIVFX.UpdateDamageEffect(currentOxygen, maxOxygen);
            }

            // Check if oxygen is depleted
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                OnOxygenDepleted?.Invoke();

                // Notify the GameManager of the player's death
                if (gameManager != null)
                {
                    gameManager.HandlePlayerDeath();
                }
            }
        }

        UpdateOxygenUI();
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0, maxOxygen);
        UpdateOxygenUI();
    }

    public void SetDepletionRate(float newRate)
    {
        oxygenDepletionRate = newRate;
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