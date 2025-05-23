using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S0_SceneChanger : MonoBehaviour
{
    public float sceneDuration = 30f; // Default duration in seconds
    public string next_scene;      // Name of the next scene to load (optional)

    private float timer;

    void Start()
    {
        timer = sceneDuration; // Initialize timer with the scene duration
    }

    void Update()
    {
        Debug.Log("Next scene: " + next_scene);
        // Decrease the timer each frame
        timer -= Time.deltaTime;

        // If the timer reaches zero, automatically load the next scene
        if (timer <= 0)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // If `next_scene` is specified, try loading that scene
        if (!string.IsNullOrEmpty(next_scene))
        {
            SceneManager.LoadScene(next_scene);
        }
        else
        {
            // If no next scene name is set, load the next scene in build order
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        // Reset timer for the next scene, if needed
        timer = sceneDuration;
    }
}
