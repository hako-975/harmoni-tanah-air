using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Globalization;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameScene currentScene;

    [SerializeField]
    private DialogBarController dialogBar;

    [SerializeField]
    private SpriteSwitcherController spriteSwitcherController;

    [SerializeField]
    private ChooseController chooseController;

    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    private DataHolder dataHolder;

    [SerializeField]
    private Button autoplayButton;
    
    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private PauseController pauseController;

    [SerializeField]
    private SaveController saveController;
    
    [SerializeField]
    private LoadController loadController;

    private bool autoplayBool = false;
    
    private bool isAutoplayRunning = false;

    private List<StoryScene> history = new List<StoryScene>();

    private List<StoryScene> storySceneSaveData = new List<StoryScene>();

    private GameScene currentSceneSaveData;

    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    private Button dialogBarButton;

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("MusicController");
        
        foreach (GameObject music in musicObj)
        {
            Destroy(music);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefsController.instance.IsHasSlotSceneLoadGame())
        {
            SaveData data = PlayerPrefsController.instance.LoadGame(PlayerPrefsController.instance.GetSlotSceneLoadGame());
            data.prevScenes.ForEach(scene =>
            {
                history.Add(dataHolder.scenes[scene] as StoryScene);
            });

            currentScene = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            dialogBar.SetSentenceIndex(data.sentence - 2);
            PlayerPrefsController.instance.DeleteKey("SlotSceneLoadGame");
        }

        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            dialogBar.PlayScene(storyScene, dialogBar.GetSentenceIndex(), true);
            spriteSwitcherController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[dialogBar.GetSentenceIndex()]);
        }

        dialogBarButton = dialogBar.GetComponent<Button>();

        pauseButton.onClick.AddListener(pauseController.OnPauseButtonClick);
        autoplayButton.onClick.AddListener(OnAutoplayButtonClick);
        dialogBarButton.onClick.AddListener(OnDialogBarButtonClick);

        saveController.saveButton1.onClick.AddListener(delegate { OnSaveButtonClick(1); });
        saveController.saveButton2.onClick.AddListener(delegate { OnSaveButtonClick(2); });
        saveController.saveButton3.onClick.AddListener(delegate { OnSaveButtonClick(3); });

        loadController.loadButton1.onClick.AddListener(delegate { OnLoadButtonClick(1); });
        loadController.loadButton2.onClick.AddListener(delegate { OnLoadButtonClick(2); });
        loadController.loadButton3.onClick.AddListener(delegate { OnLoadButtonClick(3); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseController.gameObject.activeSelf)
            {
                pauseController.OnResumeButtonClick();
            }
            else
            {
                pauseController.OnPauseButtonClick();
            }
        }

        if (autoplayBool && !isAutoplayRunning)
        {
            StartCoroutine(Autoplay());
        }
    }

    public void GetDataSaveButton(int slot)
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
                    saveController.saveButton1.GetComponent<Image>().color = Color.white;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 2:
                    saveController.saveButton2.GetComponent<Image>().color = Color.white;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 3:
                    saveController.saveButton3.GetComponent<Image>().color = Color.white;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
            }
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
                    loadController.loadButton1.GetComponent<Image>().color = Color.white;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 2:
                    loadController.loadButton2.GetComponent<Image>().color = Color.white;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 3:
                    loadController.loadButton3.GetComponent<Image>().color = Color.white;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
            }
        }
    }

    private void OnSaveButtonClick(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            saveController.confirmSavePanel.SetActive(true);
            saveController.confirmSaveYesButton.onClick.AddListener( delegate { SaveData(slot); });
            saveController.confirmSaveNoButton.onClick.AddListener(CloseConfirmSavePanel);
        }
        else
        {
            SaveData(slot);
        }
    }

    private void CloseConfirmSavePanel()
    {
        saveController.confirmSavePanel.SetActive(false);
    }

    private void SaveData(int slot)
    {
        List<int> historyIndicies = new List<int>();
        history.ForEach(scene =>
        {
            historyIndicies.Add(dataHolder.scenes.IndexOf(scene));
        });

        DateTime currentDateTime = DateTime.Now;
        SaveData data = new SaveData
        {
            sentence = dialogBar.GetSentenceIndex(),
            prevScenes = historyIndicies,
            dateSaved = currentDateTime.ToString("dddd, dd MMMM yyyy, HH:mm", new CultureInfo("id-ID"))
        };

        PlayerPrefsController.instance.SaveGame(slot, data);

        GetDataSaveButton(slot);
        CloseConfirmSavePanel();
    }

    private void OnLoadButtonClick(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            loadController.confirmLoadPanel.SetActive(true);
            loadController.confirmLoadYesButton.onClick.AddListener(delegate { LoadData(slot); });
            loadController.confirmLoadNoButton.onClick.AddListener(CloseConfirmLoadPanel);
        }
    }

    private void LoadData(int slot)
    {
        PlayerPrefsController.instance.SetSlotSceneLoadGame(slot);
        PlayerPrefsController.instance.SetNextScene("Gameplay");
    }


    private void CloseConfirmLoadPanel()
    {
        loadController.confirmLoadPanel.SetActive(false);
    }

    private void OnAutoplayButtonClick()
    {
        if (autoplayBool == false)
        {
            StartCoroutine(AnimationAutoplayOn());
        }
        else
        {
            StartCoroutine(AnimationAutoplayOff());
        }
    }

    private void OnDialogBarButtonClick()
    {
        if (state == State.IDLE)
        {
            if (dialogBar.IsCompleted())
            {
                dialogBar.StopTyping();
                if (dialogBar.IsLastSentence())
                {
                    PlayScene((currentScene as StoryScene).nextScene);
                }
                else
                {
                    dialogBar.PlayNextSentence();
                    PlayAudio((currentScene as StoryScene).sentences[dialogBar.GetSentenceIndex()]);
                }
            }
            else
            {
                dialogBar.SpeedUp();
            }
        }
    }

    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated));
    }

    private IEnumerator AnimationAutoplayOn()
    {
        dialogBarButton.interactable = false;
        autoplayBool = true;
        autoplayButton.GetComponent<Animator>().SetTrigger("On");
        yield return new WaitForSeconds(0.5f);
        autoplayButton.GetComponent<Animator>().SetTrigger("Loop");
    }

    private IEnumerator AnimationAutoplayOff()
    {
        dialogBarButton.interactable = true;
        autoplayBool = false;
        autoplayButton.GetComponent<Animator>().SetTrigger("Off");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Autoplay()
    {
        if (dialogBar.IsCompleted())
        {
            isAutoplayRunning = true;
            float waitTime = PlayerPrefsController.instance.GetAutoForward();
            yield return new WaitForSeconds(waitTime);
            OnDialogBarButtonClick();
            isAutoplayRunning = false;
        }
    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        state = State.ANIMATE;
        currentScene = scene;
        if (isAnimated)
        {
            dialogBar.Hide();
            yield return new WaitForSeconds(1f);
        }
        
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            history.Add(storyScene);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]);
            if (isAnimated)
            {
                spriteSwitcherController.SwitchImage(storyScene.background);
                yield return new WaitForSeconds(1f);
                dialogBar.ClearText();
                yield return new WaitForSeconds(1f);
                dialogBar.Show();
            }
            else
            {
                spriteSwitcherController.SetImage(storyScene.background);
                dialogBar.ClearText();
            }
            dialogBar.PlayScene(storyScene, sentenceIndex, isAnimated);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }
}