using UnityEngine;

public class PS_VisualFixer : MonoBehaviour
{
    private GameObject visualizationObject;

    void Update()
    {
        // Find the Visualization_0 object if it exists
        if (visualizationObject == null)
        {
            visualizationObject = GameObject.Find("Visualization_0");
        }

        // If the object is found, adjust its size and position
        if (visualizationObject != null)
        {
            // Example of scaling it up to fill the screen
            visualizationObject.transform.localScale = new Vector3(2.42f, 1.2f, 1f);

            // Optionally, you can modify the position to center it
            visualizationObject.transform.position = new Vector3(0, 0, 0); // Adjust as needed
        }
    }
}
