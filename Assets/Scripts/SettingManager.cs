using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    public GameObject settingPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSettingWindow()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingWindow()
    {
        settingPanel.SetActive(false);
    }
}
