using UnityEngine;

public class ChooseController : MonoBehaviour
{
    [SerializeField]
    private ChooseButtonController chooseButtonPrefabs;
    [SerializeField]
    private GameController gameController;

    private Animator animator;
    
    private float labelHeight = -1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetupChoose(ChooseScene scene)
    {
        DestroyLabels();
        animator.SetTrigger("Show");
        for (int i = 0; i < scene.labels.Count; i++)
        {
            ChooseButtonController newChooseButton = Instantiate(chooseButtonPrefabs.gameObject, transform).GetComponent<ChooseButtonController>();

            if (labelHeight == -1)
            {
                labelHeight = newChooseButton.GetHeight();
                Debug.Log(labelHeight);
            }
            newChooseButton.Setup(scene.labels[i], this, CalculateLabelPosition(i, scene.labels.Count));
        }
    }

    public void PerformChoose(StoryScene scene)
    {
        gameController.PlayScene(scene);
        animator.SetTrigger("Hide");
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        if (labelCount % 2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount / 2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return 0;
            }
        }
    }

    private void DestroyLabels()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}
