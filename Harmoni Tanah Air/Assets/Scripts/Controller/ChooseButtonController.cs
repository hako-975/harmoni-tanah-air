using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButtonController : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private TextMeshProUGUI chooseLabelText;

    private StoryScene scene;

    private ChooseController controller;
    
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickChooseButton);
    }

    void OnClickChooseButton()
    {
        controller.PerformChoose(scene);
    }

    public float GetHeight()
    {
        return 15f + (rectTransform.sizeDelta.y * rectTransform.localScale.y);
    }

    public void Setup(ChooseScene.ChooseLabel label, ChooseController controller, float y)
    {
        scene = label.nextScene;
        chooseLabelText.text = label.text;
        this.controller = controller;

        Vector3 position = rectTransform.localPosition;
        position.y = y;
        rectTransform.localPosition = position;
    }
}
