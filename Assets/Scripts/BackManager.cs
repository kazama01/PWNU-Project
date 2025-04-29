using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackManager : MonoBehaviour
{
    public GeneralButton backButton;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += ActiveSceneChangedHandler;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ActiveSceneChangedHandler;
    }

    private void ActiveSceneChangedHandler(Scene previousScene, Scene newScene)
    {
        ChangeBackDestinationScene(previousScene.name);
    }

    public void ChangeBackDestinationScene(string destinationScene)
    {
        backButton.ChangeDestinationSceneName(destinationScene);
    }
}
