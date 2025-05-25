using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // destroy the newer instance
        }
    }
}


