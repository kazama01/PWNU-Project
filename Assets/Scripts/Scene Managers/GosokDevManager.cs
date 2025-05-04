using Sirenix.OdinInspector;
using UnityEngine;

public class GosokDevManager : SceneManagerBase
{
    public static GosokDevManager Instance;

    [TitleGroup("Gosok Manager")]
    public ScratchDatabaseSO scratchDatabaseSO;

    //[TitleGroup("Gosok Manager")]
    //[ShowInInspector]
    public const string SELECTED_GOSOK_LETTER_KEY = "SelectedGosokLetter";

    [TitleGroup("Gosok Manager")]
    [ShowInInspector]
    [ReadOnly]
    public string SelectedGosokLetterString { get; private set; }

    public ArabLetter SelectedGosokLetter
    {
        get
        {
            return GlobalUtility.GetArabLetterFromString(SelectedGosokLetterString);
        }
    }

    [TitleGroup("Gosok Manager")]
    [ShowInInspector]
    [ReadOnly]
    GosokUI _gosokUI;

    public void Awake()
    {
        Instance = this;
        LoadSelectedGosokLetter();
    }

    public void LoadSelectedGosokLetter()
    {
        SelectedGosokLetterString = PlayerPrefs.GetString(SELECTED_GOSOK_LETTER_KEY);
    }

    protected override void Start()
    {
        base.Start();

        _gosokUI = FindFirstObjectByType<GosokUI>();
        _gosokUI.Setup(scratchDatabaseSO.scratchDataDict[SelectedGosokLetter]);
    }
}
