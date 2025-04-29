using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu panel
    private bool isPaused = false;  // Track if the game is paused

    void Update()
    {
        // Debugging the current scene name and Escape key press
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);

        // Check if the Escape key is pressed while in the game scene
        if (SceneManager.GetActiveScene().name == "GameSceneName" && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            if (isPaused)
            {
                ResumeGame();  // If paused, resume the game
            }
            else
            {
                PauseGame();  // If not paused, show the pause menu
            }
        }
    }

    // Show the pause menu and stop the game time
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // Show the pause menu
        Time.timeScale = 0f;  // Pause the game (freeze gameplay)
        isPaused = true;  // Set paused state to true
    }

    // Hide the pause menu and resume the game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);  // Hide the pause menu
        Time.timeScale = 1f;  // Resume the game (unfreeze gameplay)
        isPaused = false;  // Set paused state to false
    }

    // Quit to the main menu scene
    public void QuitGame()
    {
        Time.timeScale = 1f;  // Make sure the game is resumed
        SceneManager.LoadScene("MainMenu");  // Change to the name of your main menu scene
    }
}



