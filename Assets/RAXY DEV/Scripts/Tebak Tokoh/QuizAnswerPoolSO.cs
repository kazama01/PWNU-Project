using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizAnswerPoolSO", menuName = "Scriptable Objects/QuizAnswerPoolSO")]
public class QuizAnswerPoolSO : ScriptableObject
{
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "answerId")]
    public List<QuizAnswerData> answerDatas;
}

[Serializable]
public class QuizAnswerData
{
    public string answerId;

    public bool useText = true;
    public bool useImage = false;

    [ShowIf("@useText")]
    public string textAnswer;
    [ShowIf("@useText")]
    public float fontSize = 90;
    [ShowIf("@useText")]
    public Color fontColor = Color.black;

    [ShowIf("@useImage")]
    public Sprite imageAnswer;
}


