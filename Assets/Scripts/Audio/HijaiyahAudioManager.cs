using System.Collections.Generic;
using UnityEngine;

namespace HijaiyahAudio
{
    public class HijaiyahAudioManager : MonoBehaviour
    {
        // Singleton instance
        private static HijaiyahAudioManager _instance;
        public static HijaiyahAudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<HijaiyahAudioManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("HijaiyahAudioManager");
                        _instance = obj.AddComponent<HijaiyahAudioManager>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<HijaiyahAudioDataSO> audioDataCollection;
        
        // Dictionary to look up audio clips by letter and harakat type
        private Dictionary<ArabLetter, Dictionary<HarakatType, AudioClip>> harakatAudioLookup;

        private void Awake()
        {
            // Singleton setup
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize the audio source if not assigned
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Build lookup dictionaries
            BuildAudioLookups();
        }

        private void BuildAudioLookups()
        {
            harakatAudioLookup = new Dictionary<ArabLetter, Dictionary<HarakatType, AudioClip>>();
            
            if (audioDataCollection != null)
            {
                foreach (var data in audioDataCollection)
                {
                    if (data != null)
                    {
                        // Add harakat audio
                        var harakatDict = new Dictionary<HarakatType, AudioClip>();
                        
                        if (data.fathahAudio != null)
                            harakatDict[HarakatType.Fathah] = data.fathahAudio;
                            
                        if (data.kasrahAudio != null)
                            harakatDict[HarakatType.Kasrah] = data.kasrahAudio;
                            
                        if (data.dammahAudio != null)
                            harakatDict[HarakatType.Dammah] = data.dammahAudio;
                        
                        harakatAudioLookup[data.letter] = harakatDict;
                    }
                }
            }
        }
        
        public void PlayLetterHarakatAudio(ArabLetter letter, HarakatType harakat)
        {
            if (harakatAudioLookup != null && 
                harakatAudioLookup.ContainsKey(letter) && 
                harakatAudioLookup[letter].ContainsKey(harakat))
            {
                audioSource.clip = harakatAudioLookup[letter][harakat];
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"No {harakat} audio found for letter {letter}");
            }
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        
        // Add audio data to the collection at runtime
        public void AddAudioData(HijaiyahAudioDataSO audioData)
        {
            if (audioData != null)
            {
                if (audioDataCollection == null)
                {
                    audioDataCollection = new List<HijaiyahAudioDataSO>();
                }
                
                // Check if we already have this letter
                bool alreadyExists = false;
                foreach (var existingData in audioDataCollection)
                {
                    if (existingData.letter == audioData.letter)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                
                if (!alreadyExists)
                {
                    audioDataCollection.Add(audioData);
                }
                
                // Initialize lookups if needed
                if (harakatAudioLookup == null)
                {
                    harakatAudioLookup = new Dictionary<ArabLetter, Dictionary<HarakatType, AudioClip>>();
                }
                
                // Add harakat audio
                if (!harakatAudioLookup.ContainsKey(audioData.letter))
                {
                    harakatAudioLookup[audioData.letter] = new Dictionary<HarakatType, AudioClip>();
                }
                
                var harakatDict = harakatAudioLookup[audioData.letter];
                
                if (audioData.fathahAudio != null)
                    harakatDict[HarakatType.Fathah] = audioData.fathahAudio;
                    
                if (audioData.kasrahAudio != null)
                    harakatDict[HarakatType.Kasrah] = audioData.kasrahAudio;
                    
                if (audioData.dammahAudio != null)
                    harakatDict[HarakatType.Dammah] = audioData.dammahAudio;
            }
        }
    }
}