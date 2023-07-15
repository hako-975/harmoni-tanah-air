using System.Collections;
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

    private enum State
    {
        PLAYING, COMPLETED
    }

    private void Start()
    {
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

    public bool IsLastSentence()
    {
        return sentenceIndex == currentScene.sentences.Count;
    }

    public void PlayNextSentence()
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
}
