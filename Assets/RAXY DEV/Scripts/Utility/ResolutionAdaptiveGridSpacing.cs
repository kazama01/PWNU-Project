using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[ExecuteAlways]
public class ResolutionAdaptiveGridSpacing : MonoBehaviour
{
    public GridLayoutGroup gridTarget;

    [Title("Reference A (Tall)")]
    public Vector2 aspectRefA = new Vector2(9, 20);
    public Vector2 spacingRefA = new Vector2(10, 100);

    [Title("Reference B (Short)")]
    public Vector2 aspectRefB = new Vector2(9, 16);
    public Vector2 spacingRefB = new Vector2(10, 12.5f);

    [ShowInInspector, ReadOnly]
    public Vector2 currentAspectRatio;

    void Start()
    {
        UpdateSpacing();
    }

#if UNITY_EDITOR
    void Update()
    {
        UpdateSpacing();
    }
#endif

    void UpdateSpacing()
    {
        if (gridTarget == null)
            return;

        currentAspectRatio = GetAspectRatio(new Vector2(Screen.width, Screen.height));
        float aspectFloat = currentAspectRatio.x / currentAspectRatio.y;
        float aspectA = aspectRefA.x / aspectRefA.y;
        float aspectB = aspectRefB.x / aspectRefB.y;

        // Normalized t between A and B
        float t = Mathf.InverseLerp(aspectA, aspectB, aspectFloat);

        // Interpolate spacing
        gridTarget.spacing = Vector2.Lerp(spacingRefA, spacingRefB, t);
    }

    private Vector2 GetAspectRatio(Vector2 resolution)
    {
        float gcd = GCD((int)resolution.x, (int)resolution.y);
        return new Vector2(resolution.x / gcd, resolution.y / gcd);
    }

    private int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}
