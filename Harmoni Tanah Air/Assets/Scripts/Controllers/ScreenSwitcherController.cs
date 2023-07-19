using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenSwitcherController : MonoBehaviour
{
    [SerializeField]
    private string sceneToUnload;
    [SerializeField]
    private string loaderScene;
    [SerializeField]
    private string sceneToLoad;

    void Start()
    {
        UnloadStartScene();
    }

    private void UnloadStartScene()
    {
        SceneManager.sceneUnloaded += OnStartUnloaded;
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    private void OnStartUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnStartUnloaded;
        SceneManager.sceneLoaded += OnEndLoaded;
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }

    private void OnEndLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnEndLoaded;
        UnloadLoader();
    }

    private void UnloadLoader()
    {
        SceneManager.UnloadSceneAsync(loaderScene);
    }
}
