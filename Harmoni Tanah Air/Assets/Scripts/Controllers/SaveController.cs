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

    [SerializeField]
    GameController gameController;


    void OnEnable()
    {
        // load data 1 for showing data in save panel
        gameController.GetDataSaveButton(1);
        // load data 2 for showing data in save panel
        gameController.GetDataSaveButton(2);
        // load data 3 for showing data in save panel
        gameController.GetDataSaveButton(3);
    }
}
