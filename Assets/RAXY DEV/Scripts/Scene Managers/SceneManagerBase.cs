using Cysharp.Threading.Tasks;
using MEC;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneManagerBase : MonoBehaviour
{
    [TitleGroup("General Scene Setting")]
    public bool useBackButton;

    [TitleGroup("General Scene Setting")]
    [ShowIf("@useBackButton")]
    public string backTargetSceneName;

    [TitleGroup("General Scene Setting")]
    [ShowIf("@useBackButton")]
    public Transform backButtonPosOverrider;

    [TitleGroup("General Scene Setting")]
    public bool showPlayerData;

    protected virtual void Start()
    {
        Debug.Log("Start Coroutine on MenuManager");
        Timing.RunCoroutine(StartCo().CancelWith(gameObject));
    }

    IEnumerator<float> StartCo()
    {
        float elapsed = 0;

        while (elapsed < 1)
        {
            SetBackButton();
            CallPlayerDataUI();

            elapsed += Time.deltaTime;

            yield return Timing.WaitForOneFrame;
        }
    }

    void SetBackButton()
    {
        Debug.Log("Try to show back button");

        BackManager.Instance?.ShowBackButton(useBackButton);
        if (useBackButton)
        {
            BackManager.Instance?.ChangeBackDestinationScene(backTargetSceneName);
        }

        if (backButtonPosOverrider)
        {
            BackManager.Instance?.ChangeBackButtonPos(backButtonPosOverrider);
        }
        else
        {
            BackManager.Instance?.ResetBackButtonPos();
        }
    }

    void CallPlayerDataUI()
    {
        Debug.Log("Try to show KOIN");
        PlayerDataManager.Instance?.ShowKoin(showPlayerData);
    }
}
