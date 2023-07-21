using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button loadButton;

    private Animator animator;

    bool window = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        loadButton.interactable = SaveController.IsGameSaved();
        loadButton.onClick.AddListener(LoadGameButton);

        newGameButton.onClick.AddListener(NewGameButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && window)
        {
            animator.SetTrigger("Hide");
            window = false;
        }
    }

    public void NewGameButton()
    {
        SaveController.ClearSavedGame();
        LoadGameButton();
    }

    public void LoadGameButton()
    {
        PlayerPrefsController.instance.SetNextScene("Gameplay");
    }

    public void ShowSettings()
    {
        animator.SetTrigger("Show");
        window = true;
    }

    public void HideSettings()
    {
        animator.SetTrigger("Hide");
        window = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
