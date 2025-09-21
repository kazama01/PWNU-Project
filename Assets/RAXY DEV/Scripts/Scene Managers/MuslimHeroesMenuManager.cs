using UnityEngine;

public class MuslimHeroesMenuManager : SceneManagerBase
{
    public static MuslimHeroesMenuManager Instance;

    void Awake()
    {
        Instance = this;
    }
}
