using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine;
using MuslimHeroesMenu;

public class MuslimHeroesManager : SceneManagerBase
{
    public static MuslimHeroesManager Instance;

    [Title("Muslim Heroes")]
    public MuslimHeroDatabaseSO MuslimHeroDatabaseSO;
    public MuslimHeroDataUI HeroDataPreview;
    public GameObject muslimHeroBtnPrefab;
    public Transform buttonParent;
    //public NotificationRequest notificationRequest;

    [ShowInInspector]
    [ReadOnly]
    [NonSerialized]
    List<GameObject> _spawnedButton;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        //NotificationManager.Instance?.RequestNotification(notificationRequest);

        //NotificationManager.OnNotificationClosed += NotificationClosedHandler;

        SpawnButtons();
        HeroDataPreview.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        //NotificationManager.OnNotificationClosed -= NotificationClosedHandler;
    }

    // void NotificationClosedHandler(NotificationRequest notificationRequest)
    // {
    //     if (notificationRequest.RequestId == this.notificationRequest.RequestId)
    //     {
    //         Debug.Log("NAISE");
    //     }
    // }

    void SpawnButtons()
    {
        _spawnedButton = new List<GameObject>();

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in MuslimHeroDatabaseSO.muslimHeroDatas)
        {
            GameObject clone = Instantiate(muslimHeroBtnPrefab.gameObject, buttonParent);
            clone.GetComponent<MuslimHeroButtonUI>().Setup(data);
        }
    }

    public void OpenHeroDataPreview(MuslimHeroDataSO heroData)
    {
        HeroDataPreview.gameObject.SetActive(true);
        HeroDataPreview.Setup(heroData);
    }
}
