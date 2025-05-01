using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MEC;

public class TexturePainterUI : MonoBehaviour
{
    public RawImage targetImage;
    public Material targetMaterial;
    public int textureSize = 512;
    public int brushSize = 64;
    public Color brushColor = Color.white;

    private Texture2D maskTexture;
    private RectTransform imageRect;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        maskTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);
        ClearMask();

        targetMaterial.SetTexture("_MaskTex", maskTexture);
        targetImage.material = targetMaterial;

        imageRect = targetImage.rectTransform;
        raycaster = FindFirstObjectByType<GraphicRaycaster>();
        eventSystem = FindFirstObjectByType<EventSystem>();

        //DelayedUpdateCoHandle = Timing.RunCoroutine(DelayedUpdateCo());
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(DelayedUpdateCoHandle);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject == targetImage.gameObject)
                {
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, Input.mousePosition, null, out localPoint);

                    Vector2 pivot = imageRect.pivot;
                    float x = (localPoint.x + imageRect.rect.width * pivot.x) / imageRect.rect.width;
                    float y = (localPoint.y + imageRect.rect.height * pivot.y) / imageRect.rect.height;

                    PaintAtUV(new Vector2(x, y));
                    break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CalculateBlackWhiteRatio();
        }
    }

    CoroutineHandle DelayedUpdateCoHandle;
    IEnumerator<float> DelayedUpdateCo()
    {
        while (true)
        {
            CalculateBlackWhiteRatio();
            yield return Timing.WaitForSeconds(0.1f);
        }
    }

    void ClearMask()
    {
        Color[] colors = new Color[textureSize * textureSize];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.black;

        maskTexture.SetPixels(colors);
        maskTexture.Apply();
    }

    void PaintAtUV(Vector2 uv)
    {
        int x = (int)(uv.x * textureSize);
        int y = (int)(uv.y * textureSize);

        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < textureSize && py >= 0 && py < textureSize)
                {
                    float dist = Vector2.Distance(new Vector2(px, py), new Vector2(x, y));
                    if (dist <= brushSize)
                        maskTexture.SetPixel(px, py, brushColor);
                }
            }
        }

        maskTexture.Apply();
    }

    [Button]
    void CalculateBlackWhiteRatio()
    {
        int blackCount = 0;
        int whiteCount = 0;

        // Mengakses seluruh piksel dalam gambar
        Color[] pixels = maskTexture.GetPixels();

        // Menghitung jumlah piksel hitam dan putih
        foreach (Color pixel in pixels)
        {
            if (pixel == Color.black) // Hitam
            {
                blackCount++;
            }
            else if (pixel == Color.white) // Putih
            {
                whiteCount++;
            }
        }

        // Menghitung total piksel
        int totalPixels = blackCount + whiteCount;

        // Menghitung perbandingan hitam-putih
        if (totalPixels > 0)
        {
            float blackRatio = (float)blackCount / totalPixels;
            float whiteRatio = (float)whiteCount / totalPixels;

            //Debug.Log($"Perbandingan Hitam: {blackRatio * 100}%");
            Debug.Log($"Perbandingan Putih: {whiteRatio * 100}%");
        }
        else
        {
            Debug.Log("Gambar tidak memiliki piksel hitam atau putih.");
        }
    }
}
