using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Muslim Hero Data SO", menuName = "Scriptable Objects/Muslim Hero Data SO")]
public class MuslimHeroDataSO : ScriptableObject
{
    [HideLabel]
    public NotificationRequest NotificationRequest;

    [Button]
    [HideInEditorMode]
    public void TestRequest()
    {
        NotificationManager.Instance.RequestNotification(NotificationRequest);
    }
}
