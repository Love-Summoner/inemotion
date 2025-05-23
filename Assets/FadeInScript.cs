using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using extOSC;

public class RawImageFader : MonoBehaviour
{
    public RawImage rawImage; // Drag your RawImage here
    public float fadeDuration = 1.0f;
    [Range(0f, 1f)] public float maxOpacity = 0.5f; // Editable in Inspector (0 to 1)

    private bool isFadingIn = false;
    private bool isVisible = false;

    public string Address = "/example/1";
    [Header("OSC Settings")]
    public OSCReceiver Receiver;

    private void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);
    }

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        if (isFadingIn)
            return; // Prevent overlapping coroutines

        if (isVisible)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            StartCoroutine(FadeIn());
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (isFadingIn)
                return; // Prevent overlapping coroutines

            if (isVisible)
            {
                StartCoroutine(FadeOut());
            }
            else
            {
                StartCoroutine(FadeIn());
            }
        }
    }

    IEnumerator FadeIn()
    {
        isFadingIn = true;
        float elapsedTime = 0f;
        Color color = rawImage.color;
        color.a = 0f;
        rawImage.color = color;
        rawImage.gameObject.SetActive(true); // Enable RawImage before fading in

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, maxOpacity, elapsedTime / fadeDuration);
            rawImage.color = color;
            yield return null;
        }

        color.a = maxOpacity;
        rawImage.color = color;
        isVisible = true;
        isFadingIn = false;
    }

    IEnumerator FadeOut()
    {
        isFadingIn = true;
        float elapsedTime = 0f;
        Color color = rawImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(maxOpacity, 0f, elapsedTime / fadeDuration);
            rawImage.color = color;
            yield return null;
        }

        color.a = 0f;
        rawImage.color = color;
        rawImage.gameObject.SetActive(false); // Disable after fading out
        isVisible = false;
        isFadingIn = false;
    }
}
