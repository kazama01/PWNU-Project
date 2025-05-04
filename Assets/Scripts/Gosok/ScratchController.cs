using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MEC;

public class ScratchController : MonoBehaviour
{
    public Texture2D originalImage;
    public RawImage originalRawImageComp;
    public RawImage scratchRawImageComp;
    public Material targetMaterial;

    //public int textureSize = 512;
    public int brushSize = 48;
    public Color brushColor = Color.white;

    [InfoBox("Place the canvas here")]
    public GraphicRaycaster raycaster;

    [ShowInInspector]
    [ReadOnly]
    Texture2D _generatedMaskTexture;
    RectTransform _imageRect;  
    PointerEventData _pointerEventData;
    EventSystem _eventSystem;

    float _width;
    float _height;

    void Start()
    {
        _imageRect = scratchRawImageComp.rectTransform;
        _eventSystem = FindFirstObjectByType<EventSystem>();
    }

    [Button]
    public void Setup(Texture2D image)
    {
        if (image != null)
        {
            originalImage = image;
            _generatedMaskTexture = GetAlphaMap(originalImage);

            _width = originalImage.width;
            _height = originalImage.height;

            targetMaterial.SetTexture("_MaskTex", _generatedMaskTexture);
            originalRawImageComp.texture = originalImage;
            scratchRawImageComp.material = targetMaterial;
        }
        else
        {
            Debug.LogError("No object found in the list.");
        }
    }

    [Button]
    Texture2D GetAlphaMap(Texture2D texture, float threshold = 0.5f)
    {
        if (texture == null)
        {
            Debug.LogError("Original texture is null.");
            return null;
        }

        // Create a new texture to store the alpha map
        Texture2D alphaMap = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        // Get all pixels from the original texture
        Color[] pixels = texture.GetPixels();

        // Create an array to store the black-and-white alpha values
        Color[] alphaPixels = new Color[pixels.Length];

        // Apply the threshold to generate a black-and-white alpha map
        for (int i = 0; i < pixels.Length; i++)
        {
            float alpha = pixels[i].a; // Get the alpha value
            if (alpha > threshold)
            {
                alphaPixels[i] = Color.black;
            }
            else
            {
                alphaPixels[i] = Color.white;
            }
        }

        // Set the black-and-white alpha values to the new texture
        alphaMap.SetPixels(alphaPixels);
        alphaMap.Apply();

        return alphaMap;
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(DelayedUpdateCoHandle);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(_pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject == scratchRawImageComp.gameObject)
                {
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_imageRect, Input.mousePosition, null, out localPoint);

                    Vector2 pivot = _imageRect.pivot;
                    float x = (localPoint.x + _imageRect.rect.width * pivot.x) / _imageRect.rect.width;
                    float y = (localPoint.y + _imageRect.rect.height * pivot.y) / _imageRect.rect.height;

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

    //void ClearMask()
    //{
    //    Color[] colors = new Color[textureSize * textureSize];
    //    for (int i = 0; i < colors.Length; i++)
    //        colors[i] = Color.black;

    //    _generatedMaskTexture.SetPixels(colors);
    //    _generatedMaskTexture.Apply();
    //}

    void PaintAtUV(Vector2 uv)
    {
        int x = (int)(uv.x * _width);
        int y = (int)(uv.y * _height);

        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < _width && py >= 0 && py < _height)
                {
                    float dist = Vector2.Distance(new Vector2(px, py), new Vector2(x, y));
                    if (dist <= brushSize)
                        _generatedMaskTexture.SetPixel(px, py, brushColor);
                }
            }
        }

        _generatedMaskTexture.Apply();
    }

    [Button]
    void CalculateBlackWhiteRatio()
    {
        int blackCount = 0;
        int whiteCount = 0;

        // Mengakses seluruh piksel dalam gambar
        Color[] pixels = _generatedMaskTexture.GetPixels();

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
