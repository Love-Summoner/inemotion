using UnityEngine;

public class RevolvingCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform focalPoint; // The focal point the camera will look at
    public float radius = 5f;    // Distance from the focal point
    public float speed = 1f;     // Speed of revolution
    public float verticalAmplitude = 1f; // Vertical movement amplitude

    private float angle = 0f;    // Current angle for horizontal rotation
    private float verticalAngle = 0f; // Current angle for vertical oscillation

    void Start()
    {
        if (focalPoint == null)
        {
            Debug.LogError("Please assign a focal point for the camera.");
            return;
        }

        // Set the initial position of the camera
        transform.position = new Vector3(
            focalPoint.position.x + radius,
            focalPoint.position.y,
            focalPoint.position.z
        );

        // Look at the focal point
        transform.LookAt(focalPoint.position);
    }

    void Update()
    {
        if (focalPoint == null)
            return;

        // Update the angle for circular movement
        angle += speed * Time.deltaTime;

        // Calculate the vertical oscillation
        verticalAngle += speed * Time.deltaTime;
        float verticalOffset = Mathf.Sin(verticalAngle) * verticalAmplitude;

        // Calculate the new camera position
        float x = focalPoint.position.x + Mathf.Cos(angle) * radius;
        float z = focalPoint.position.z + Mathf.Sin(angle) * radius;
        float y = focalPoint.position.y + verticalOffset;

        transform.position = new Vector3(x, y, z);

        // Always look at the focal point
        transform.LookAt(focalPoint.position);
    }
}
