using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TebakTokohManager : SceneManagerBase
{
    public static TebakTokohManager Instance;

    [Title("Tebak Tokoh")]
    public MuslimHeroDatabaseSO MuslimHeroDatabaseSO;

    public string questionString = "Yang manakah gambar dari [A]?";
    [TextArea(3, 5)]
    public string correctAnswerString = "Ya Benar!!!\n\nIni adalah gambar dari [A]";
    [TextArea(3, 5)]
    public string wrongAnswerString = "Ini bukan gambar dari [A]\n\nCoba diperhatikan lagi!!!";
    public TextMeshProUGUI questionText;
    public GameObject answerButtonPrefab;
    public Transform answerParent;
    public int answerCount = 4;

    public int koinReward;

    [Title("Notification Requests")]
    public NotificationRequest correctReq;
    public NotificationRequest wrongReq;

    [FoldoutGroup("Runtime")]
    [ShowInInspector]
    [ReadOnly]
    MuslimHeroDataSO _selectedCorrectSO;

    [FoldoutGroup("Runtime")]
    [ShowInInspector]
    [ReadOnly]
    int _randomCorrectIndex;

    [FoldoutGroup("Runtime")]
    [ShowInInspector]
    [ReadOnly]
    List<MuslimHeroDataSO> _selectedWrongSoList;

    [FoldoutGroup("Runtime")]
    [ShowInInspector]
    [ReadOnly]
    List<GameObject> _spawnedAnswer;

    [FoldoutGroup("Runtime")]
    [ShowInInspector]
    [ReadOnly]
    NotificationRequest _currentNotifReq;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        ShowQuestion();

        NotificationManager.OnNotificationClosed += NotificationClosedHandler;
    }

    private void OnDestroy()
    {
        NotificationManager.OnNotificationClosed -= NotificationClosedHandler;
    }

    void NotificationClosedHandler(NotificationRequest request)
    {
        if (request.RequestId == _currentNotifReq.RequestId)
        {
            ShowQuestion();
        }
    }

    [Button]
    void ShowQuestion()
    {
        _selectedCorrectSO = MuslimHeroDatabaseSO.GetRandomHero();
        _randomCorrectIndex = Random.Range(0, answerCount);

        // Get the wrong answers, excluding the correct one
        _selectedWrongSoList = MuslimHeroDatabaseSO.GetRandomHeroes(
            answerCount - 1,
            new List<MuslimHeroDataSO> { _selectedCorrectSO }
        );

        questionText.text = questionString.Replace("[A]", _selectedCorrectSO.NotificationRequest.headerMessage);

        // Clear previous answers
        foreach (Transform child in answerParent)
        {
            Destroy(child.gameObject);
        }

        _spawnedAnswer = new List<GameObject>();

        int wrongIndex = 0;

        for (int i = 0; i < answerCount; i++)
        {
            GameObject clone = Instantiate(answerButtonPrefab, answerParent);
            _spawnedAnswer.Add(clone);

            var buttonUI = clone.GetComponent<TebakTokohAnswerButtonUI>();

            if (i == _randomCorrectIndex)
            {
                buttonUI.Setup(_selectedCorrectSO, i);
                clone.name = _selectedCorrectSO.heroName + " - Answer Button";
            }
            else
            {
                buttonUI.Setup(_selectedWrongSoList[wrongIndex], i);
                clone.name = _selectedWrongSoList[wrongIndex].heroName + " - Answer Button";

                wrongIndex++;
            }
        }
    }

    public void TryAnswer(MuslimHeroDataSO answer)
    {
        if (answer == _selectedCorrectSO)
        {
            Correct();
        }
        else
        {
            Wrong(answer);
        }
    }

    void Correct()
    {
        correctReq.imageSprite = _selectedCorrectSO.NotificationRequest.imageSprite;
        correctReq.message = correctAnswerString.Replace("[A]", _selectedCorrectSO.heroName);

        NotificationManager.Instance.RequestNotification(correctReq);
        _currentNotifReq = correctReq;

        PlayerDataManager.Instance.AddKoin(koinReward);
    }

    void Wrong(MuslimHeroDataSO wrongDataSO)
    {
        wrongReq.imageSprite = wrongDataSO.NotificationRequest.imageSprite;
        wrongReq.message = wrongAnswerString.Replace("[A]", _selectedCorrectSO.heroName);

        NotificationManager.Instance.RequestNotification(wrongReq);
        _currentNotifReq = wrongReq;
    }
}
