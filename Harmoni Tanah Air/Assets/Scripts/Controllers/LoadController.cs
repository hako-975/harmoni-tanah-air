using UnityEngine;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Button loadButton1;
    public Button loadButton2;
    public Button loadButton3;

    public GameObject confirmLoadPanel;
    public Button confirmLoadYesButton;
    public Button confirmLoadNoButton;

    [SerializeField]
    GameController gameController;

    void Update()
    {
        // load data 1 for showing data in load panel
        gameController.GetDataLoadButton(1);
        // load data 2 for showing data in load panel
        gameController.GetDataLoadButton(2);
        // load data 3 for showing data in load panel
        gameController.GetDataLoadButton(3);
    }
}
