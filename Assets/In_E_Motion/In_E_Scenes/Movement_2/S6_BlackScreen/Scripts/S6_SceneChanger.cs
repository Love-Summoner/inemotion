using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S6_SceneChanger : MonoBehaviour
{
    //public float delay = 10f;  // Time in seconds to wait before transitioning
    public float delay = 1f;  // Time in seconds to wait before transitioning
    public string next_scene;

    void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        SceneManager.LoadScene(next_scene);  // Load the next scene
    }
}
