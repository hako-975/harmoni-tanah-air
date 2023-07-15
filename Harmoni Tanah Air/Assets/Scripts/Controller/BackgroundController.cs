using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private bool isSwitched = false;
    [SerializeField]
    private Image background1;
    [SerializeField]
    private Image background2;
    [SerializeField]
    private Animator animator;

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background2.sprite = sprite;
            animator.SetTrigger("Switch1to2");
        }
        else
        {
            background1.sprite = sprite;
            animator.SetTrigger("Switch2to1");
        }

        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background1.sprite = sprite;
        }
        else
        {
            background2.sprite = sprite;
        }
    }
}
