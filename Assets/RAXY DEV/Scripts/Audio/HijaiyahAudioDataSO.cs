using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HijaiyahAudioData", menuName = "Scriptable Objects/Hijaiyah Audio Data")]
public class HijaiyahAudioDataSO : ScriptableObject
{
    [Title("Letter Information")]
    public ArabLetter letter;
    public Sprite letterSprite;
    public string prefixPronounciation;

    [Title("Harakat Audio")]
    [Tooltip("Audio clip for the letter with Fathah (A sound)")]
    public AudioClip fathahAudio;

    [Tooltip("Audio clip for the letter with Kasrah (I sound)")]
    public AudioClip kasrahAudio;

    [Tooltip("Audio clip for the letter with Dammah (U sound)")]
    public AudioClip dammahAudio;

    [Tooltip("Audio clip for the letter with Fathah (An sound)")]
    public AudioClip fathahtainAudio;
    
    [Tooltip("Audio clip for the letter with Kasrah (In sound)")]
    public AudioClip kasrahtainAudio;
    
    [Tooltip("Audio clip for the letter with Dammah (Un sound)")]
    public AudioClip dammahtainAudio;
}