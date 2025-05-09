using HijaiyahHunterMenu;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HijaiyahHunterMenuManager : SceneManagerBase
{
    public static HijaiyahHunterMenuManager Instance;

    [Title("Hijaiyah Hunter", "SpawnButtons() is based on Sprite Atlas count")]
    public SpriteAtlas spriteAtlas;
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

        int spriteCount = spriteAtlas.spriteCount;
        Sprite[] sprites = new Sprite[spriteCount];
        spriteAtlas.GetSprites(sprites);

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < spriteCount; i++)
        {
            GameObject clone = Instantiate(letterButtonPrefab.gameObject, buttonParent);
            clone.GetComponent<LetterButtonUI>().Setup(sprites[i], (ArabLetter)i);
            clone.name = (ArabLetter)i + " - Button";
            _spawnedButton.Add(clone);
        }
    }
}
