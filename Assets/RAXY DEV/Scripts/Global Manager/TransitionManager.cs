using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    public Transform transitionOverlay;
    public float inDuration = 0.25f;
    public Ease inEase = Ease.OutQuad;
    public float outDuration = 0.25f;
    public Ease outEase = Ease.OutQuad;

    CanvasGroup _canvasGroup;
    bool _isTransitionIn;
    bool _isTransitionOut;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _canvasGroup = transitionOverlay.GetComponent<CanvasGroup>();
    }

    public async UniTask In_Transition()
    {
        _isTransitionIn = true;
        _isTransitionOut = false;
        DOTween.To(() => _canvasGroup.alpha, 
                    (x) => _canvasGroup.alpha = x, 
                    1, inDuration).SetEase(inEase).OnComplete(() => _isTransitionIn = false);

        await UniTask.WaitUntil(() => _isTransitionIn == false);
    }

    public async UniTask Out_Transition()
    {
        _isTransitionIn = false;
        _isTransitionOut = true;
        DOTween.To(() => _canvasGroup.alpha,
                    (x) => _canvasGroup.alpha = x,
                    0, inDuration).SetEase(inEase).OnComplete(() => _isTransitionOut = false);

        await UniTask.WaitUntil(() => _isTransitionOut == false);
    }
}
