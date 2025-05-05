using MNDRiN.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public EntityThoughtUI Prefab;
    public ObjectPool ThoughtsUIPool;

    public TMP_Text MoneyText;

    public Canvas WorldSpaceCanvas;

    private GameOptions GameOptions = new GameOptions();

    public UIToggle MusicToggle, SoundToggle;

    public GameObject PauseMenuPanel, OptionsPanel;

    public bool IsPaused { get; private set; }  = false;
    public GameObject RestockUI;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !OptionsPanel.activeInHierarchy)
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();


            }
        }
    }

    Camera _camera;
    public void SetRestockUI(Shelve shhelve)
    {
        if (_camera == null)
            _camera = Camera.main;

        if (shhelve != null)
        {
            RestockUI.gameObject.SetActive(true);
            RestockUI.transform.position = shhelve.transform.position + Vector3.up * 2f;


            Vector3 direction = _camera.transform.position - RestockUI.transform.position;
            direction.y = 0;
            RestockUI.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
    public void HideRestock()
    {
        RestockUI.gameObject.SetActive(false);
    }
    public void PauseGame()
    {
        IsPaused = true;
        SetActiveUI(PauseMenuPanel);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        GameOptions.GameMusic = MusicToggle.IsOn ? 1 : 0;
        GameOptions.GameSound = SoundToggle.IsOn ? 1 : 0;
        

        SaveManager.GetCurrentGameSave().GameOptions = GameOptions;
        SaveManager.SaveGame();
        IsPaused = false;
        SetActiveUI(null);
        Time.timeScale = 1;

        SoundManager.Instance.MainMusicActivate(GameOptions.GameMusic > 0);
    }
    public void GoToMainMenu()
    {
        GameSave currentGameSave = SaveManager.GetCurrentGameSave();
        PlayerInfo playerInfo = new PlayerInfo();

        playerInfo.MoneyAccount = GameManager.Instance.Player.MoneyAccount;
        playerInfo.PlayerPosition = new SavableVector3(GameManager.Instance.Player.transform.position);

        currentGameSave.PlayerInfo = playerInfo;

        SaveManager.GetCurrentGameSave().GameOptions = GameOptions;
        SaveManager.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    private void Awake()
    {
        CreateThoughtUIPool();
        
    }

    private void Start()
    {
        GameOptions = SaveManager.GetCurrentGameSave().GameOptions;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null");
            return;
        }

        if (GameManager.Instance.Player == null)
        {
            Debug.LogError("GameManager.Instance.Player is null");
            return;
        }

        if (GameManager.Instance.Player.MoneyAccount == null)
        {
            Debug.LogError("Player.MoneyAccount is null");
            return;
        }

        if (GameManager.Instance.Player.MoneyAccount.OnMoneyChanged == null)
        {
            Debug.LogError("OnMoneyChanged is null");
            return;
        }

        GameManager.Instance.Player.MoneyAccount.OnMoneyChanged.AddListener(UpdateMoney);

        MusicToggle.SetState(GameOptions.GameMusic > 0);
        SoundToggle.SetState(GameOptions.GameSound > 0);
    }

    public void CreateThoughtUIPool()
    {
        int count = 20;
        GameObject go = new GameObject("ThougsUIHolder");
        go.transform.SetParent(WorldSpaceCanvas.transform);
        ThoughtsUIPool = new ObjectPool(Prefab.gameObject, count, go.transform, "ThoughtUI");
    }
    void UpdateMoney(int money)
    {
        string str = $"${money}";
        MoneyText.text = str;
    }

    public void SetActiveUI(GameObject activePanel)
    {
        PauseMenuPanel.SetActive(false);
        OptionsPanel.SetActive(false);

        if (activePanel != null)
        {
            activePanel.SetActive(true);
        }
    }
}
