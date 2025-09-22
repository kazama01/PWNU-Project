using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizQuestionDatabaseSO", menuName = "Scriptable Objects/QuizQuestionDatabaseSO")]
public class QuizQuestionDatabaseSO : ScriptableObject
{
    public List<QuizQuestionSO> quizQuestions;

    [Button]
    public QuizQuestionSO GetRandomQuestion()
    {
        if (quizQuestions == null || quizQuestions.Count == 0)
        {
            Debug.LogWarning($"{name}: No quiz questions in database.");
            return null;
        }

        int index = Random.Range(0, quizQuestions.Count);
        return quizQuestions[index];
    }
}
