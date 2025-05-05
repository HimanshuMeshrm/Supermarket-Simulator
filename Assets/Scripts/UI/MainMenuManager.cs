using MNDRiN.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public UIToggle MusicToggle, SoundToggle;

    public GameOptions GameOptions = new GameOptions();
    private void Start()
    {
        ShowMainMenu();
        GameOptions = SaveManager.GetCurrentGameSave().GameOptions;


        MusicToggle.SetState(GameOptions.GameMusic > 0);
        SoundToggle.SetState(GameOptions.GameSound > 0);
    }

    private void Update()
    {
        HandleMainMenuInput();
    }

    private void HandleMainMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void ShowMainMenu()
    {
        SetActiveUI(mainMenuPanel);
       
    }

    public void StartGame()
    {
        SetActiveUI(null);
        SceneManager.LoadScene("MainScene");
        
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }

    public void OpenSettings()
    {
        SetActiveUI(settingsPanel);

        
    }

    public void CloseSettings()
    {
        GameOptions.GameMusic = MusicToggle.IsOn ? 1 : 0;
        GameOptions.GameSound = SoundToggle.IsOn ? 1 : 0;

        SaveManager.GetCurrentGameSave().GameOptions = GameOptions;
        SaveManager.SaveGame();
        SetActiveUI(mainMenuPanel);
    }

    private void SetActiveUI(GameObject activePanel)
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (activePanel != null)
        {
            activePanel.SetActive(true);
        }
    }
}
