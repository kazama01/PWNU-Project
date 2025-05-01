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

    private void Start()
    {
        Timing.RunCoroutine(ForceSetBackButtonCo().CancelWith(gameObject));
    }

    IEnumerator<float> ForceSetBackButtonCo()
    {
        float elapsed = 0;

        while (elapsed < 1)
        {
            SetBackButton();
            elapsed += Time.deltaTime;

            yield return Timing.WaitForOneFrame;
        }
    }

    [Button]
    void SetBackButton()
    {
        BackManager.Instance.ShowBackButton(useBackButton);
        if (useBackButton)
        {
            BackManager.Instance.ChangeBackDestinationScene(backTargetSceneName);
        }

        if (backButtonPosOverrider)
        {
            BackManager.Instance.ChangeBackButtonPos(backButtonPosOverrider);
        }
        else
        {
            BackManager.Instance.ResetBackButtonPos();
        }
    }
}
