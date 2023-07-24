using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    
    [SerializeField]
    private Button loadDataButton1;
    [SerializeField]
    private Button loadDataButton2;
    [SerializeField]
    private Button loadDataButton3;

    [SerializeField]
    private DataHolder dataHolder;

    private List<StoryScene> storySceneSaveData = new List<StoryScene>();

    private GameScene currentSceneSaveData;

    void Start()
    {
        animator = GetComponent<Animator>();
        loadButton.interactable = PlayerPrefsController.instance.IsHasGameSaved();
        newGameButton.onClick.AddListener(NewGameButton);

        loadDataButton1.onClick.AddListener(delegate { OnLoadButtonClick(1); });
        loadDataButton2.onClick.AddListener(delegate { OnLoadButtonClick(2); });
        loadDataButton3.onClick.AddListener(delegate { OnLoadButtonClick(3); });

        GetDataLoadButton(1);
        GetDataLoadButton(2);
        GetDataLoadButton(3);
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

    private void OnLoadButtonClick(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            PlayerPrefsController.instance.SetSlotSceneLoadGame(slot);
            PlayerPrefsController.instance.SetNextScene("Gameplay");
        }
    }

    public void GetDataLoadButton(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            SaveData data = PlayerPrefsController.instance.LoadGame(slot);
            data.prevScenes.ForEach(scene =>
            {
                storySceneSaveData.Add(dataHolder.scenes[scene] as StoryScene);
            });

            currentSceneSaveData = storySceneSaveData[storySceneSaveData.Count - 1];
            switch (slot)
            {
                case 1:
                    loadDataButton1.GetComponent<Image>().color = Color.white;
                    loadDataButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadDataButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadDataButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadDataButton1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 2:
                    loadDataButton2.GetComponent<Image>().color = Color.white;
                    loadDataButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadDataButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadDataButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadDataButton2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 3:
                    loadDataButton3.GetComponent<Image>().color = Color.white;
                    loadDataButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadDataButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadDataButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadDataButton3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
            }
        }
    }

    public void QuitButton()
    {
        Debug.Log("Keluar");
        Application.Quit();
    }
}
