using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterManager : MonoBehaviour {

    private static MasterManager _instance;
    public static MasterManager instance { get { return _instance; } }

    float _sceneLoadProgress;
    public float sceneLoadProgress { get { return _sceneLoadProgress; } }

    AsyncOperation operation;
    public int initialScene;
    int loadScene = 1;

    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    void Start () {
        LoadScene(initialScene);
    }

    public void LoadScene(int sceneIndex)
    {
        //SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    public void LoadScene(int sceneIndex, int previousScene)
    {
        //SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneAsync(sceneIndex, previousScene));
    }

    IEnumerator LoadSceneAsync(int sceneIndex, int previousScene)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / 0.9f);
            _sceneLoadProgress = progress;
            yield return null;
        }

        //LoadingDone(loadScene);
        LoadingDone(previousScene);
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / 0.9f);
            _sceneLoadProgress = progress;
            yield return null;
        }

        //LoadingDone(loadScene);
    }

    void LoadingDone(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
}
