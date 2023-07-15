using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField]
    private Image tapToStart;

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
        SceneManager.LoadScene("Gameplay");
    }

    private IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(2f);
        tapToStart.GetComponent<Animator>().SetTrigger("White Screen");
        yield return new WaitForSeconds(1f);
        tapToStart.raycastTarget = true;
    }
}
