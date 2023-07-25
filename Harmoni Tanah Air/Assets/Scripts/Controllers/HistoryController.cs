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
            for (int i = 0; i < gameController.dialogBar.GetSentenceIndex() + 1; i++)
            {
                Debug.Log("i: " + i);
                Debug.Log("dialogsentenceindex: " + gameController.dialogBar.GetSentenceIndex() + 1);
                StoryScene.Sentence sentence = scene.sentences[i];

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
