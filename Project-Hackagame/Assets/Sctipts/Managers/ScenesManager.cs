using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float delay = 2f;
    [SerializeField] private Image fadeImage; 
    [SerializeField] private GameObject panel;

    private void Start()
    {
        // Start with a fade-in effect
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            LeanTween.alpha(fadeImage.rectTransform, 0f, 1f).setOnComplete(() =>
            {
                fadeImage.gameObject.SetActive(false);
            });
        }
    }

    // Method to load a scene with a fade-out effect
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);
                LeanTween.alpha(fadeImage.rectTransform, 1f, 1f).setOnComplete(() =>
                {
                    SceneManager.LoadScene(sceneToLoad);
                });
            }
            else
            {
                Debug.LogWarning("Fade image is not assigned!");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else
        {
            Debug.LogWarning("Scene name is empty or null!");
        }
    }

    public void FadeToMainMenu(float fadeDuration)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            LeanTween.alpha(fadeImage.rectTransform, 1f, fadeDuration).setOnComplete(() =>
            {
                SceneManager.LoadScene(sceneToLoad);
            });
        }
        else
        {
            Debug.LogWarning("Fade image is not assigned!");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RestartScene()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            LeanTween.alpha(fadeImage.rectTransform, 1f, 1f).setOnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
        else
        {
            Debug.LogWarning("Fade image is not assigned!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Method to quit the game with a fade-out effect
    public void QuitGame()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            LeanTween.alpha(fadeImage.rectTransform, 1f, 1f).setOnComplete(() =>
            {
                Debug.Log("Quitting the game...");
                Application.Quit();
            });
        }
        else
        {
            Debug.LogWarning("Fade image is not assigned!");
            Application.Quit();
        }
    }

    // Method to show the panel
    public void ShowPanel(bool pauseGame)
    {
        if (panel != null && panel.activeSelf == false)
        {
            panel.SetActive(true);

            if (pauseGame)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Show the cursor
                Time.timeScale = 0f; // Pause the game
            }
        }
        else
        {
            HidePanel(); // Hide the panel if it's already active
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
        }
    }

    // Method to hide the panel
    public void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);

            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("Panel is null!");
        }
    }

    public void HidePanelAndResumeGame()
    {
        if(!panel.activeSelf)  return;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        HidePanel();
    }
}