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

    bool windowLoad = false;
    bool windowSettings = false;
    bool windowCredit = false;
    bool windowGaleri = false;
    bool windowQuit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        loadButton.interactable = PlayerPrefsController.instance.IsHasGameSaved();
        newGameButton.onClick.AddListener(NewGameButton);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (windowLoad || windowSettings || windowCredit || windowGaleri || windowQuit))
        {
            if (windowLoad)
            {
                animator.SetTrigger("HideLoad");
            }
            else if (windowSettings)
            {
                animator.SetTrigger("HideSettings");
            }
            else if (windowCredit)
            {
                animator.SetTrigger("HideCredit");
            }
            else if (windowGaleri)
            {
                animator.SetTrigger("HideGaleri");
            }
            else if (windowQuit)
            {
                animator.SetTrigger("HideQuit");
            }

            windowSettings = false;
            windowLoad = false;
            windowCredit = false;
            windowGaleri = false;
            windowQuit = false;
        }
    }

    public void NewGameButton()
    {
        PlayerPrefsController.instance.SetNextScene("Gameplay");
    }

    public void ShowLoad()
    {
        animator.SetTrigger("ShowLoad");
        windowLoad = true;
    }

    public void HideLoad()
    {
        animator.SetTrigger("HideLoad");
        windowLoad = false;
    }

    public void ShowSettings()
    {
        animator.SetTrigger("ShowSettings");
        windowSettings = true;
    }

    public void HideSettings()
    {
        animator.SetTrigger("HideSettings");
        windowSettings = false;
    }

    public void ShowCredit()
    {
        animator.SetTrigger("ShowCredit");
        windowCredit = true;
    }

    public void HideCredit()
    {
        animator.SetTrigger("HideCredit");
        windowCredit = false;
    }

    public void ShowGaleri()
    {
        animator.SetTrigger("ShowGaleri");
        windowGaleri = true;
    }

    public void HideGaleri()
    {
        animator.SetTrigger("HideGaleri");
        windowGaleri = false;
    }

    public void ShowQuit()
    {
        animator.SetTrigger("ShowQuit");
        windowQuit = true;
    }

    public void HideQuit()
    {
        animator.SetTrigger("HideQuit");
        windowQuit = false;
    }

    public void QuitButton()
    {
        Debug.Log("Keluar");
        Application.Quit();
    }
}
