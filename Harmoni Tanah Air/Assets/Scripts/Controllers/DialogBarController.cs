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

    private TextMeshProUGUI nameBoxText;

    private StoryScene currentScene;

    private int sentenceIndex = -1;

    private State state = State.COMPLETED;

    private Animator animator;

    private bool isHidden = false;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesGameObject;

    private Coroutine typingCoroutine;
    private float speedFactor = 1f;

    private enum State
    {
        PLAYING, SPEEDED_UP, COMPLETED
    }

    private void Start()
    {
        sprites = new Dictionary<Speaker, SpriteController>();

        nameBoxText = nameBoxTextUI.GetComponentInChildren<TextMeshProUGUI>();

        animator = GetComponent<Animator>();
    }

    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    public void SetSentenceIndex(int sentenceIndex)
    {
        if (sentenceIndex < -1)
        {
            sentenceIndex = -1;
        }

        this.sentenceIndex = sentenceIndex;
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

    public void PlayScene(StoryScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        currentScene = scene;
        this.sentenceIndex = sentenceIndex;
        PlayNextSentence(isAnimated);
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsPlaying()
    {
        return state == State.PLAYING;
    }

    public bool IsFirstSentence()
    {
        return sentenceIndex == 0;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
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

    public void PlayNextSentence(bool isAnimated = true)
    {
        sentenceIndex++;
        PlaySentence(isAnimated);
    }

    public void GoBack()
    {
        sentenceIndex--;
        StopTyping();
        HideSprites();
        PlaySentence(false);
    }

    public void SpeedUp()
    {
        state = State.SPEEDED_UP;
        speedFactor = 0.25f;
    }

    public void StopTyping()
    {
        state = State.COMPLETED;
        StopCoroutine(typingCoroutine);
    }

    public void HideSprites()
    {
        while (spritesGameObject.transform.childCount > 0)
        {
            DestroyImmediate(spritesGameObject.transform.GetChild(0).gameObject);
        }

        sprites.Clear();
    }

    private void PlaySentence(bool isAnimated = true)
    {
        speedFactor = 1f;
        typingCoroutine = StartCoroutine(TypeText(currentScene.sentences[sentenceIndex].text));
        SetNameBox();
        ActSpeakers(isAnimated);
    }

    private IEnumerator TypeText(string text)
    {
        textBoxText.text = "";

        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            textBoxText.text += text[wordIndex];
            yield return new WaitForSeconds(speedFactor * (PlayerPrefsController.instance.GetTextSpeed() / 1000f));

            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }

    private void ActSpeakers(bool isAnimated = true)
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i], isAnimated);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.Action action, bool isAnimated = true)
    {
        // timpa coords y
        /*action.coords.y = 165;*/

        SpriteController controller;

        if (!sprites.ContainsKey(action.speaker))
        {
            controller = Instantiate(action.speaker.prefab.gameObject, spritesGameObject.transform).GetComponent<SpriteController>();
            sprites.Add(action.speaker, controller);
        }
        else
        {
            controller = sprites[action.speaker];
        }

        /*switch (action.actionType)
        {
            *//*case StoryScene.Sentence.Action.Type.APPEAR:
                controller.Setup(action.sprite);
                controller.Show(action.coords, isAnimated);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:
                controller.SwitchSprite(action.coords, action.sprite, isAnimated);
                controller.Move(action.coords, action.speedOrDuration, isAnimated);
                return;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                controller.Hide(action.speedOrDuration, isAnimated);
                return;
            case StoryScene.Sentence.Action.Type.NONE:
                controller.SwitchSprite(action.coords, action.sprite, isAnimated);
                return;*//*
        }*/

    }
}