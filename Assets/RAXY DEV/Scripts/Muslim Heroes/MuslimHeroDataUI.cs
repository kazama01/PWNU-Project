using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MuslimHeroDataUI : MonoBehaviour
{
    public Image heroImg;
    public TextMeshProUGUI heroNameTxt;
    public TextMeshProUGUI lifespanTxt;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI detailTxt;

    public MuslimHeroDataSO CurrentHeroData { get; set; }

    [Button]
    public void Setup(MuslimHeroDataSO heroData)
    {
        CurrentHeroData = heroData;

        heroImg.sprite = CurrentHeroData.heroImage;
        heroNameTxt.text = CurrentHeroData.heroName;
        lifespanTxt.text = CurrentHeroData.heroLifeSpan;
        titleTxt.text = CurrentHeroData.heroTitle;
        detailTxt.text = CurrentHeroData.heroDetail;
    }
}
