using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HijaiyahAudioData", menuName = "Scriptable Objects/Hijaiyah Audio Data")]
public class HijaiyahAudioDataSO : ScriptableObject
{
    [Title("Letter Information")]
    public ArabLetter letter;

    [Title("Harakat Audio")]
    [Tooltip("Audio clip for the letter with Fathah (A sound)")]
    public AudioClip fathahAudio;
    
    [Tooltip("Audio clip for the letter with Kasrah (I sound)")]
    public AudioClip kasrahAudio;
    
    [Tooltip("Audio clip for the letter with Dammah (U sound)")]
    public AudioClip dammahAudio;
    
    [Title("Harakat Visuals")]
    [Tooltip("Sprite for the letter with Fathah (A sound)")]
    public Sprite fathahSprite;
    
    [Tooltip("Sprite for the letter with Kasrah (I sound)")]
    public Sprite kasrahSprite;
    
    [Tooltip("Sprite for the letter with Dammah (U sound)")]
    public Sprite dammahSprite;
}