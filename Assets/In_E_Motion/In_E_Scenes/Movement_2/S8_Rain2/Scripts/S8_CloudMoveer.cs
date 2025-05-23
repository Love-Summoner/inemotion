using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public float range = 10f; // Distance it moves from the center
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move left and right
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= startPosition.x + range)
                movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= startPosition.x - range)
                movingRight = true;
        }
    }
}
