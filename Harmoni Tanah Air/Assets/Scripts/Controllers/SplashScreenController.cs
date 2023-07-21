using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField]
    private Image tapToStart;

    [SerializeField]
    private GameObject tapToStartButton;

    private void Awake()
    {
        tapToStart.raycastTarget = false;
    }

    private void Start()
    {
        StartCoroutine(WaitAnim());
    }
    
    public void TapToStartButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1f);
        tapToStart.GetComponent<Animator>().SetTrigger("White Screen");
        yield return new WaitForSeconds(1f);
        tapToStartButton.GetComponent<Animator>().SetTrigger("Tap To Start Fade");
        tapToStart.raycastTarget = true;
    }
}
