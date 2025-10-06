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

    [ShowInInspector]
    [ReadOnly]
    [NonSerialized]
    List<GameObject> _spawnedButton;

    AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();

        SpawnButtons();
        HeroDataPreview.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
    }

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
            clone.gameObject.name = data.heroName;
        }
    }

    public void OpenHeroDataPreview(MuslimHeroDataSO heroData)
    {
        HeroDataPreview.Setup(heroData);
    }

    [Button]
    public void PlayAudio(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
