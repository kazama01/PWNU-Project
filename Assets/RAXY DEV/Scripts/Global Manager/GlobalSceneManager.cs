using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public async UniTask ChangeScene(string sceneName)
    {
        var loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        await TransitionManager.Instance.In_Transition();

        // Wait until the scene is done loading (but not activated)
        while (loadOp.progress < 0.9f)
        {
            await UniTask.Yield();
        }

        loadOp.allowSceneActivation = true;

        // Optionally wait one frame to ensure the scene is active
        await UniTask.NextFrame();

        await TransitionManager.Instance.Out_Transition();
    }
}
