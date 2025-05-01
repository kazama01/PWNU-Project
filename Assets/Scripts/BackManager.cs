using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackManager : MonoBehaviour
{
    public static BackManager Instance;

    public GeneralButton backButton;

    [ReadOnly]
    public string targetSceneName;

    Vector3 _defaultPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _defaultPos = backButton.transform.position;
    }

    //private void OnEnable()
    //{
    //    SceneManager.activeSceneChanged += ActiveSceneChangedHandler;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.activeSceneChanged -= ActiveSceneChangedHandler;
    //}

    //private void ActiveSceneChangedHandler(Scene previousScene, Scene newScene)
    //{
    //    ChangeBackDestinationScene(previousScene.name);
    //}

    public void ChangeBackDestinationScene(string destinationScene)
    {
        targetSceneName = destinationScene;
        backButton.ChangeDestinationSceneName(destinationScene);
    }

    [Button]
    public void ChangeBackButtonPos(Vector3 pos)
    {
        backButton.transform.position = pos;
    }

    [Button]
    public void ChangeBackButtonPos(Transform pos)
    {
        backButton.transform.position = pos.position;
    }

    [Button]
    public void ResetBackButtonPos()
    {
        backButton.transform.position = _defaultPos;
    }

    [Button]
    public void ShowBackButton(bool show)
    {
        backButton?.gameObject?.SetActive(show);
    }
}
