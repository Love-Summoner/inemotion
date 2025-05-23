using System.Collections;
using UnityEngine;
using extOSC;

public class S1_3_PlaneFader : MonoBehaviour
{
    public GameObject plane; // Assign your plane GameObject here
    public GameObject whiteNoise; // Assign your white noise GameObject here
    public float fadeDuration = 1.0f; // Duration of the fade-out and fade-in

    private Material planeMaterial;
    private bool isFading = false;
    private bool isFadedOut = false;
    private bool showWhiteNoiseNext = true;

    public string Address = "/example/1";
    [Header("OSC Settings")]
    public OSCReceiver Receiver;

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("EFFECT Received: {0}", message);

            if (isFadedOut)
            {
                StartCoroutine(FadeIn());
            }
            else
            {
                StartCoroutine(FadeOut());
            }

    }

    void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);
        if (plane != null)
        {
            planeMaterial = plane.GetComponent<Renderer>().material;
        }
        if (whiteNoise != null)
        {
            whiteNoise.SetActive(false); // Ensure white noise is initially disabled
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        Color startColor = planeMaterial.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        if (whiteNoise != null && showWhiteNoiseNext)
        {
            whiteNoise.SetActive(true);
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            planeMaterial.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        planeMaterial.color = endColor;
        isFading = false;
        isFadedOut = true;

        if (whiteNoise != null)
        {
            whiteNoise.SetActive(showWhiteNoiseNext);
            showWhiteNoiseNext = !showWhiteNoiseNext; // Toggle for next time
        }
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float elapsedTime = 0f;

        Color startColor = planeMaterial.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            planeMaterial.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        planeMaterial.color = endColor;
        isFading = false;
        isFadedOut = false;

        if (whiteNoise != null)
        {
            whiteNoise.SetActive(false);
        }
    }
}
