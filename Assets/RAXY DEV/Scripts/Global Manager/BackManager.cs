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
        
        _defaultPos = backButton.transform.position;
    }

    // private void Start()
    // {
    //     _defaultPos = backButton.transform.position;
    // }

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
        if (backButton == null)
        {
            Debug.LogWarning("[UI] BackButton reference is null — cannot change position.");
            return;
        }

        if (pos == null)
        {
            Debug.LogWarning("[UI] Provided Transform is null — cannot change back button position.");
            return;
        }

        backButton.transform.position = pos.position;
        Debug.Log($"[UI] BackButton position changed to {pos.position}");
    }

    [Button]
    public void ResetBackButtonPos()
    {
        if (backButton == null)
        {
            Debug.LogWarning("[UI] BackButton reference is null — cannot reset position.");
            return;
        }

        backButton.transform.position = _defaultPos;
        Debug.Log($"[UI] BackButton position reset to default at {_defaultPos}");
    }

    [Button]
    public void ShowBackButton(bool show)
    {
        if (backButton == null)
        {
            Debug.LogWarning($"[UI] BackButton reference is null — cannot set active to {show}");
            return;
        }

        backButton.gameObject.SetActive(show);
        Debug.Log($"[UI] BackButton set active = {show}");
    }
}
