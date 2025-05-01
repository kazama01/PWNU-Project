using Sirenix.OdinInspector;
using UnityEngine;

public class SceneManagerBase : MonoBehaviour
{
    [TitleGroup("General Scene Setting")]
    public bool useBackButton;

    [TitleGroup("General Scene Setting")]
    [ShowIf("@showBackButton")]
    public string backTargetSceneName;

    [TitleGroup("General Scene Setting")]
    [ShowIf("@showBackButton")]
    public Transform backButtonPosOverrider;
}
