using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public const string KOIN_KEY = "Koin";

    [TitleGroup("Koin")]
    [ShowInInspector]
    public int Koin
    {
        get
        {
            return PlayerPrefs.GetInt(KOIN_KEY);
        }
        private set
        {
            PlayerPrefs.SetInt(KOIN_KEY, value);
            koin_Txt.text = Koin.ToString();
        }
    }

    [TitleGroup("UI")]
    public Transform koinParent;
    [TitleGroup("UI")]
    public TextMeshProUGUI koin_Txt;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (PlayerPrefs.HasKey(KOIN_KEY) == false)
            Koin = 0;
    }

    private void Start()
    {
        koin_Txt.text = Koin.ToString();
    }

    [TitleGroup("Koin")]
    [Button]
    public void AddKoin(int amount)
    {
        Koin += amount;
    }

    [TitleGroup("Koin")]
    [Button]
    public void RemoveKoin(int amount)
    {
        Koin -= amount;
    }

    [TitleGroup("UI")]
    [Button]
    public void ShowKoin(bool show)
    {
        koinParent?.gameObject?.SetActive(show);
    }
}
