using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video_SceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Assign this in the Inspector

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;  // Subscribe to the loopPointReached event
    }

    void EndReached(VideoPlayer vp)
    {
        LoadNextScene();  // Load the next scene when the video ends
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= EndReached;  // Always good to unsubscribe
    }
}
