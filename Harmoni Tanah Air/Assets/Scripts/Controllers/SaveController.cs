using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveController : MonoBehaviour
{
    public Button saveButton1;
    public Button saveButton2;
    public Button saveButton3;

    public GameObject confirmSavePanel;
    public Button confirmSaveYesButton;
    public Button confirmSaveNoButton;

    private static readonly string SAVED_GAME = "SavedGame";

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
}
