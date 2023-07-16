using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBarController : MonoBehaviour
{
    [SerializeField]
    private GameObject nameBoxTextUI;
    
    [SerializeField]
    private TextMeshProUGUI textBoxText;

    [SerializeField]
    private StoryScene currentScene;

    private TextMeshProUGUI nameBoxText;

    private int sentenceIndex = 0;

    private State state = State.COMPLETED;

    private Animator animator;

    private bool isHidden = false;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesGameObject;

    private enum State
    {
        PLAYING, COMPLETED
    }

    private void Start()
    {
        sprites = new Dictionary<Speaker, SpriteController>();
        nameBoxText = nameBoxTextUI.GetComponentInChildren<TextMeshProUGUI>();

        animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        }
    }
    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        nameBoxText.text = "";
        textBoxText.text = "";
    }

    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = 0;
        PlayNextSentence();
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsPlaying()
    {
        return state == State.PLAYING;
    }

    public void SetStateCompleted()
    {
        state = State.COMPLETED;

        ClearText();

        sentenceIndex -= 1;
        
        textBoxText.text = currentScene.sentences[sentenceIndex].text;

        SetNameBox();
        
        sentenceIndex += 1;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex == currentScene.sentences.Count;
    }

    private void SetNameBox()
    {
        if (currentScene.sentences[sentenceIndex].speaker.speakerName != "Narrator")
        {
            nameBoxTextUI.SetActive(true);

            nameBoxText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
            nameBoxText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
        }
        else
        {
            nameBoxTextUI.SetActive(false);
        }
    }

    public void PlayNextSentence()
    {
        SetNameBox();
        ActSpeakers();
        StartCoroutine(TypeText(currentScene.sentences[sentenceIndex++].text));

    }

    private IEnumerator TypeText(string text)
    {
        textBoxText.text = "";

        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            textBoxText.text += text[wordIndex];
            yield return new WaitForSeconds(PlayerPrefs.GetFloat("TextSpeed", 0.05f));

            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }

    private void ActSpeakers()
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i]);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.Action action)
    {
        SpriteController controller = null;
        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.NONE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                }
                break;
            case StoryScene.Sentence.Action.Type.APPEAR:
                if (!sprites.ContainsKey(action.speaker))
                {
                    controller = Instantiate(action.speaker.prefab.gameObject, spritesGameObject.transform).GetComponent<SpriteController>();
                    sprites.Add(action.speaker, controller);
                }
                else
                {
                    controller = sprites[action.speaker];
                }

                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Show(action.coords);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Move(action.coords, action.moveSpeed);
                }
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Hide();
                }
                break;
        }

        if (controller != null)
        {
            controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
        }
    }
}