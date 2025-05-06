using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HijaiyahAudio
{
    [RequireComponent(typeof(Image))] // This ensures the GameObject has an Image component
    public class AudioLetterButtonUI : MonoBehaviour
    {
        // Match the original structure to make it easier to assign in the inspector
        [Tooltip("The Image component that will display the letter sprite")]
        public Image imageComp;
        
        // Making these public serialized fields instead of properties to ensure they're properly serialized
        [SerializeField]
        [ReadOnly] 
        private ArabLetter _letter;
        public ArabLetter Letter { get { return _letter; } private set { _letter = value; } }
        
        [SerializeField]
        [ReadOnly] 
        private HarakatType _harakat;
        public HarakatType Harakat { get { return _harakat; } private set { _harakat = value; } }

        // Reference to the audio data
        private HijaiyahAudioDataSO audioData;

        // Automatically assign the Image component if not manually assigned
        private void Awake()
        {
            if (imageComp == null)
            {
                imageComp = GetComponent<Image>();
                if (imageComp == null)
                {
                    Debug.LogError("No Image component found on GameObject. Please add one.");
                }
            }
        }

        // Setup method for harakat
        public void SetupWithExplicitHarakat(ArabLetter letter, HijaiyahAudioDataSO data, HarakatType harakatType)
        {
            // Store the letter and harakat type
            _letter = letter;
            _harakat = harakatType;
            audioData = data;
            
            if (data == null)
            {
                Debug.LogError($"Audio data is null for letter {letter}");
                return;
            }
            
            // Set the appropriate sprite based on harakat type
            if (imageComp != null)
            {
                Sprite spriteToUse = null;
                switch (harakatType)
                {
                    case HarakatType.Fathah:
                        spriteToUse = data.fathahSprite;
                        break;
                    case HarakatType.Kasrah:
                        spriteToUse = data.kasrahSprite;
                        break;
                    case HarakatType.Dammah:
                        spriteToUse = data.dammahSprite;
                        break;
                }
                
                if (spriteToUse != null)
                {
                    imageComp.sprite = spriteToUse;
                }
                else
                {
                    Debug.LogWarning($"No sprite found for {letter} with {harakatType}");
                }
            }
            else
            {
                Debug.LogError("Image component not assigned in AudioLetterButtonUI");
            }
        }

        public void OnClick()
        {
            // Always use the HijaiyahAudioManager regardless of whether we have local audioData
            HijaiyahAudioManager.Instance.PlayLetterHarakatAudio(_letter, _harakat);
        }
    }
}