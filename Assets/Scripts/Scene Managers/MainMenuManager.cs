using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSetting()
    {
        SettingManager.Instance.OpenSettingWindow();
    }

    public void CloseSetting()
    {
        SettingManager.Instance.CloseSettingWindow();
    }
}
