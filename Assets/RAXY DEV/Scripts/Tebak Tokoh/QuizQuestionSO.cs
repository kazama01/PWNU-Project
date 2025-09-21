using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizQuestionSO", menuName = "Scriptable Objects/QuizQuestionSO")]
public class QuizQuestionSO : ScriptableObject
{
    [TitleGroup("Question")]
    public bool useText = true;

    [TitleGroup("Question")]
    public bool useImage = false;

    [TitleGroup("Question")]
    [ShowIf("@useText")]
    [TextArea(3, 5)]
    public string textQuestion;

    [TitleGroup("Question")]
    [ShowIf("@useImage")]
    public Sprite imageQuestion;

    [TitleGroup("Answer Pool")]
    public bool usePoolSO;

    [TitleGroup("Answer Pool")]
    [ShowIf("@!usePoolSO")]
    [SerializeField]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "answerId")]
    private List<QuizAnswerData> _answerDatas;

    [TitleGroup("Answer Pool")]
    [ShowIf("@usePoolSO")]
    [SerializeField]
    private QuizAnswerPoolSO _answerPool;

    [TitleGroup("Debug")]
    [ShowInInspector, ReadOnly]
    public List<QuizAnswerData> AnswerDatas
    {
        get
        {
            if (usePoolSO)
                return _answerPool != null ? _answerPool.answerDatas : null;
            else
                return _answerDatas;
        }
    }

    private int AnswerDatasLastIndex => AnswerDatas?.Count - 1 ?? 0;
    private int AnswerDatasCount => AnswerDatas?.Count ?? 0;

    [TitleGroup("Setting")]
    [PropertyRange(0, "AnswerDatasLastIndex")]
    public int correctAnswerIndex;

    [TitleGroup("Setting")]
    [PropertyRange(2, "AnswerDatasCount")]
    public int answerCount = 4;

    [TitleGroup("Debug")]
    [ShowInInspector, ReadOnly]
    public string CorrectAnswerId
    {
        get
        {
            if (AnswerDatas == null || AnswerDatas.Count == 0)
                return "<No Answers>";

            if (correctAnswerIndex < 0 || correctAnswerIndex >= AnswerDatas.Count)
                return "<Index Out of Range>";

            return AnswerDatas[correctAnswerIndex]?.answerId ?? "<Null Answer>";
        }
    }

    [Button]
    public List<QuizAnswerData> GetAnswers()
    {
        if (AnswerDatas == null || AnswerDatas.Count == 0)
        {
            Debug.LogWarning($"{name}: No answers available.");
            return new List<QuizAnswerData>();
        }

        if (correctAnswerIndex < 0 || correctAnswerIndex >= AnswerDatas.Count)
        {
            Debug.LogWarning($"{name}: Correct answer index out of range.");
            return new List<QuizAnswerData>();
        }

        var correctAnswer = AnswerDatas[correctAnswerIndex];
        if (correctAnswer == null)
        {
            Debug.LogWarning($"{name}: Correct answer is null.");
            return new List<QuizAnswerData>();
        }

        // All wrong answers
        var wrongAnswers = AnswerDatas.Where((a, i) => i != correctAnswerIndex && a != null).ToList();

        // Clamp answerCount so it’s always valid
        int count = Mathf.Clamp(answerCount, 2, AnswerDatas.Count);

        // Pick random wrongs
        var selectedWrongs = new List<QuizAnswerData>();
        for (int i = 0; i < count - 1 && wrongAnswers.Count > 0; i++)
        {
            int rand = Random.Range(0, wrongAnswers.Count);
            selectedWrongs.Add(wrongAnswers[rand]);
            wrongAnswers.RemoveAt(rand);
        }

        // Final answers
        var finalList = new List<QuizAnswerData> { correctAnswer };
        finalList.AddRange(selectedWrongs);

        // Shuffle so correct isn’t always on top
        finalList.Shuffle();

        return finalList;
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
