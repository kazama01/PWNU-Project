using MEC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static event Action<NotificationRequest> OnNotificationClosed;
    public static event Action<NotificationType> OnAllNotificationTypeCleared;

    public static NotificationManager Instance;

    public Transform notificationParent;

    [Title("Center Notification")]
    public GameObject centerNotification;
    public TextMeshProUGUI messageHeaderTxt_Center;
    public TextMeshProUGUI messageTxt_Center;
    public Image messageIconImg_Center;

    [TitleGroup("Data")]
    public List<Sprite> messageIcons;
    [TitleGroup("Data")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "presetId")]
    public List<NotificationPreset> Presets;

    [TitleGroup("Queue")]
    [ShowInInspector]
    [ReadOnly]
    private Dictionary<NotificationType, Queue<NotificationRequest>> notificationQueues;

    [TitleGroup("Queue")]
    [ShowInInspector]
    [ReadOnly]
    private Dictionary<NotificationType, bool> isNotificationActive;

    [TitleGroup("Queue")]
    [ShowInInspector]
    [ReadOnly]
    public NotificationRequest ActiveNotification { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        notificationQueues = new Dictionary<NotificationType, Queue<NotificationRequest>>();
        isNotificationActive = new Dictionary<NotificationType, bool>();

        foreach (NotificationType type in Enum.GetValues(typeof(NotificationType)))
        {
            notificationQueues[type] = new Queue<NotificationRequest>();
            isNotificationActive[type] = false;
        }
    }

    [TitleGroup("Debug")]
    [Button]
    public void RequestNotification(string header, string message, string presetId = "default")
    {
        NotificationRequest request = new NotificationRequest
        {
            presetId = presetId,
            headerMessage = header,
            message = message,
        };

        RequestNotification(request);
    }

    [TitleGroup("Debug")]
    [Button]
    public void RequestNotification(NotificationRequest request)
    {
        NotificationPreset selectedPreset = Presets.Find(x => x.presetId == request.presetId);

        if (selectedPreset == null)
        {
            Debug.LogError($"Preset with ID '{request.presetId}' not found.");
            return;
        }

        NotificationType notifType = selectedPreset.notificationType;
        notificationQueues[notifType].Enqueue(request);

        if (!isNotificationActive[notifType])
        {
            ShowNextNotification(notifType);
        }
    }

    private void ShowNextNotification(NotificationType notificationType)
    {
        if (notificationQueues[notificationType].Count == 0)
        {
            isNotificationActive[notificationType] = false;
            OnAllNotificationTypeCleared?.Invoke(notificationType);
            return;
        }

        isNotificationActive[notificationType] = true;
        ActiveNotification = notificationQueues[notificationType].Dequeue();
        ShowNotification(ActiveNotification);
    }

    private void ShowNotification(NotificationRequest request)
    {
        NotificationPreset selectedPreset = Presets.Find(x => x.presetId == request.presetId);

        if (selectedPreset == null)
        {
            Debug.LogError($"Preset with ID '{request.presetId}' not found.");
            return;
        }

        switch (selectedPreset.notificationType)
        {
            case NotificationType.Center:
                ShowCenterNotification(request, selectedPreset);
                break;
            case NotificationType.Top:
                // Implement Top notification logic here
                break;
            case NotificationType.Bottom:
                // Implement Bottom notification logic here
                break;
        }
    }

    private void ShowCenterNotification(NotificationRequest request, NotificationPreset preset)
    {
        messageHeaderTxt_Center.text = request.headerMessage;
        messageTxt_Center.text = request.message;

        if (preset.messageIconIndex >= 0 && preset.messageIconIndex < messageIcons.Count)
        {
            messageIconImg_Center.sprite = messageIcons[preset.messageIconIndex];
        }

        centerNotification.SetActive(true);

        if (!preset.isInfinite)
        {
            Timing.RunCoroutine(CloseNotificationAfterDelay(NotificationType.Center, preset.duration));
        }
    }

    private IEnumerator<float> CloseNotificationAfterDelay(NotificationType notificationType, float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        CloseNotification(notificationType);
    }

    public void CloseNotification(NotificationType notificationType)
    {
        if (notificationType == NotificationType.Center)
        {
            centerNotification.SetActive(false);
        }

        OnNotificationClosed?.Invoke(ActiveNotification);
        ShowNextNotification(notificationType);
    }
    public void CloseNotification_Center()
    {
        CloseNotification(NotificationType.Center);
    }
}

[Serializable]
public class NotificationPreset
{
    public string presetId;
    public int messageIconIndex;

    [HideIf("@isInfinite")]
    public bool canBeClosedByClick;

    [Title("Duration")]
    public bool isInfinite;
    [HideIf("@isInfinite")]
    public float duration;

    public NotificationType notificationType;
}

[Serializable]
public struct NotificationRequest
{
    public string presetId;
    public string headerMessage;

    [TextArea(3, 5)]
    public string message;

    [ShowInInspector]
    public string RequestId
    {
        get
        {
            if (string.IsNullOrEmpty(headerMessage))
            {
                return string.Empty;
            }
            return headerMessage.ToLower().Replace(" ", "_");
        }
    }
}

public enum NotificationType
{
    Center,
    Top,
    Bottom
}
