using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Muslim Hero Data SO", menuName = "Scriptable Objects/Muslim Hero Data SO")]
public class MuslimHeroDataSO : ScriptableObject
{
    public string heroName;
    [TextArea(3, 5)]
    public string heroDetail;
    public Sprite heroImage;

    [ShowInInspector]
    [ReadOnly]
    public NotificationRequest NotificationRequest
    {
        get
        {
            NotificationRequest req = new NotificationRequest();
            req.presetId = "default";
            req.headerMessage = heroName;
            req.message = heroDetail;
            req.useImage = true;
            req.imageSprite = heroImage;

            return req;
        }
    }

    [Button]
    [HideInEditorMode]
    public void TestRequest()
    {
        NotificationManager.Instance.RequestNotification(NotificationRequest);
    }
}
