using Cysharp.Threading.Tasks;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GosokDevManager : SceneManagerBase
{
    public const string SELECTED_GOSOK_LETTER_KEY = "SelectedGosokLetter";
    public static GosokDevManager Instance;

    [TitleGroup("Gosok Manager")]
    public ScratchDatabaseSO scratchDatabaseSO;

    [TitleGroup("Settings")]
    [SuffixLabel("%")]
    public float winCondition = 95;

    [TitleGroup("Settings")]
    public int koinReward;

    [TitleGroup("Settings")]
    [SuffixLabel("seconds")]
    public float delayForConsecutiveAudio = 1;

    [TitleGroup("Settings")]
    [SuffixLabel("seconds")]
    public float delayForLoadNext = 2;

    [TitleGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public string SelectedGosokLetterString { get; private set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public ArabLetter SelectedGosokLetter
    {
        get
        {
            return GlobalUtility.GetArabLetterFromString(SelectedGosokLetterString);
        }
    }

    [TitleGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    ScratchObjectAudio _selectedGosokObject;

    [TitleGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    GosokUI _gosokUI;

    public AudioSource AudioSource { get; private set; }

    public void Awake()
    {
        Instance = this;
        LoadSelectedGosokLetter();

        AudioSource = GetComponent<AudioSource>();
    }

    public void LoadSelectedGosokLetter()
    {
        SelectedGosokLetterString = PlayerPrefs.GetString(SELECTED_GOSOK_LETTER_KEY);
    }

    protected override void Start()
    {
        base.Start();

        _gosokUI = FindFirstObjectByType<GosokUI>();

        var scratchData = scratchDatabaseSO.scratchDataDict[SelectedGosokLetter];
        _selectedGosokObject = scratchData.GetRandomObject();
        _gosokUI.Setup(scratchData, _selectedGosokObject);

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

                _selectedGosokObject = scratchData.GetRandomObject();
                _gosokUI.Setup(scratchData, _selectedGosokObject);
                return;
            }
        }

        Debug.LogWarning("No valid Gosok letter found with non-null data.");
    }

    void BlackWhiteCalculatedHandler(float whiteRatio)
    {
        if (whiteRatio >= winCondition)
        {
            PlayerDataManager.Instance?.AddKoin(koinReward);

            _gosokUI.scratcher.enabled = false;
            _gosokUI.scratcher.transform.
                DOPunchScale(Vector3.one * 0.1f, 0.5f, 10, 1).
                SetEase(Ease.OutElastic);
            
            Timing.RunCoroutine(PlayAudioConsecutively(_selectedGosokObject.audioClips, LoadNext));
        }
    }

    public IEnumerator<float> PlayAudioConsecutively(List<AudioClip> audioClips, Action OnDone)
    {
        foreach (var clip in audioClips)
        {
            AudioSource.PlayOneShot(clip);
            yield return Timing.WaitForSeconds(clip.length + delayForConsecutiveAudio);
        }

        yield return Timing.WaitForSeconds(delayForLoadNext);

        OnDone?.Invoke();
    }
}
