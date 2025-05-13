using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TebakTokohAnswerButtonUI : MonoBehaviour
{
    public TextMeshProUGUI answerLabelTxt;
    public Image answerImage;

    [SerializeField]
    [ReadOnly]
    MuslimHeroDataSO _dataSO;

    public void Setup(MuslimHeroDataSO dataSO, int answerIndex)
    {
        _dataSO = dataSO;
        answerLabelTxt.text = ((char)('A' + answerIndex)) + ".";

        answerImage.sprite = _dataSO.NotificationRequest.imageSprite;
    }

    public void OnClick()
    {
        TebakTokohManager.Instance.TryAnswer(_dataSO);
    }
}
