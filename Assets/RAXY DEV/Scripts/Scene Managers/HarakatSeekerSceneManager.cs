using HijaiyahAudio;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarakatSeekerSceneManager : SceneManagerBase
{
    [Tooltip("Audio data for each letter")]
    public List<HijaiyahAudioDataSO> allLetterAudioDataList;

    // Current index in the letter list
    [ShowInInspector, ReadOnly]
    private int currentLetterIndex = 0;

    [ShowInInspector, ReadOnly]
    public HijaiyahAudioDataSO CurrentLetterSO => allLetterAudioDataList[currentLetterIndex];

    [HorizontalGroup("Letters")]
    public List<Image> letterImages;

    [HorizontalGroup("Letters")]
    public List<TextMeshProUGUI> letterPronounciations;

    protected override void Start()
    {
        base.Start();

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
        if (allLetterAudioDataList != null)
        {
            foreach (var audioData in allLetterAudioDataList)
            {
                if (audioData != null)
                {
                    audioManager.AddAudioData(audioData);
                }
            }
        }
    }

    [Button]
    public void ShowNextLetter()
    {
        if (allLetterAudioDataList != null && allLetterAudioDataList.Count > 0)
        {
            currentLetterIndex = (currentLetterIndex + 1) % allLetterAudioDataList.Count;
            ShowCurrentLetter();
        }
    }

    [Button]
    public void ShowPreviousLetter()
    {
        if (allLetterAudioDataList != null && allLetterAudioDataList.Count > 0)
        {
            currentLetterIndex = (currentLetterIndex - 1 + allLetterAudioDataList.Count) % allLetterAudioDataList.Count;
            ShowCurrentLetter();
        }
    }

    [Button]
    private void ShowCurrentLetter()
    {
        if (allLetterAudioDataList == null || allLetterAudioDataList.Count == 0)
        {
            Debug.LogError("No letter data available in allLetterAudioDataList. Please assign HijaiyahAudioDataSO objects to the list.");
            return;
        }

        if (currentLetterIndex < 0 || currentLetterIndex >= allLetterAudioDataList.Count)
        {
            Debug.LogError($"Current letter index {currentLetterIndex} is out of range for list with {allLetterAudioDataList.Count} items.");
            currentLetterIndex = 0;
        }

        // Get the current letter data
        var currentLetterData = allLetterAudioDataList[currentLetterIndex];

        if (currentLetterData == null)
        {
            Debug.LogError($"Letter data at index {currentLetterIndex} is null. Please check your ScriptableObject assignments.");
            return;
        }

        // Update the UI text if available
        // if (letterNameText)
        // {
        //     letterNameText.text = currentLetterData.letter.ToString();
        // }

        // Clear existing buttons and create new ones
        //ClearButtons();
        SpawnHarakatButtonsForLetter(currentLetterData);

        // Update navigation button states
        //UpdateNavigationButtons();
    }

    // private void ClearButtons()
    // {
    //     foreach (var button in _spawnedButtons)
    //     {
    //         if (button != null)
    //         {
    //             Destroy(button);
    //         }
    //     }
    //     _spawnedButtons.Clear();
    // }

    private void SpawnHarakatButtonsForLetter(HijaiyahAudioDataSO letterData)
    {
        // if (letterData == null || audioLetterButtonPrefab == null || canvasTransform == null)
        // {
        //     Debug.LogError("Missing required components for button creation: letterData, audioLetterButtonPrefab, or canvasTransform.");
        //     return;
        // }

        // if (fathahButtonAnchor == null || kasrahButtonAnchor == null || dammahButtonAnchor == null)
        // {
        //     Debug.LogError("One or more Harakat button anchors (fathahButtonAnchor, kasrahButtonAnchor, dammahButtonAnchor) are not assigned in the Inspector.");
        //     return;
        // }

        // // Create all three harakat buttons using the world positions from the assigned anchors
        // CreateHarakatButton(letterData, HarakatType.Fathah, fathahButtonAnchor.position);
        // CreateHarakatButton(letterData, HarakatType.Kasrah, kasrahButtonAnchor.position);
        // CreateHarakatButton(letterData, HarakatType.Dammah, dammahButtonAnchor.position);

        foreach (var letterImg in letterImages)
        {
            letterImg.sprite = letterData.letterSprite;
        }

        int index = 0;
        foreach (var pronounce in letterPronounciations)
        {
            if (index == 0)
            {
                pronounce.text = $"{letterData.prefixPronounciation}a".ToLower();
            }
            else if (index == 1)
            {
                pronounce.text = $"{letterData.prefixPronounciation}i".ToLower();
            }
            else if (index == 2)
            {
                pronounce.text = $"{letterData.prefixPronounciation}u".ToLower();
            }

            index++;
        }
    }

    public void PlayLetterAudio_A()
    {
        HijaiyahAudioManager.Instance.PlayAudioClip(CurrentLetterSO.fathahAudio);
    }

    public void PlayLetterAudio_I()
    {
        HijaiyahAudioManager.Instance.PlayAudioClip(CurrentLetterSO.kasrahAudio);
    }
    
    public void PlayLetterAudio_U()
    {
        HijaiyahAudioManager.Instance.PlayAudioClip(CurrentLetterSO.dammahAudio);
    }
    
    // private void CreateHarakatButton(HijaiyahAudioDataSO letterData, HarakatType harakatType, Vector3 worldPosition)
    // {
    //     // var button = Instantiate(audioLetterButtonPrefab.gameObject, canvasTransform);
    //     // // Set the button's world position to match the anchor's world position.
    //     // // This aligns the pivot of the button with the pivot of the anchor GameObject.
    //     // button.transform.position = worldPosition;

    //     // var buttonUI = button.GetComponent<AudioLetterButtonUI>();
    //     // buttonUI.SetupWithExplicitHarakat(letterData.letter, letterData, harakatType);

    //     // button.name = $"{letterData.letter} - {harakatType}";
    //     // _spawnedButtons.Add(button);
    // }

    // private void UpdateNavigationButtons()
    // {
    //     bool hasMultipleLetters = (allLetterAudioDataList != null && allLetterAudioDataList.Count > 1);

    //     if (nextButton) nextButton.interactable = hasMultipleLetters;
    //     if (previousButton) previousButton.interactable = hasMultipleLetters;
    // }

    // private void OnDestroy()
    // {
    //     // Clean up listeners
    //     if (nextButton) nextButton.onClick.RemoveListener(ShowNextLetter);
    //     if (previousButton) previousButton.onClick.RemoveListener(ShowPreviousLetter);
    // }
}