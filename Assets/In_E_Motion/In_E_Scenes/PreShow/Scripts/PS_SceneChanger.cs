using extOSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PS_SceneChanger : MonoBehaviour
{
    #region Public Vars
    private static PS_SceneChanger instance;
    public string Address = "/example/1";
    public Image blackScreen; // Assign your UI Panel's Image component in Inspector
    public float fadeDuration = 2f; // Duration of fade

    [Header("OSC Settings")]
    public OSCReceiver Receiver;
    public OSCTransmitter Transmitter;

    [Header("Scene Cycling")]
    public bool enableSceneCycling = false; // Toggle to enable or disable scene cycling
    public float sceneCycleTime = 30f; // Time in seconds for each scene

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy any new instance
        }
    }

    protected virtual void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);

        // Start scene cycling if enabled
        if (enableSceneCycling)
        {
            StartCoroutine(CycleScenes());
        }
    }

    #endregion

    #region Private Methods

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        if (message.Values[0].StringValue == "scene")
        {
            StartCoroutine(FadeToBlack(message));
        }
        else
        {
            var msg = new OSCMessage("/inemotion/effect");
            Transmitter.Send(msg);
        }
    }

    void LoadNextScene(OSCMessage msg)
    {
        int intSc = msg.Values[1].IntValue;
        int nextSceneIndex = intSc % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator FadeToBlack(OSCMessage msg)
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            blackScreen.color = color;
            yield return null;
        }

        color.a = 1; // Ensure it's fully black
        blackScreen.color = color;
        LoadNextScene(msg);
        yield return new WaitForSeconds(1f);
        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            blackScreen.color = color;
            yield return null;
        }
    }

    IEnumerator CycleScenes()
    {
        while (enableSceneCycling)
        {
            // Wait for the specified amount of time before changing scenes
            yield return new WaitForSeconds(sceneCycleTime);

            // Get the current scene index and load the next one
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            StartCoroutine(FadeToBlackForCycling(nextSceneIndex));
        }
    }

    IEnumerator FadeToBlackForCycling(int nextSceneIndex)
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        // Fade to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            blackScreen.color = color;
            yield return null;
        }

        color.a = 1;
        blackScreen.color = color;
        SceneManager.LoadScene(nextSceneIndex); // Load next scene in cycle
        yield return new WaitForSeconds(1f);
        elapsedTime = 0f;

        // Fade from black back to scene
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            blackScreen.color = color;
            yield return null;
        }
    }
    #endregion
}
