using Cysharp.Threading.Tasks;
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
    public RectTransform detailFrameRect;

    public MuslimHeroDataSO CurrentHeroData { get; set; }

    [Button]
    public void Setup(MuslimHeroDataSO heroData)
    {
        SetupAsync(heroData).Forget();
    }

    async UniTask SetupAsync(MuslimHeroDataSO heroData)
    {
        CurrentHeroData = heroData;

        heroImg.sprite = CurrentHeroData.heroImage;
        heroNameTxt.text = CurrentHeroData.heroName;
        lifespanTxt.text = CurrentHeroData.heroLifeSpan;
        titleTxt.text = CurrentHeroData.heroTitle;
        detailTxt.text = CurrentHeroData.heroDetail;

        gameObject.SetActive(true);
        detailFrameRect.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(0.1f);
        detailFrameRect.gameObject.SetActive(false);
        await UniTask.WaitForSeconds(0.1f);
        detailFrameRect.gameObject.SetActive(true);
    }

    public void OnClick_SuaraBtn()
    {
        MuslimHeroesManager.Instance.PlayAudio(CurrentHeroData.heroDetailNarrationClip);
    }
}
