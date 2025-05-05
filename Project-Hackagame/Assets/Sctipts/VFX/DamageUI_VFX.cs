using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class DamageUI_VFX : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume; // Reference to the Post-Processing Volume
    private Vignette vignette;
    private LensDistortion lensDistortion;

    [Header("Vignette Settings")]
    [SerializeField] private float duration = 1.0f; // Duration for fading effects
    [SerializeField] private float oxygenThreshold = 30f; // Threshold for low oxygen
    [SerializeField] private float maxVignetteIntensity = 0.6f; // Maximum vignette intensity
    [SerializeField] private float maxVignetteSmoothness = 1.0f; // Maximum vignette smoothness
    [SerializeField] private float initialVignetteSmoothness = 0.4f; // Starting vignette smoothness

    [Header("Lens Distortion Settings")]
    [SerializeField] private float maxLensDistortion = -0.5f; // Maximum lens distortion
    [SerializeField] private float initialLensDistortion = 0f; // Starting lens distortion

    private Dialogue dialogueScript;
    private bool firstAlmostDead = false;

    private void Awake()
    {
        // Ensure the Post-Processing Volume is assigned
        if (postProcessingVolume == null)
        {
            postProcessingVolume = FindFirstObjectByType<Volume>();
        }

        // Ensure the Dialogue script is assigned
        if (dialogueScript == null)
        {
            dialogueScript = FindFirstObjectByType<Dialogue>();
        }
    }

    private void Start()
    {
        // Get the Vignette effect from the Volume
        if (postProcessingVolume != null)
        {
            if (postProcessingVolume.profile.TryGet(out Vignette vignetteEffect))
            {
                vignette = vignetteEffect;
                vignette.intensity.value = 0f; // Ensure the vignette starts at 0
                vignette.smoothness.value = initialVignetteSmoothness; // Set initial smoothness
            }
            else
            {
                Debug.LogError("Vignette effect not found in the Post-Processing Volume!");
            }

            if (postProcessingVolume.profile.TryGet(out LensDistortion lensDistortionEffect))
            {
                lensDistortion = lensDistortionEffect;
                lensDistortion.intensity.value = initialLensDistortion; // Set initial lens distortion
            }
            else
            {
                Debug.LogError("Lens Distortion effect not found in the Post-Processing Volume!");
            }
        }
        else
        {
            Debug.LogError("Post-Processing Volume is not assigned!");
        }
    }

    public void UpdateDamageEffect(float oxygen, float maxOxygen)
    {
        if (vignette == null || lensDistortion == null) return;

        if (oxygen < oxygenThreshold)
        {
            if(!firstAlmostDead)
            {
                firstAlmostDead = true;
                dialogueScript.TriggerDialogue(6); // Trigger the almost dead dialogue
            }

            // Calculate vignette intensity and smoothness based on oxygen level
            float intensity = Mathf.Lerp(0f, maxVignetteIntensity, (oxygenThreshold - oxygen) / oxygenThreshold);
            float smoothness = Mathf.Lerp(initialVignetteSmoothness, maxVignetteSmoothness, (oxygenThreshold - oxygen) / oxygenThreshold);

            // Calculate lens distortion based on oxygen level
            float distortion = Mathf.Lerp(initialLensDistortion, maxLensDistortion, (oxygenThreshold - oxygen) / oxygenThreshold);

            // Apply the calculated values
            vignette.intensity.value = intensity;
            vignette.smoothness.value = smoothness;
            lensDistortion.intensity.value = distortion;
        }
        else
        {
            // Reset effects when oxygen is above the threshold
            vignette.intensity.value = 0f;
            vignette.smoothness.value = initialVignetteSmoothness;
            lensDistortion.intensity.value = initialLensDistortion;
        }
    }

    public void FadeToBlack(TweenCallback onComplete)
    {
        if (vignette == null) return;

        // Fade the vignette intensity to maximum (1.0) over the duration
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, duration)
            .OnComplete(onComplete);

        // Optionally, fade smoothness and lens distortion to their maximum values
        DOTween.To(() => vignette.smoothness.value, x => vignette.smoothness.value = x, maxVignetteSmoothness, duration);
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, maxLensDistortion, duration);
    }
}