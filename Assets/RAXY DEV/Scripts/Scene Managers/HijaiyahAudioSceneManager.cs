using HijaiyahAudio;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HijaiyahAudioSceneManager : SceneManagerBase
{
    [Title("Hijaiyah Audio Player")]
    [Tooltip("The button prefab for audio letter buttons")]
    public AudioLetterButtonUI audioLetterButtonPrefab;
    
    [Tooltip("Canvas transform where buttons will be spawned")]
    public Transform canvasTransform;

    [Title("Harakat Button Anchors")]
    [Tooltip("Assign a Transform whose localPosition will dictate the Fathah button's placement.")]
    public Transform fathahButtonAnchor;
    [Tooltip("Assign a Transform whose localPosition will dictate the Kasrah button's placement.")]
    public Transform kasrahButtonAnchor;
    [Tooltip("Assign a Transform whose localPosition will dictate the Dammah button's placement.")]
    public Transform dammahButtonAnchor;
    
    [Tooltip("Audio data for each letter")]
    public List<HijaiyahAudioDataSO> allLetterAudioDataList;

    [ShowInInspector, ReadOnly]
    private List<GameObject> _spawnedButtons = new List<GameObject>();
    
    [Title("Navigation")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Text letterNameText;
    
    // Current index in the letter list
    [ShowInInspector, ReadOnly]
    private int currentLetterIndex = 0;

    protected override void Start()
    {
        base.Start();
        
        // Initialize the audio manager and register all audio data
        InitializeAudioManager();
        
        // Set up button listeners
        if (nextButton) nextButton.onClick.AddListener(ShowNextLetter);
        if (previousButton) previousButton.onClick.AddListener(ShowPreviousLetter);
        
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
    
    private void ShowNextLetter()
    {
        if (allLetterAudioDataList != null && allLetterAudioDataList.Count > 0)
        {
            currentLetterIndex = (currentLetterIndex + 1) % allLetterAudioDataList.Count;
            ShowCurrentLetter();
        }
    }
    
    private void ShowPreviousLetter()
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
        if (letterNameText)
        {
            letterNameText.text = currentLetterData.letter.ToString();
        }
        
        // Clear existing buttons and create new ones
        ClearButtons();
        SpawnHarakatButtonsForLetter(currentLetterData);
        
        // Update navigation button states
        UpdateNavigationButtons();
    }
    
    private void ClearButtons()
    {
        foreach (var button in _spawnedButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        _spawnedButtons.Clear();
    }
    
    private void SpawnHarakatButtonsForLetter(HijaiyahAudioDataSO letterData)
    {
        if (letterData == null || audioLetterButtonPrefab == null || canvasTransform == null)
        {
            Debug.LogError("Missing required components for button creation: letterData, audioLetterButtonPrefab, or canvasTransform.");
            return;
        }

        if (fathahButtonAnchor == null || kasrahButtonAnchor == null || dammahButtonAnchor == null)
        {
            Debug.LogError("One or more Harakat button anchors (fathahButtonAnchor, kasrahButtonAnchor, dammahButtonAnchor) are not assigned in the Inspector.");
            return;
        }
        
        // Create all three harakat buttons using the world positions from the assigned anchors
        CreateHarakatButton(letterData, HarakatType.Fathah, fathahButtonAnchor.position);
        CreateHarakatButton(letterData, HarakatType.Kasrah, kasrahButtonAnchor.position);
        CreateHarakatButton(letterData, HarakatType.Dammah, dammahButtonAnchor.position);
    }
    
    private void CreateHarakatButton(HijaiyahAudioDataSO letterData, HarakatType harakatType, Vector3 worldPosition)
    {
        var button = Instantiate(audioLetterButtonPrefab.gameObject, canvasTransform);
        // Set the button's world position to match the anchor's world position.
        // This aligns the pivot of the button with the pivot of the anchor GameObject.
        button.transform.position = worldPosition;
        
        var buttonUI = button.GetComponent<AudioLetterButtonUI>();
        buttonUI.SetupWithExplicitHarakat(letterData.letter, letterData, harakatType);
        
        button.name = $"{letterData.letter} - {harakatType}";
        _spawnedButtons.Add(button);
    }
    
    private void UpdateNavigationButtons()
    {
        bool hasMultipleLetters = (allLetterAudioDataList != null && allLetterAudioDataList.Count > 1);
        
        if (nextButton) nextButton.interactable = hasMultipleLetters;
        if (previousButton) previousButton.interactable = hasMultipleLetters;
    }

    private void OnDestroy()
    {
        // Clean up listeners
        if (nextButton) nextButton.onClick.RemoveListener(ShowNextLetter);
        if (previousButton) previousButton.onClick.RemoveListener(ShowPreviousLetter);
    }
}