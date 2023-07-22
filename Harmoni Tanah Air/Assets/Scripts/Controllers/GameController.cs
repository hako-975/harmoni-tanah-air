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
        dialogBarButton = dialogBar.GetComponent<Button>();

        // load data 1 for showing data in save panel
        LoadSaveData(1);
        // load data 2 for showing data in save panel
        LoadSaveData(2);
        // load data 3 for showing data in save panel
        LoadSaveData(3);

        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            dialogBar.PlayScene(storyScene, dialogBar.GetSentenceIndex());
            spriteSwitcherController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[dialogBar.GetSentenceIndex()]);
        }
        
        pauseButton.onClick.AddListener(pauseController.OnPauseButtonClick);
        autoplayButton.onClick.AddListener(OnAutoplayButtonClick);
        dialogBarButton.onClick.AddListener(OnDialogBarButtonClick);
        saveController.saveButton1.onClick.AddListener(delegate { OnSaveButtonClick(1); });
        saveController.saveButton2.onClick.AddListener(delegate { OnSaveButtonClick(2); });
        saveController.saveButton3.onClick.AddListener(delegate { OnSaveButtonClick(3); });
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

    private void LoadSaveData(int slot)
    {
        if (saveController.IsGameSaved(slot))
        {
            SaveData data = saveController.LoadGame(slot);
            data.prevScenes.ForEach(scene =>
            {
                storySceneSaveData.Add(this.dataHolder.scenes[scene] as StoryScene);
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

    private void OnSaveButtonClick(int slot)
    {
        if (saveController.IsGameSaved(slot))
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
            historyIndicies.Add(this.dataHolder.scenes.IndexOf(scene));
        });

        DateTime currentDateTime = DateTime.Now;
        SaveData data = new SaveData
        {
            sentence = dialogBar.GetSentenceIndex(),
            prevScenes = historyIndicies,
            dateSaved = currentDateTime.ToString("dddd, dd MMMM yyyy, HH:mm", new CultureInfo("id-ID"))
        };

        saveController.SaveGame(slot, data);

        LoadSaveData(slot);
        CloseConfirmSavePanel();
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