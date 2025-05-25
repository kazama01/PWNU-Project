using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GosokDevManager : SceneManagerBase
{
    public static GosokDevManager Instance;

    [TitleGroup("Gosok Manager")]
    public ScratchDatabaseSO scratchDatabaseSO;

    public const string SELECTED_GOSOK_LETTER_KEY = "SelectedGosokLetter";

    [TitleGroup("Gosok Manager")]
    [ShowInInspector]
    [ReadOnly]
    public string SelectedGosokLetterString { get; private set; }

    [TitleGroup("Gosok Manager")]
    [SuffixLabel("%")]
    public float winCondition = 95;

    [TitleGroup("Gosok Manager")]
    public int koinReward;

    public ArabLetter SelectedGosokLetter
    {
        get
        {
            return GlobalUtility.GetArabLetterFromString(SelectedGosokLetterString);
        }
    }

    [TitleGroup("Gosok Manager")]
    [ShowInInspector]
    [ReadOnly]
    GosokUI _gosokUI;

    public void Awake()
    {
        Instance = this;
        LoadSelectedGosokLetter();
    }

    public void LoadSelectedGosokLetter()
    {
        SelectedGosokLetterString = PlayerPrefs.GetString(SELECTED_GOSOK_LETTER_KEY);
    }

    protected override void Start()
    {
        base.Start();

        _gosokUI = FindFirstObjectByType<GosokUI>();
        _gosokUI.Setup(scratchDatabaseSO.scratchDataDict[SelectedGosokLetter]);

        _gosokUI.scratcher.OnBlackWhiteRatioCalculated += BlackWhiteCalculatedHandler;
    }

    private void OnDestroy()
    {
        _gosokUI.scratcher.OnBlackWhiteRatioCalculated -= BlackWhiteCalculatedHandler;
    }

    public void LoadNext()
    {
        var allLetters = (ArabLetter[])System.Enum.GetValues(typeof(ArabLetter));
        ArabLetter currentLetter = SelectedGosokLetter;

        int currentIndex = System.Array.IndexOf(allLetters, currentLetter);
        int totalLetters = allLetters.Length;

        for (int i = 1; i <= totalLetters; i++) // loop through all letters at most once
        {
            int nextIndex = (currentIndex + i) % totalLetters;
            ArabLetter nextLetter = allLetters[nextIndex];

            if (scratchDatabaseSO.scratchDataDict.TryGetValue(nextLetter, out var scratchData) && scratchData != null)
            {
                SelectedGosokLetterString = nextLetter.ToString();
                PlayerPrefs.SetString(SELECTED_GOSOK_LETTER_KEY, SelectedGosokLetterString);
                PlayerPrefs.Save();

                _gosokUI.Setup(scratchData);
                return;
            }
        }

        Debug.LogWarning("No valid Gosok letter found with non-null data.");
    }

    void BlackWhiteCalculatedHandler(float whiteRatio)
    {
        if (whiteRatio >= winCondition)
        {
            PlayerDataManager.Instance.AddKoin(koinReward);
            LoadNext();
        }
    }
}
