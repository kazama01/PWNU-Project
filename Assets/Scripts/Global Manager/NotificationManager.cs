using MEC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public Transform notificationParent;

    [Title("Center Notification")]
    public GameObject centerNotification;
    public TextMeshProUGUI messageHeaderTxt_Center;
    public TextMeshProUGUI messageTxt_Center;
    public Image messageIconImg_Center;
    //public Image messageImageImg_Center;
    public GameObject closeMessageLabel_Center;

    [TitleGroup("Data")]
    public List<Sprite> messageIcons;
    [TitleGroup("Data")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "presetId")]
    public List<NotificationPreset> Presets;

    [TitleGroup("Queue")]
    [ShowInInspector]
    public Queue<NotificationRequest> CenterNotificationQueue;
    [TitleGroup("Queue")]
    [ShowInInspector]
    public Queue<NotificationRequest> TopNotificationQueue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        CenterNotificationQueue = new Queue<NotificationRequest>();
        TopNotificationQueue = new Queue<NotificationRequest>();
    }

    [TitleGroup("Debug")]
    [Button]
    public void RequestNotification(string header, string message, string presetId = "default")
    {
        NotificationRequest request = new NotificationRequest()
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
            Debug.Log($"Selected preset is Null");
            return;
        }

        NotificationType notifType = selectedPreset.notificationType;

        if (notifType == NotificationType.Center)
        {
            CenterNotificationQueue.Enqueue(request);
        }
        else if (notifType == NotificationType.Top)
        {
            TopNotificationQueue.Enqueue(request);
        }

        CheckQueue(notifType);
    }

    void ShowNotification(NotificationRequest request)
    {
        NotificationPreset selectedPreset = Presets.Find(x => x.presetId == request.presetId);

        if (selectedPreset == null)
        {
            Debug.Log($"Selected preset is Null");
            return;
        }

        if (selectedPreset.notificationType == NotificationType.Center)
        {
            if (selectedPreset.isInfinite)
            {
                selectedPreset.canBeClosedByClick = true;
            }
            else
            {
                DelayCloseNotificationCoHandle_Center = Timing.RunCoroutine(DelayCloseNotificationCo_Center(selectedPreset.duration));
            }

            messageHeaderTxt_Center.text = request.headerMessage;
            messageTxt_Center.text = request.message;

            if (messageIcons != null &&
                selectedPreset != null &&
                selectedPreset.messageIconIndex >= 0 &&
                selectedPreset.messageIconIndex < messageIcons.Count &&
                messageIcons[selectedPreset.messageIconIndex] != null &&
                messageIconImg_Center != null)
            {
                messageIconImg_Center.sprite = messageIcons[selectedPreset.messageIconIndex];
            }

            centerNotification.SetActive(true);
        }
        else if (selectedPreset.notificationType == NotificationType.Top)
        {
        }
    }

    public void CloseNotification_Center()
    {
        CloseNotification(NotificationType.Center);
    }

    public void CloseNotification(NotificationType notificationType)
    {
        if (notificationType == NotificationType.Center)
        {
            Timing.KillCoroutines(DelayCloseNotificationCoHandle_Center);
            centerNotification.SetActive(false);

            CheckQueue(notificationType);
        }
        else if (notificationType == NotificationType.Top)
        {

        }
    }

    CoroutineHandle DelayCloseNotificationCoHandle_Center;
    IEnumerator<float> DelayCloseNotificationCo_Center(float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        CloseNotification_Center();
    }

    void CheckQueue(NotificationType notificationType)
    {
        if (notificationType == NotificationType.Center)
        {
            if (CenterNotificationQueue.Count > 0)
            {
                ShowNotification(CenterNotificationQueue.Dequeue());
            }
        }
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
    public string message;
}

public enum NotificationType
{
    Center,
    Top,
    Bottom
}
