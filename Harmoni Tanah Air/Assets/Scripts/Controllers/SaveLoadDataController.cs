using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SaveLoadDataController : MonoBehaviour
{
    public DataHolder dataHolder;

    #region SAVE
    public void SaveData(int slot)
    {
        List<int> historyIndicies = new List<int>();
        history.ForEach(scene =>
        {
            historyIndicies.Add(dataHolder.scenes.IndexOf(scene));
        });

        DateTime currentDateTime = DateTime.Now;
        SaveData data = new SaveData
        {
            sentence = dialogBar.GetSentenceIndex(),
            prevScenes = historyIndicies,
            dateSaved = currentDateTime.ToString("dddd, dd MMMM yyyy, HH:mm", new CultureInfo("id-ID"))
        };

        PlayerPrefsController.instance.SaveGame(slot, data);

        GetDataSaveButton(slot);
        CloseConfirmSavePanel();
    }
    
    public void OnSaveButtonClick(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            saveController.confirmSavePanel.SetActive(true);
            saveController.confirmSaveYesButton.onClick.AddListener(delegate { SaveData(slot); });
            saveController.confirmSaveNoButton.onClick.AddListener(CloseConfirmSavePanel);
        }
        else
        {
            SaveData(slot);
        }
    }

    public void GetDataSaveButton(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            SaveData data = PlayerPrefsController.instance.LoadGame(slot);
            data.prevScenes.ForEach(scene =>
            {
                storySceneSaveData.Add(dataHolder.scenes[scene] as StoryScene);
            });

            currentSceneSaveData = storySceneSaveData[storySceneSaveData.Count - 1];
            switch (slot)
            {
                case 1:
                    saveController.saveButton1.GetComponent<Image>().color = Color.white;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 2:
                    saveController.saveButton2.GetComponent<Image>().color = Color.white;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 3:
                    saveController.saveButton3.GetComponent<Image>().color = Color.white;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    saveController.saveButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    saveController.saveButton3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
            }
        }
    }
    #endregion

    #region LOAD
    private void LoadData(int slot)
    {
        PlayerPrefsController.instance.SetSlotSceneLoadGame(slot);
        PlayerPrefsController.instance.SetNextScene("Gameplay");
    }

    public void OnLoadButtonClick(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            loadController.confirmLoadPanel.SetActive(true);
            loadController.confirmLoadYesButton.onClick.AddListener(delegate { LoadData(slot); });
            loadController.confirmLoadNoButton.onClick.AddListener(CloseConfirmLoadPanel);
        }
    }

    

    public void GetDataLoadButton(int slot)
    {
        if (PlayerPrefsController.instance.IsGameSaved(slot))
        {
            SaveData data = PlayerPrefsController.instance.LoadGame(slot);
            data.prevScenes.ForEach(scene =>
            {
                storySceneSaveData.Add(dataHolder.scenes[scene] as StoryScene);
            });

            currentSceneSaveData = storySceneSaveData[storySceneSaveData.Count - 1];
            switch (slot)
            {
                case 1:
                    loadController.loadButton1.GetComponent<Image>().color = Color.white;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 2:
                    loadController.loadButton2.GetComponent<Image>().color = Color.white;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
                case 3:
                    loadController.loadButton3.GetComponent<Image>().color = Color.white;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 24;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
                    loadController.loadButton3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSceneSaveData as StoryScene).sentences[data.sentence].text;
                    loadController.loadButton3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.dateSaved;
                    break;
            }
        }
    }
    #endregion
}
