using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GeneralButton : MonoBehaviour
{
    public ButtonFunctionType buttonType;

    [ShowIf("@buttonType == ButtonFunctionType.ChangeScene")]
    [SerializeField]
    string _destinationSceneName;

    public void ChangeDestinationSceneName(string destinationSceneName)
    {
        this._destinationSceneName = destinationSceneName;
    }

    public void OnClick()
    {
        switch(buttonType)
        {
            case ButtonFunctionType.None:
                break;
            case ButtonFunctionType.ChangeScene:
                GlobalSceneManager.Instance.ChangeScene(_destinationSceneName).Forget();
                break;
        }
    }
}

public enum ButtonFunctionType
{
    None,
    ChangeScene
}