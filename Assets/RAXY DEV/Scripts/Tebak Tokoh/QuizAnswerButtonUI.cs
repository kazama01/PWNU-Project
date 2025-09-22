using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizAnswerButtonUI : MonoBehaviour
{
    public TextMeshProUGUI answerLabelTxt;
    public Image answerImage;
    public TextMeshProUGUI answerTxt;

    public List<Color> randomAnswerLabelColors;

    [ShowInInspector]
    QuizAnswerData _answerData;
    [ShowInInspector]
    int _answerDataIndex;

    [Button]
    public void Setup(QuizAnswerData data, int answerIndex)
    {
        _answerData = data;
        answerLabelTxt.text = ((char)('A' + answerIndex)) + "";
        int rand = Random.Range(0, randomAnswerLabelColors.Count);
        answerLabelTxt.color = randomAnswerLabelColors[rand];

        answerImage.gameObject.SetActive(data.useImage);
        answerTxt.transform.parent.gameObject.SetActive(data.useText);

        answerImage.sprite = data.imageAnswer;

        answerTxt.text = data.textAnswer;
        answerTxt.fontSize = data.fontSize;
        answerTxt.color = data.fontColor;

        gameObject.name = data.answerId;

        _answerDataIndex = answerIndex;
    }

    public void OnClick()
    {
        TebakTokohManager.Instance.TryAnswer(_answerDataIndex);
    }
}
