using HijaiyahHunterMenu;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HijaiyahHunterMenuManager : SceneManagerBase
{
    public static HijaiyahHunterMenuManager Instance;

    [Title("Hijaiyah Hunter")]
    public ScratchDatabaseSO scratchDatabaseSO;
    public LetterButtonUI letterButtonPrefab;
    public Transform buttonParent;

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

        SpawnButtons();
    }

    [Button]
    void SpawnButtons()
    {
        _spawnedButton = new List<GameObject>();

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var scratchData in scratchDatabaseSO.scratchDataDict)
        {
            if (scratchData.Value == null)
                continue;

            GameObject clone = Instantiate(letterButtonPrefab.gameObject, buttonParent);
            clone.GetComponent<LetterButtonUI>().Setup(scratchData.Value.letterImage, scratchData.Key);
            clone.name = scratchData.Key.ToString();
            _spawnedButton.Add(clone);
        }
    }
}
