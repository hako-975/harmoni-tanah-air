using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsync(PlayerPrefsController.instance.GetNextScene()));
    }

    IEnumerator LoadAsync(string nextScene)
    {
        // set to main menu
        PlayerPrefsController.instance.DeleteKey("NextScene");

        AsyncOperation sync = SceneManager.LoadSceneAsync(nextScene);

        if (sync == null)
        {
            SceneManager.LoadScene("MainMenu");
        }
        yield return new WaitForSeconds(1f);
    }
}
