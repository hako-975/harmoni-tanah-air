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

    [SerializeField]
    private string menuScene;

    private List<StoryScene> history = new List<StoryScene>();

    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    private Button dialogBarButton;

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

    // nanti ganti tombol
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
            SceneManager.LoadScene(menuScene);
        }
    }

    private void OnDialogBarButtonClick()
    {
        if (dialogBar.IsPlaying())
        {
            dialogBar.SetStateCompleted();
            return;
        }
        else if (state == State.IDLE && dialogBar.IsCompleted())
        {
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
    }

    public void PlayScene(GameScene scene, int sentenceIndex = -1)
    {
        StartCoroutine(SwitchScene(scene, sentenceIndex));
    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1)
    {
        state = State.ANIMATE;
        currentScene = scene;
        dialogBar.Hide();
        yield return new WaitForSeconds(1f);
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            history.Add(storyScene);
            spriteSwitcherController.SwitchImage(storyScene.background);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]);
            yield return new WaitForSeconds(1f);
            dialogBar.ClearText();
            yield return new WaitForSeconds(1f);
            dialogBar.Show();
            dialogBar.PlayScene(storyScene, sentenceIndex);
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