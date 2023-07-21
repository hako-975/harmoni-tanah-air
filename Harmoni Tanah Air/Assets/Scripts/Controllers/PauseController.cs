using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [Header("Buttons")]
    [SerializeField]
    private Button pauseButton;
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

    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        historyButton.onClick.AddListener(OnHistoryButtonClick);
        saveButton.onClick.AddListener(OnSaveButtonClick);
        loadButton.onClick.AddListener(OnLoadButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }
    
    private void OnPauseButtonClick()
    {
        DisabledAllPanel();
        historyPanel.SetActive(true);
        pausePanel.SetActive(true);
    }

    private void OnResumeButtonClick()
    {
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
        SceneManager.LoadScene("MainMenu");
    }

    private void DisabledAllPanel()
    {
        historyPanel.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
