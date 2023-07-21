using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private DataHolder data;

    private List<StoryScene> history = new List<StoryScene>();

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

        if (SaveController.IsGameSaved())
        {
            SaveData data = SaveController.LoadGame();
            data.prevScenes.ForEach(scene =>
            {
                history.Add(this.data.scenes[scene] as StoryScene);
            });

            currentScene = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            dialogBar.SetSentenceIndex(data.sentence - 1);
        }

        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            dialogBar.PlayScene(storyScene, dialogBar.GetSentenceIndex());
            spriteSwitcherController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[dialogBar.GetSentenceIndex()]);
        }

        dialogBarButton.onClick.AddListener(OnDialogBarButtonClick);
    }

    private void OnSaveButtonClick()
    {
        Debug.Log("Saved");
        List<int> historyIndicies = new List<int>();
        history.ForEach(scene =>
        {
            historyIndicies.Add(this.data.scenes.IndexOf(scene));
        });

        SaveData data = new SaveData
        {
            sentence = dialogBar.GetSentenceIndex(),
            prevScenes = historyIndicies
        };

        SaveController.SaveGame(data);
        PlayerPrefsController.instance.SetNextScene("MainMenu");
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