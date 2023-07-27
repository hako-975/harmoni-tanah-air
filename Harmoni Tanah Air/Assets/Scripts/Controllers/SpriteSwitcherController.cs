using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcherController : MonoBehaviour
{
    private bool isSwitched = false;

    [SerializeField]
    private Image image1;
    [SerializeField]
    private Image image2;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            image2.sprite = sprite;
            animator.SetTrigger("Switch1to2");
        }
        else
        {
            image1.sprite = sprite;
            animator.SetTrigger("Switch2to1");
        }

        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            image1.sprite = sprite;
        }
        else
        {
            image2.sprite = sprite;
        }
    }

    public void SyncImages()
    {
        if (!isSwitched)
        {
            image2.sprite = image1.sprite;
        }
        else
        {
            image1.sprite = image2.sprite;
        }
    }

    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }
}
