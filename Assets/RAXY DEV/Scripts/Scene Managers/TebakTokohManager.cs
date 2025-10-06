using Cysharp.Threading.Tasks;
using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class TebakTokohManager : SceneManagerBase
{
    public static TebakTokohManager Instance;

    [TitleGroup("Tebak Tokoh")]
    public TextMeshProUGUI questionTxt;

    [TitleGroup("Tebak Tokoh")]
    public QuizAnswerButtonUI answerPrefab;

    [TitleGroup("Tebak Tokoh")]
    public QuizQuestionDatabaseSO questionDb;

    [TitleGroup("Tebak Tokoh")]
    public Transform left;

    [TitleGroup("Tebak Tokoh")]
    public Transform right;

    [TitleGroup("Result")]
    public GameObject resultPanel;

    [TitleGroup("Result")]
    public GameObject correct;

    [TitleGroup("Result")]
    public GameObject wrong;

    [TitleGroup("Result")]
    public AudioResource correctClip;

    [TitleGroup("Result")]
    public AudioResource wrongClip;

    [TitleGroup("Debug"), ShowInInspector, ReadOnly]
    private QuizQuestionSO currentQuestion;

    // Cache the current answers so we don’t call GetAnswers() multiple times
    private List<QuizAnswerData> currentAnswers;

    AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Duplicate {nameof(TebakTokohManager)} detected, destroying one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        SetupQuestionAndAnswer();
    }

    [Button]
    public void SetupQuestionAndAnswer()
    {
        SelectQuestion();

        questionTxt.text = currentQuestion.textQuestion;

        // Safety: no prefab or no question DB
        if (answerPrefab == null)
        {
            Debug.LogError("AnswerPrefab is missing!");
            return;
        }

        if (currentQuestion == null)
        {
            Debug.LogWarning("No current question selected.");
            return;
        }

        ClearChildren(left);
        ClearChildren(right);

        // Get the answer set ONCE and cache it
        currentAnswers = currentQuestion.GetAnswers();

        if (currentAnswers == null || currentAnswers.Count == 0)
        {
            Debug.LogWarning("No answers available for current question.");
            return;
        }

        // Spawn answers
        for (int i = 0; i < currentAnswers.Count; i++)
        {
            var answerData = currentAnswers[i];
            if (answerData == null)
                continue;

            var answerClone = Instantiate(answerPrefab,
                (i % 2 == 0 ? left : right),   // parent on spawn
                false);                        // keep local transform

            answerClone.Setup(answerData, i);
        }
    }

    private void ClearChildren(Transform parent)
    {
        if (parent == null) return;

        // Editor-safe destroy
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
#endif
                Destroy(child.gameObject);
        }
    }

    private void SelectQuestion()
    {
        if (questionDb == null)
        {
            Debug.LogError("Question Database not assigned!");
            currentQuestion = null;
            return;
        }

        currentQuestion = questionDb.GetRandomQuestion();
    }

    public bool TryAnswer(int answerIndex)
    {
        if (currentAnswers == null || currentAnswers.Count == 0)
        {
            Debug.LogWarning("No answers to validate.");
            return false;
        }

        if (answerIndex < 0 || answerIndex >= currentAnswers.Count)
        {
            Debug.LogError($"Answer index {answerIndex} is out of range!");
            return false;
        }

        var chosen = currentAnswers[answerIndex];

        if (chosen == null)
        {
            Debug.LogWarning("Selected answer data is null.");
            return false;
        }

        bool isCorrect = currentQuestion.CorrectAnswerId == chosen.answerId; // <- assuming your QuizAnswerData has this flag
        HandleResultCoHandle = Timing.RunCoroutine(HandleResultCo(isCorrect));

        if (isCorrect)
        {
            Debug.Log("Correct Answer!");
            PlayerDataManager.Instance?.AddKoin(10);
        }
        else
        {
            Debug.Log("Wrong Answer!");
        }

        return isCorrect;
    }

    public void OnClick_Next()
    {
        Timing.KillCoroutines(HandleResultCoHandle);
        SetupQuestionAndAnswer();
        resultPanel.SetActive(false);

        _audioSource.Stop();
    }

    CoroutineHandle HandleResultCoHandle;
    IEnumerator<float> HandleResultCo(bool isCorrect)
    {
        _audioSource.Stop();

        resultPanel.SetActive(true);
        if (isCorrect)
        {
            correct.SetActive(true);
            wrong.SetActive(false);

            _audioSource.resource = correctClip;
            _audioSource.Play();
        }
        else
        {
            correct.SetActive(false);
            wrong.SetActive(true);

            _audioSource.resource = wrongClip;
            _audioSource.Play();
        }

        yield return Timing.WaitForSeconds(2f);
        resultPanel.SetActive(false);
        SetupQuestionAndAnswer();
    }

    public void OnClick_SuaraBtn()
    {
        _audioSource.PlayOneShot(currentQuestion.questionNarrationClip);
    }
}
