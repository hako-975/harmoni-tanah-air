using System.Collections.Generic;
using UnityEngine;

public class HistoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject historyDialogBarPrefab;

    [SerializeField]
    private GameObject contentHistory;

    [SerializeField]
    private GameController gameController;

    void OnEnable()
    {
        foreach (Transform child in contentHistory.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (StoryScene scene in gameController.history)
        {
            List<StoryScene.Sentence> sentencesForHistory = new List<StoryScene.Sentence>();

            int maxIndex = Mathf.Min(scene.sentences.Count, gameController.dialogBar.GetSentenceIndex() + 1);

            for (int i = 0; i < maxIndex; i++)
            {
                sentencesForHistory.Add(scene.sentences[i]);
            }

            foreach (StoryScene.Sentence sentence in sentencesForHistory)
            {
                if (scene.sentences != sentencesForHistory)
                {
                    Speaker speaker = sentence.speaker;
                    string speakerName = speaker.speakerName;
                    Color speakerColor = speaker.textColor;

                    GameObject historyDialogBar = Instantiate(historyDialogBarPrefab, Vector3.zero, Quaternion.identity, contentHistory.transform);
                    var textbox = historyDialogBar.GetComponent<HistoryDialogBarController>().textBoxText;
                    textbox.text = sentence.text;

                    var namebox = historyDialogBar.GetComponent<HistoryDialogBarController>();

                    if (speakerName != "Narrator")
                    {
                        namebox.nameBoxUI.SetActive(true);
                        namebox.nameBoxText.text = speakerName;
                        namebox.nameBoxText.color = speakerColor;
                    }
                    else
                    {
                        namebox.nameBoxUI.SetActive(false);
                    }
                }
            }
        }

    }
}
