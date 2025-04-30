using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                // Close settings if it's open
                settingsPanel.SetActive(false);
                pauseMenuPanel.SetActive(true);
            }
            else
            {
                TogglePauseMenu();
            }
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze game time
        }
        else
        {
            Time.timeScale = 1f; // Resume game time
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game time resumes
        SceneManager.LoadScene("MainMenu"); // Replace with your actual scene name
    }

    void OnDisable()
    {
        Time.timeScale = 1f; // Safety net to resume time
    }
}





