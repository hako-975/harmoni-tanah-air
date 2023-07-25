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


    }
}
