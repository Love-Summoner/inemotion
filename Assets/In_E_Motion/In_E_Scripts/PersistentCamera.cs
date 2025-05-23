using UnityEngine;

public class PersistentCamera : MonoBehaviour
{
    private static PersistentCamera instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the camera in all scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate cameras
        }
    }
}
