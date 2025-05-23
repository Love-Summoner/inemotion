using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S1_SceneChanger : MonoBehaviour
{
    public string next_scene;

    void Start()
    {
        Debug.Log("Next scene: " + next_scene);
    }

    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Load the next scene immediately when spacebar is pressed
            SceneManager.LoadScene(next_scene);
        }
    }
}