using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            dialogBar.PlayScene(storyScene);
            spriteSwitcherController.SetImage(storyScene.background);
        }

        dialogBarButton.onClick.AddListener(OnDialogBarButtonClick);
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
            }
        }
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;
        dialogBar.Hide();
        yield return new WaitForSeconds(1f);
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            spriteSwitcherController.SwitchImage(storyScene.background);
            yield return new WaitForSeconds(1f);
            dialogBar.ClearText();
            yield return new WaitForSeconds(1f);
            dialogBar.Show();
            dialogBar.PlayScene(storyScene);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }
}