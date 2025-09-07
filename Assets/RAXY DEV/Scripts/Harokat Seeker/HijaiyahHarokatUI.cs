using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HijaiyahHarokatUI : MonoBehaviour
{
    public HarokatType Harokat;

    public Image harokatImage;
    public TextMeshProUGUI harokatPronounceTxt;
    public List<GameObject> harokatVisuals;

    [ShowInInspector]
    [ReadOnly]
    HijaiyahAudioDataSO _audioDataSO;

    [Button]
    public void Setup(HijaiyahAudioDataSO audioData, HarokatType harokat)
    {
        _audioDataSO = audioData;
        Harokat = harokat;

        foreach (var obj in harokatVisuals)
        {
            obj.SetActive(false);
        }

        if (harokatVisuals.Count > (int)harokat)
        {
            harokatVisuals[(int)harokat].SetActive(true);
        }

        harokatImage.sprite = audioData.letterSprite;

        string suffix = harokat switch
        {
            HarokatType.Fathah => "a",
            HarokatType.Fathahtain => "an",
            HarokatType.Kasroh => "i",
            HarokatType.Kasrohtain => "in",
            HarokatType.Dhommah => "u",
            HarokatType.Dhommahtain => "un",
            _ => string.Empty
        };

        harokatPronounceTxt.text = $"{audioData.prefixPronounciation}{suffix}".ToLower();
    }

    public void OnClick()
    {
        HarakatSeekerSceneManager.Instance.PlayLetterAudio(_audioDataSO, Harokat);
    }
}
