using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject historyPanel;
    [SerializeField]
    private GameObject savePanel;
    [SerializeField]
    private GameObject loadPanel;
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject mainMenuPanel;

    [Header("Buttons")]
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button historyButton;
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private Button mainMenuYesButton;
    [SerializeField]
    private Button mainMenuNoButton;


    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        historyButton.onClick.AddListener(OnHistoryButtonClick);
        saveButton.onClick.AddListener(OnSaveButtonClick);
        loadButton.onClick.AddListener(OnLoadButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        mainMenuYesButton.onClick.AddListener(OnMainMenuYesButtonClick);
        mainMenuNoButton.onClick.AddListener(OnMainMenuNoButtonClick);
    }
    
    public void OnPauseButtonClick()
    {
        Time.timeScale = 0f;
        DisabledAllPanel();
        historyPanel.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    private void OnHistoryButtonClick()
    {
        DisabledAllPanel();
        historyPanel.SetActive(true);
    }
    private void OnSaveButtonClick()
    {
        DisabledAllPanel();
        savePanel.SetActive(true);
    }
    private void OnLoadButtonClick()
    {
        DisabledAllPanel();
        loadPanel.SetActive(true);
    }
    private void OnSettingsButtonClick()
    {
        DisabledAllPanel();
        settingsPanel.SetActive(true);
    }
    private void OnMainMenuButtonClick()
    {
        DisabledAllPanel();
        mainMenuPanel.SetActive(true);
    }

    private void OnMainMenuYesButtonClick()
    {
        Time.timeScale = 1f;
        PlayerPrefsController.instance.SetNextScene("MainMenu");
    }

    private void OnMainMenuNoButtonClick()
    {
        DisabledAllPanel();
        historyPanel.SetActive(true);
    }

    private void DisabledAllPanel()
    {
        historyPanel.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }
}
