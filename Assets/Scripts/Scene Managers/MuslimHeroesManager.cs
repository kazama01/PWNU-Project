using Sirenix.OdinInspector;
using UnityEngine;

public class MuslimHeroesManager : SceneManagerBase
{
    public static MuslimHeroesManager Instance;

    [Title("Muslim Heroes")]
    public NotificationRequest notificationRequest;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        NotificationManager.Instance.RequestNotification(notificationRequest);

        NotificationManager.OnNotificationClosed += NotificationClosedHandler;
    }

    private void OnDestroy()
    {
        NotificationManager.OnNotificationClosed -= NotificationClosedHandler;
    }

    void NotificationClosedHandler(NotificationRequest notificationRequest)
    {
        if (notificationRequest.RequestId == this.notificationRequest.RequestId)
        {
            Debug.Log("NAISE");
        }
    }
}
