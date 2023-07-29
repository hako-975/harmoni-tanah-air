using System.Collections;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteSwitcherController switcher;

    private Animator animator;

    private RectTransform rect;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        switcher = GetComponent<SpriteSwitcherController>();
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
        switcher.SyncImages();
    }

    public void Show(Vector2 coords, bool isAnimated = true)
    {
        if (isAnimated)
        {
            animator.enabled = true;
            animator.SetTrigger("Show");
        }
        else
        {
            animator.enabled = false;
            canvasGroup.alpha = 1;
        }
        rect.localPosition = coords;
    }

    public void Hide(float duration, bool isAnimated = true)
    {
        if (isAnimated)
        {
            StartCoroutine(HideCoroutine(duration));
        }
        else
        {
            animator.enabled = false;
            canvasGroup.alpha = 0;
        }
    }

    private IEnumerator HideCoroutine(float duration)
    {
        animator.enabled = true;
        switcher.SyncImages();
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("Hide");
    }

    public void Move(Vector2 coords, float speed, bool isAnimated = true)
    {
        if (isAnimated)
        {
            StartCoroutine(MoveCoroutine(coords, speed));
        }
        else
        {
            rect.localPosition = coords;
        }    
    }

    private IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while (rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords, Time.deltaTime * 1000f * speed);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SwitchSprite(Vector2 coords, Sprite sprite, bool isAnimated = true)
    {
        if (switcher.GetImage() != sprite)
        {
            if (isAnimated)
            {
                switcher.SwitchImage(sprite);
            }
            else
            {
                switcher.SetImage(sprite);
            }
        }
        rect.localPosition = coords;
    }
}
