using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string gameScene;

    [SerializeField]
    private TextMeshProUGUI textSpeedValue;
    [SerializeField]
    private TextMeshProUGUI autoForwardValue;
    [SerializeField]
    private TextMeshProUGUI musicValue;
    [SerializeField]
    private TextMeshProUGUI soundValue;

    [SerializeField]
    private AudioMixer musicMixer;

    [SerializeField]
    private AudioMixer soundMixer;

    private Animator animator;

    bool window = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && window)
        {
            animator.SetTrigger("Hide");
            window = false;
        }
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    public void ShowSettings()
    {
        animator.SetTrigger("Show");
        window = true;
    }

    public void HideSettings()
    {
        animator.SetTrigger("Hide");
        window = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OnTextSpeedChanged(float value)
    {
        textSpeedValue.SetText(value + " ms");
    }

    public void OnAutoForwardChanged(float value)
    {
        autoForwardValue.SetText(value + " detik");
    }

    public void OnMusicChanged(float value)
    {
        musicValue.SetText(value + "%");
        musicMixer.SetFloat("volume", - 50 + value / 2);
    }

    public void OnSoundChanged(float value)
    {
        soundValue.SetText(value + "%");
        soundMixer.SetFloat("volume", -50 + value / 2);
    }
}
