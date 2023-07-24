
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    public static readonly string SAVED_GAME = "SavedGame";
    
    void Start()
    {
        // load settings configuration on start
        musicMixer.SetFloat("volume", -50 + GetMusicVolume() / 2);
        soundMixer.SetFloat("volume", -50 + GetSoundVolume() / 2);
    }

    public void SaveGame(int slot, SaveData data)
    {
        PlayerPrefs.SetString(SAVED_GAME + slot, JsonUtility.ToJson(data));
    }

    public SaveData LoadGame(int slot)
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVED_GAME + slot));
    }

    public bool IsGameSaved(int slot)
    {
        return PlayerPrefs.HasKey(SAVED_GAME + slot);
    }

    public void ClearSavedGame(int slot)
    {
        PlayerPrefs.DeleteKey(SAVED_GAME + slot);
    }

    public string GetNextScene()
    {
        return PlayerPrefs.GetString("NextScene", "MainMenu");
    }

    public void SetNextScene(string nameScene)
    {
        Time.timeScale = 1;

        PlayerPrefs.SetString("NextScene", nameScene);
        SceneManager.LoadScene("LoadingScreen");
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
        return PlayerPrefs.GetInt("AutoForward", 3);
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

    public int GetSlotSceneLoadGame()
    {
        return PlayerPrefs.GetInt("SlotSceneLoadGame");
    }

    public void SetSlotSceneLoadGame(int slot)
    {
        PlayerPrefs.SetInt("SlotSceneLoadGame", slot);
    }

    public bool IsHasSlotSceneLoadGame()
    {
        return PlayerPrefs.HasKey("SlotSceneLoadGame");
    }

    public bool IsHasGameSaved()
    {
        return PlayerPrefs.HasKey(SAVED_GAME + 1) || PlayerPrefs.HasKey(SAVED_GAME + 2) || PlayerPrefs.HasKey(SAVED_GAME + 3);
    }

    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
