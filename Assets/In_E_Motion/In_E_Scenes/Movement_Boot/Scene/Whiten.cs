using UnityEngine;
using UnityEngine.UI;

public class WhiteScreenWipe : MonoBehaviour
{
    public RectTransform whiteImage;
    public float duration = 60f;

    private float elapsedTime = 0f;
    private float screenHeight;

    void Start()
    {
        screenHeight = Screen.height;
        SetupRectTransform();
        SetHeight(0f);
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Smoothstep for natural ease-in-out
            float smoothT = t * t * (3f - 2f * t);

            float newHeight = Mathf.Lerp(0f, screenHeight, smoothT);
            SetHeight(newHeight);
        }
        else
        {
            // Guarantee full coverage
            SetHeight(screenHeight);
        }
    }

    void SetHeight(float height)
    {
        whiteImage.sizeDelta = new Vector2(0f, height);
        whiteImage.anchoredPosition = Vector2.zero;
    }

    void SetupRectTransform()
    {
        whiteImage.anchorMin = new Vector2(0f, 0f); // Bottom
        whiteImage.anchorMax = new Vector2(1f, 0f); // Bottom stretch
        whiteImage.pivot = new Vector2(0.5f, 0f);   // Pivot from bottom
    }
}
