
using UnityEngine;
using UnityEngine.Audio;

public class PlayerPrefsController : MonoBehaviour
{
    #region Singleton
    public static PlayerPrefsController instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer soundMixer;

    void Start()
    {
        // load settings configuration on start
        musicMixer.SetFloat("volume", -50 + GetMusicVolume() / 2);
        soundMixer.SetFloat("volume", -50 + GetSoundVolume() / 2);
    }


    public int GetTextSpeed()
    {
        return PlayerPrefs.GetInt("TextSpeed", 50);
    }

    public void SetTextSpeed(int millisecond)
    {
        PlayerPrefs.SetInt("TextSpeed", millisecond);
    }

    public int GetAutoForward()
    {
        return PlayerPrefs.GetInt("AutoForward", 1);
    }

    public void SetAutoForward(int second)
    {
        PlayerPrefs.SetInt("AutoForward", second);
    }

    public int GetMusicVolume()
    {
        return PlayerPrefs.GetInt("MusicVolume", 100);
    }

    public void SetMusicVolume(int volume)
    {
        PlayerPrefs.SetInt("MusicVolume", volume);
    }

    public int GetSoundVolume()
    {
        return PlayerPrefs.GetInt("SoundVolume", 100);
    }

    public void SetSoundVolume(int volume)
    {
        PlayerPrefs.SetInt("SoundVolume", volume);
    }

    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
