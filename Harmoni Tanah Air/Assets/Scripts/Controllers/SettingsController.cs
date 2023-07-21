using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private Button resetButton;

    [Header("TMPro")]
    [SerializeField]
    private TextMeshProUGUI textSpeedValue;
    [SerializeField]
    private TextMeshProUGUI autoForwardValue;
    [SerializeField]
    private TextMeshProUGUI musicValue;
    [SerializeField]
    private TextMeshProUGUI soundValue;

    [Header("Slider")]
    [SerializeField]
    private Slider textSpeedSlider;
    [SerializeField]
    private Slider autoForwardSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundSlider;

    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer soundMixer;

    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(OnResetButtonClick);
    }

    void Update()
    {
        textSpeedSlider.value = PlayerPrefsController.instance.GetTextSpeed();
        textSpeedValue.SetText(PlayerPrefsController.instance.GetTextSpeed() + " ms");

        autoForwardSlider.value = PlayerPrefsController.instance.GetAutoForward();
        autoForwardValue.SetText(PlayerPrefsController.instance.GetAutoForward() + " detik");

        musicSlider.value = PlayerPrefsController.instance.GetMusicVolume();
        musicValue.SetText(PlayerPrefsController.instance.GetMusicVolume() + "%");

        soundSlider.value = PlayerPrefsController.instance.GetSoundVolume();
        soundValue.SetText(PlayerPrefsController.instance.GetSoundVolume() + "%");
    }

    private void OnResetButtonClick()
    {
        PlayerPrefsController.instance.DeleteKey("TextSpeed");
        PlayerPrefsController.instance.DeleteKey("AutoForward");
        PlayerPrefsController.instance.DeleteKey("MusicVolume");
        PlayerPrefsController.instance.DeleteKey("SoundVolume");
    }

    public void OnTextSpeedChanged(float value)
    {
        textSpeedValue.SetText(value + " ms");
        PlayerPrefsController.instance.SetTextSpeed((int)value);
    }

    public void OnAutoForwardChanged(float value)
    {
        autoForwardValue.SetText(value + " detik");
        PlayerPrefsController.instance.SetAutoForward((int)value);
    }

    public void OnMusicChanged(float value)
    {
        musicValue.SetText(value + "%");
        float calValue = -50 + value / 2;
        musicMixer.SetFloat("volume", calValue);
        PlayerPrefsController.instance.SetMusicVolume((int)value);
    }

    public void OnSoundChanged(float value)
    {
        soundValue.SetText(value + "%");
        float calValue = -50 + value / 2;
        soundMixer.SetFloat("volume", calValue);
        PlayerPrefsController.instance.SetSoundVolume((int)value);
    }
}
