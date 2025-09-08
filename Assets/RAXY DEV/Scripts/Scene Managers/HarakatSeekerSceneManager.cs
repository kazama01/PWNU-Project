using HijaiyahAudio;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarakatSeekerSceneManager : SceneManagerBase
{
    [TitleGroup("References")]
    [Tooltip("Audio data for each letter")]
    public HarokatSeekerDatabaseSO database;

    [TitleGroup("References")]
    public List<HijaiyahHarokatUI> harakatButtons;

    [TitleGroup("Debug")]
    // Current index in the letter list
    [ShowInInspector, ReadOnly]
    private int currentLetterIndex = 0;

    [TitleGroup("Debug")]
    [ShowInInspector, ReadOnly]
    public HijaiyahAudioDataSO CurrentLetterSO
    {
        get
        {
            if (_buildedAudioData == null || _buildedAudioData.Count == 0)
                return null;

            if (currentLetterIndex < 0 || currentLetterIndex >= _buildedAudioData.Count)
                return null;

            return _buildedAudioData[currentLetterIndex];
        }
    }


    [TitleGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    [ListDrawerSettings(ShowIndexLabels = true)]
    List<HijaiyahAudioDataSO> _buildedAudioData = new();

    public static HarakatSeekerSceneManager Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        _buildedAudioData.Clear();
        foreach (var letterData in database.harakatDataDict.Values)
        {
            _buildedAudioData.Add(letterData);
            _buildedAudioData.Add(letterData);
        }

        // Initialize the audio manager and register all audio data
        InitializeAudioManager();

        // Show the first letter
        ShowCurrentLetter();
    }

    private void InitializeAudioManager()
    {
        // Access the singleton instance to force initialization if needed
        var audioManager = HijaiyahAudioManager.Instance;

        // Add all audio data to the manager
        if (database.harakatDataDict != null)
        {
            foreach (var audioData in database.harakatDataDict.Values)
            {
                if (audioData != null)
                {
                    audioManager.AddAudioData(audioData);
                }
            }
        }
    }

    [TitleGroup("Debug Functions")]
    [Button]
    public void ShowNextLetter()
    {
        if (_buildedAudioData != null && _buildedAudioData.Count > 0)
        {
            currentLetterIndex = (currentLetterIndex + 1) % _buildedAudioData.Count;
            ShowCurrentLetter();
        }
    }

    [TitleGroup("Debug Functions")]
    [Button]
    public void ShowPreviousLetter()
    {
        if (_buildedAudioData != null && _buildedAudioData.Count > 0)
        {
            currentLetterIndex = (currentLetterIndex - 1 + _buildedAudioData.Count) % _buildedAudioData.Count;
            ShowCurrentLetter();
        }
    }

    [TitleGroup("Debug Functions")]
    [Button]
    private void ShowCurrentLetter()
    {
        if (_buildedAudioData == null || _buildedAudioData.Count == 0)
        {
            Debug.LogError("No letter data available in allLetterAudioDataList. Please assign HijaiyahAudioDataSO objects to the list.");
            return;
        }

        if (currentLetterIndex < 0 || currentLetterIndex >= _buildedAudioData.Count)
        {
            Debug.LogError($"Current letter index {currentLetterIndex} is out of range for list with {_buildedAudioData.Count} items.");
            currentLetterIndex = 0;
        }

        // Get the current letter data
        var currentLetterData = _buildedAudioData[currentLetterIndex];

        if (currentLetterData == null)
        {
            Debug.LogError($"Letter data at index {currentLetterIndex} is null. Please check your ScriptableObject assignments.");
            return;
        }

        SpawnHarakatButtonsForLetter(currentLetterData);
    }

    private void SpawnHarakatButtonsForLetter(HijaiyahAudioDataSO letterData)
    {
        if (harakatButtons == null || harakatButtons.Count < 3)
            return;

        if (currentLetterIndex % 2 == 0)
        {
            // Even index → Non-tain
            harakatButtons[0].Setup(letterData, HarokatType.Fathah);
            harakatButtons[1].Setup(letterData, HarokatType.Kasroh);
            harakatButtons[2].Setup(letterData, HarokatType.Dhommah);
        }
        else
        {
            // Odd index → Tain
            harakatButtons[0].Setup(letterData, HarokatType.Fathahtain);
            harakatButtons[1].Setup(letterData, HarokatType.Kasrohtain);
            harakatButtons[2].Setup(letterData, HarokatType.Dhommahtain);
        }
    }

    public void PlayLetterAudio(HijaiyahAudioDataSO audioData, HarokatType harokat)
    {
        switch (harokat)
        {
            case HarokatType.Fathah:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.fathahAudio);
                break;
            case HarokatType.Fathahtain:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.fathahtainAudio);
                break;
            case HarokatType.Kasroh:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.kasrahAudio);
                break;
            case HarokatType.Kasrohtain:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.kasrahtainAudio);
                break;
            case HarokatType.Dhommah:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.dammahAudio);
                break;
            case HarokatType.Dhommahtain:
                HijaiyahAudioManager.Instance.PlayAudioClip(audioData.dammahtainAudio);
                break;
        }
    }
}

public enum HarokatType
{
    Fathah,
    Fathahtain,
    Kasroh,
    Kasrohtain,
    Dhommah,
    Dhommahtain
}