using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MuslimHeroesMenu
{
    public class MuslimHeroButtonUI : MonoBehaviour
    {
        public Image imageComp;

        [ShowInInspector]
        [ReadOnly]
        public MuslimHeroDataSO MuslimHeroDataSO { get; private set; }

        public void Setup(MuslimHeroDataSO dataSO)
        {
            MuslimHeroDataSO = dataSO;
            imageComp.sprite = MuslimHeroDataSO.NotificationRequest.imageSprite;
        }

        public void OnClick()
        {
            NotificationManager.Instance.RequestNotification(MuslimHeroDataSO.NotificationRequest);
        }
    }
}
